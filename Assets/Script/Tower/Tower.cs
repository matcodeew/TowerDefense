using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour, IBuildable, IUpgradeable
{
    [SerializeField] public S_Tower TowerData;
    [SerializeField] public List<GameObject> EnemyToKill = new();
    [SerializeField] public LayerMask layerAccept;
    private float _fireTimer;
    private int YRotate = 0;
    public bool isPosed = false;

    private Vector3 halfExtent = new Vector3(0, 0, 2);
    public void Build(S_Tower data, Vector3 position)
    {
        transform.position = position;
        InitializeTower(data);
        EventsManager.TowerBuilt(this, position);

        // mettre ici des effets visuel sur la construction de la tour comme vfx ou audio
    }
    public void Fire(GameObject enemyTarget)
    {
        IShootable shootable = GetComponent<IShootable>();
        if (shootable != null)
        {
            shootable.Fire(enemyTarget);
        }
    }

    public void Upgrade()
    {
        //Upgrate Tower 
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
        Collider[] hittedObject = null;
        if (GetComponent<T_FlameThrower>() == null)
        {
            hittedObject = Physics.OverlapSphere(transform.position, TowerData.FireRange, layerAccept);
        }
        else
        {
            hittedObject = Physics.OverlapBox(transform.position + (transform.forward * 2), halfExtent, TowerBuilder.Instance.previewRotation, layerAccept);
        }
        EnemyToKill.Clear();

        foreach (var hit in hittedObject)
        {
            if (!EnemyToKill.Contains(hit.gameObject))
            {
                EnemyToKill.Add(hit.gameObject);
            }
        }

        EnemyToKill = EnemyToKill.OrderBy((enemyToFocus) => enemyToFocus.GetComponent<EnemyBehaviour>().totalDistanceToGoal).ToList();
    }
    private void OnDrawGizmos()
    {
        if (GetComponent<T_FlameThrower>() == null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, TowerData.FireRange);
        }
        else
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position + (transform.forward * 2), transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, halfExtent);
        }
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
    }
}
