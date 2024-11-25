using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TowerFire))]
public class Tower : MonoBehaviour, IBuildable, IUpgradeable, IShootable
{
    [SerializeField] public S_Tower TowerData;
    [SerializeField] public List<GameObject> EnemyToKill = new();
    [SerializeField] private LayerMask layerAccept;

    public bool isPosed = false;
    public void Build(S_Tower data, Vector3 position)
    {
        transform.position = position;
        InitializeTower(data);
        EventsManager.TowerBuilt(this, position);

        // mettre ici des effets visuel sur la construction de la tour comme sfx ou audio
    }
    public void Fire()
    {
        // mettre ici le projectile, l'audio ou le sfx
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
            Collider[] hittedObject = Physics.OverlapSphere(transform.position, TowerData.FireRange, layerAccept);

            foreach (var hit in hittedObject)
            {
                if (Vector3.Distance(transform.position, hit.transform.position) >= TowerData.FireRange)
                {
                    if (EnemyToKill.Contains(hit.gameObject))
                    {
                        EnemyToKill.Remove(hit.gameObject);
                    }
                    break;
                }
                if (!EnemyToKill.Contains(hit.gameObject))
                {
                    EnemyToKill.Add(hit.gameObject);
                }
            }
            EnemyToKill = EnemyToKill.OrderBy((enemyToFocus) => enemyToFocus.GetComponent<EnemyBehaviour>().totalDistanceToGoal).ToList();
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
        if(TowerBuilder.Instance.CanDestroyTower)
        {
            DestroyTower(this);
        }
    }
}
