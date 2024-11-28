using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour, IBuildable, IUpgradeable
{
    [System.Serializable]
    public class TowerStat
    {
        public float Damage;
        public float FireRate;
        public float FireRange;
        public int GoldsCost;
    }

    [SerializeField] public S_Tower TowerData;
    public TowerStat stat;
    [SerializeField] public List<GameObject> EnemyToKill = new();
    [SerializeField] public LayerMask layerAccept;
    private float _fireTimer;
    private int YRotate = 0;
    public bool isPosed = false;
    public ParticleSystem towerParticleSystem;


    [Header("Upgrade Count")]
    public int DmgUpgradecount;
    public int FireRateUpgradecount;
    public int RangeUpgradecount;


    [Header("Show Range")]
    public GameObject newRange;
    [SerializeField] private GameObject towerRangePrefab;
    public bool rangeCreated = false;
    [SerializeField] private Material RangeMaterial;

    //[Header("Tower Upgrade Panel")]
    //public RectTransform image; // L'image dans le canvas
    //public Canvas canvas;

    private void OnEnable()
    {
        EventsManager.OnWaveStart += ClearEnemyList;
    }

    private void OnDisable()
    {
        EventsManager.OnWaveStart -= ClearEnemyList;
    }

    private void ClearEnemyList(S_Enemy enemy, float quantity)
    {
        EnemyToKill.Clear();
    }
    public void Build(S_Tower data, Vector3 position)
    {
        GameObject vfxObject = Instantiate(data.Vfx, transform);
        towerParticleSystem = vfxObject.GetComponent<ParticleSystem>();
        towerParticleSystem.transform.localPosition = new Vector3(0, 1, 0);
        towerParticleSystem.Stop();
        transform.position = position;

        InitializeTower(data);

        EventsManager.TowerBuild(this);

        // mettre ici des effets visuel sur la construction de la tour comme vfx ou audio si commun a chaque tower
    }
    public void Fire(GameObject enemyTarget)
    {
        IShootable shootable = GetComponent<IShootable>();
        if (shootable != null)
        {
            EnemyToKill.RemoveAll(enemy => enemy == null || !enemy.activeInHierarchy);

            if (EnemyToKill.Count > 0)
            {
                shootable.Fire(enemyTarget);
            }
            else
            {
                Debug.Log($"{name}: No valid enemies to attack!");
            }
        }
    }
    public void EnemyTouched()
    {
        IShootable shootable = GetComponent<IShootable>();
        if (shootable != null)
        {
            shootable.StartVfx(towerParticleSystem);
        }
    }


    public void Upgrade()
    {
        //transform.GetChild(0).gameObject.SetActive(true);
        //Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPosition, canvas.worldCamera, out Vector2 localPosition);
        //image.localPosition = localPosition;


        TowerUpgrade.Instance.SelectTowerToUpgrade(this);
    }

    public void InitializeTower(S_Tower data)
    {
        TowerData = data;


        stat.FireRate = data.FireRate;
        stat.FireRange = data.FireRange;
        stat.GoldsCost = data.GoldsCost;
        stat.Damage = data.Damage;
    }
    public void RemoveEnemyForAllTower(GameObject enemy)
    {
        foreach (var tower in TowerBuilder.Instance.AllTowerPosedOnMap)
        {
            if (tower.EnemyToKill.Contains(enemy))
            {
                tower.EnemyToKill.Remove(enemy);
            }
        }
    }
    #region Detect Enemy

    private void Update()
    {
        if (isPosed)
        {
            UpdateEnemyList(); // Gestion des ennemis proches
            HandleFiring();    // Gestion du tir en fonction du FireRate
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            YRotate += 90;
            transform.rotation = Quaternion.Euler(0, YRotate, 0);
        }
    }

    private void UpdateEnemyList()
    {
        Collider[] hittedObject = Physics.OverlapSphere(transform.position, stat.FireRange, layerAccept);
        EnemyToKill.Clear();

        foreach (var hit in hittedObject)
        {
            if (!EnemyToKill.Contains(hit.gameObject))
            {
                EnemyToKill.Add(hit.gameObject);
            }
        }
        EnemyToKill.RemoveAll(enemy => enemy == null || !enemy.activeInHierarchy);
        EnemyToKill = EnemyToKill.OrderBy((enemyToFocus) => enemyToFocus.GetComponent<EnemyBehaviour>().totalDistanceToGoal).ToList();

        if (EnemyToKill.Count > 0)
        {
            Vector3 direction = EnemyToKill[0].transform.position - transform.position;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stat.FireRange);
    }

    private void HandleFiring()
    {
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= stat.FireRate && EnemyToKill.Count > 0)
        {
            _fireTimer = 0.0f;
            Fire(EnemyToKill[0]);
        }
    }
    #endregion

    private void DestroyTower(Tower tower)
    {
        foreach (Tile tile in TowerBuilder.Instance.TilesOccupied)
        {
            if (tile.transform.position + tower.TowerData.PosOnMap == tower.transform.position)
            {
                tile.IsOccupied = false;
                break;
            }
        }
        EventsManager.TowerDestroy(tower);
        TowerBuilder.Instance.AllTowerPosedOnMap.Remove(tower);
        TowerBuilder.Instance.CanDestroy();
        Destroy(tower.gameObject);

        //UiManager.Instance.IsActive = false;
        //UiManager.Instance.ActivateTowerInfoPanel(this);
        //DestroyRange();
    }

    private void OnMouseDown()
    {
        if (TowerBuilder.Instance.CanDestroyTower)
        {
            DestroyTower(this);
        }
        if (TowerBuilder.Instance.CanUpgradeTower)
        {
            Upgrade();
        }
    }
    public void ShowRange()
    {
        if (rangeCreated)
        {
            rangeCreated = false;
            newRange = Instantiate(towerRangePrefab);
            newRange.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            newRange.transform.localScale = new Vector3(stat.FireRange * 2, 0.1f, stat.FireRange * 2);
        }
    }
    public void DestroyRange()
    {
        Destroy(newRange);
        rangeCreated = true;
    }
    private void OnMouseOver()
    {
        if (!TowerBuilder.Instance.CanUpgradeTower)
        {
            UiManager.Instance.TowerInfoPanelIsActive = true;
            UiManager.Instance.ShowTowerInfoPanel();
            UiManager.Instance.UpdateTowerInfoPanel(this);
            ShowRange();
        }
    }
    private void OnMouseExit()
    {
        if (!TowerBuilder.Instance.CanUpgradeTower)
        {
            UiManager.Instance.TowerInfoPanelIsActive = false;
            UiManager.Instance.ShowTowerInfoPanel();
        }
        DestroyRange();
    }
    public void HideInfoPanel()
    {
        UiManager.Instance.TowerInfoPanelIsActive = false;
        UiManager.Instance.ShowTowerInfoPanel();
        DestroyRange();
    }
    private void OnDestroy()
    {
        HideInfoPanel();
    }
}
