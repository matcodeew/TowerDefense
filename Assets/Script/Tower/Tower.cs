using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TowerFire))]
public class Tower : MonoBehaviour, IBuildable, IUpgradeable, IShootable
{
    [HideInInspector] public S_Tower TowerData;
    [SerializeField] public List<GameObject> EnemyToKill = new();
    public void Build(S_Tower data, Vector3 position)
    {
        transform.position = position;
        InitializeTower(data);
        GetComponent<SphereCollider>().enabled = true;
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
        GetComponent<SphereCollider>().radius = TowerData.FireRange;
    }
    public void RemoveEnemyForAllTower(GameObject enemy)
    {
        foreach (var tower in TowerBuilder.Instance.AllTowerPosedOnMap)
        {
            if(tower.EnemyToKill.Contains(enemy))
            {
                tower.EnemyToKill.Remove(enemy);
            }
        }
    }
    #region Detect Enemy
    private void OnTriggerEnter(Collider other) // detect enemy
    {
        if(!EnemyToKill.Contains(other.gameObject))
        {
            EnemyToKill.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (EnemyToKill.Contains(other.gameObject))
        {
            EnemyToKill.Remove(other.gameObject);
        }
    }
    #endregion
}
