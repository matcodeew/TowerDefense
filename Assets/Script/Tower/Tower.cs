using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Tower : Building
{
    [System.Serializable]
    public class TowerStat
    {
        public float FireRate;
        public float FireRange;
        public float Damage;
    }

    [Header("Tower Stats")]
    [SerializeField] public TowerStat stat = new TowerStat();
    [SerializeField] public S_Tower towerData;

    [Header("Tower Data")]
    protected List<GameObject> EnemyToKill = new();
    protected LayerMask layerAccept;

    [Header("Internal Variables")]
    private Vector3 direction;
    private float _fireTimer;

    [Header("Tower Upgrades Settings")]
    public int dmgUpgradecount = 0;
    public int fireRateUpgradecount = 0;
    public int rangeUpgradecount = 0;

    [Header("Tower Range")]
    [SerializeField] public GameObject towerRange;

    private void Start()
    {
        layerAccept = EnemySpawner.Instance.EnemyMask;
        towerRange = CreateTowerRange(transform);
    }
    private void Update()
    {
        UpdateEnemyList();
        HandleFiring();
    }

    public override void Upgrade()
    {
        base.Upgrade();
        TowerUpgrade.Instance.SelectTowerToUpgrade(this);
    }

    public override void DestroyBuilding()
    {
        base.DestroyBuilding();
        TowerUpgrade.Instance.towerToUpgrade = null;
    }

    protected virtual void Fire(GameObject enemyToKill)
    {
        StartHittedEnemyVfx(enemyToKill);
    }
    protected abstract void StartHittedEnemyVfx(GameObject enemyToKill);
    protected virtual void StartingVfxInRange()
    {
    }
    protected virtual void StopingVfxInRange()
    {
    }

    protected virtual void InitializeTowerStats(S_Tower data)
    {
        towerData = data;
        stat = data.GetTowerStats();
        buildingStat = data.GetBuildingStats();
        dmgUpgradecount = 0;
        fireRateUpgradecount = 0;
        rangeUpgradecount = 0;
    }

    public virtual GameObject BuildTower(S_Tower towerToInstantiate, Transform transform, Transform parent, Tile buildOnTile)
    {
        transform.position += towerToInstantiate.PosOnMap;
        GameObject newTower = Build(towerToInstantiate.Prefab, transform, towerToInstantiate.GoldsCost, buildOnTile);
        newTower.transform.parent = parent;
        Tower towerBehaviour = newTower.GetComponent<Tower>();
        if (towerBehaviour is not null)
        {
            towerBehaviour.InitializeTowerStats(towerToInstantiate);
        }
        return newTower;
    }

    public GameObject CreateTowerRange(Transform towerToInstantiate)
    {
        GameObject newRange = Instantiate(UiManager.Instance.towerRange, towerToInstantiate);
        newRange.SetActive(false);
        newRange.transform.position = new Vector3(towerToInstantiate.position.x, 0.5f, towerToInstantiate.position.z);
        UpdateTowerRange(newRange);
        return newRange;
    }
    public void UpdateTowerRange(GameObject towerRange)
    {
        towerRange.transform.localScale = new Vector3(stat.FireRange * 2, 0.1f, stat.FireRange * 2);
    }

    private void UpdateEnemyList()
    {
        Collider[] hittedObject = Physics.OverlapSphere(transform.position, stat.FireRange, layerAccept);
        EnemyToKill.Clear();

        if (hittedObject.Length <= 0) { StopingVfxInRange(); return; }
        StartingVfxInRange();

        foreach (var hit in hittedObject)
        {
            if (!EnemyToKill.Contains(hit.gameObject))
            {
                EnemyToKill.Add(hit.gameObject);
            }
        }

        EnemyToKill.RemoveAll(enemy => enemy is null || !enemy.activeInHierarchy);
        EnemyToKill = EnemyToKill.OrderBy((enemyToFocus) => enemyToFocus.GetComponent<EnemyBehaviour>().totalDistanceToGoal).ToList();
        RotateTower();
    }

    protected virtual void RotateTower()
    {
        if (EnemyToKill.Count > 0)
        {
            direction = EnemyToKill[0].transform.position - transform.position;
        }
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float rotationSpeed = 5f;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
    private void HandleFiring()
    {
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= stat.FireRate)
        {
            _fireTimer = 0.0f;
            if (EnemyToKill.Count > 0)
            {
                Fire(EnemyToKill[0]);
            }
        }
    }

    private void OnMouseOver()
    {
        if (!TowerBuilderManager.Instance.CanUpgradeTower && !TowerBuilderManager.Instance.DragTower)
        {
            UiManager.Instance.TowerInfoPanelIsActive = true;
            UiManager.Instance.ShowTowerInfoPanel();
            UiManager.Instance.UpdateTowerInfoPanel(this);
            ShowRange();
        }
    }
    private void OnMouseExit()
    {
        if (!TowerBuilderManager.Instance.CanUpgradeTower)
        {
            UiManager.Instance.TowerInfoPanelIsActive = false;
            UiManager.Instance.ShowTowerInfoPanel();
            DestroyRange();
        }
    }
    public void HideInfoPanel()
    {
        UiManager.Instance.TowerInfoPanelIsActive = false;
        UiManager.Instance.ShowTowerInfoPanel();
        DestroyRange();
    }

    public void ShowRange()
    {
        towerRange.SetActive(true);
        UpdateTowerRange(towerRange);
    }

    public void DestroyRange()
    {
        towerRange.SetActive(false);
    }
    private void OnDestroy()
    {
        HideInfoPanel();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stat.FireRange);
    }
}
