using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tower : MonoBehaviour, IBuildable, IUpgradeable
{
    [SerializeField] public S_Tower TowerData;
    [SerializeField] public List<GameObject> EnemyToKill = new();
    [SerializeField] public LayerMask layerAccept;
    private float _fireTimer;
    private int YRotate = 0;
    public bool isPosed = false;
    public ParticleSystem towerParticleSystem;

    private Vector3 newWorldObjectPosition;

    [Header("Tower Upgrade Panel")]
    public RectTransform image; // L'image dans le canvas
    public Canvas canvas;

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
        towerParticleSystem.transform.localPosition = new Vector3(0,1,0);
        towerParticleSystem.Stop();


        transform.position = position;
        InitializeTower(data);
        EventsManager.TowerBuilt(this, position);

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
                shootable.StartVfx(towerParticleSystem);
            }
            else
            {
                Debug.Log($"{name}: No valid enemies to attack!");
            }
        }
    }


    public void Upgrade()
    {
         transform.GetChild(0).gameObject.SetActive(true);

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);


        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPosition, canvas.worldCamera, out Vector2 localPosition);

        image.localPosition = localPosition;
    }

    private void InitializeTower(S_Tower data)
    {
        TowerData = data;
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
        Collider[] hittedObject = Physics.OverlapSphere(transform.position, TowerData.FireRange, layerAccept);
        EnemyToKill.Clear();

        foreach (var hit in hittedObject)
        {
            if (!EnemyToKill.Contains(hit.gameObject))
            {
                EnemyToKill.Add(hit.gameObject);
            }
        }

        //GameObject ok = GameObject.CreatePrimitive(PrimitiveType.Sphere); // faire sa pour mettre preview zone
        EnemyToKill.RemoveAll(enemy => enemy == null || !enemy.activeInHierarchy);

        EnemyToKill = EnemyToKill.OrderBy((enemyToFocus) => enemyToFocus.GetComponent<EnemyBehaviour>().totalDistanceToGoal).ToList();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, TowerData.FireRange);
    }

    private void HandleFiring()
    {
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= TowerData.FireRate && EnemyToKill.Count > 0)
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
            if (tile.transform.position + new Vector3(0, 1, 0) == tower.transform.position)
            {
                tile.IsOccupied = false;
                break;
            }
        }

        EventsManager.TowerDestroy(tower);
        TowerBuilder.Instance.AllTowerPosedOnMap.Remove(tower);
        Destroy(tower.gameObject);
    }

    private void OnMouseDown()
    {
        if (TowerBuilder.Instance.CanDestroyTower)
        {
            DestroyTower(this);
        }
        if(TowerBuilder.Instance.CanUpgradeTower)
        {
            Upgrade();
        }
    }
}
