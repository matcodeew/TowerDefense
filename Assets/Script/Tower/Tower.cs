using NUnit.Framework;
using System.Collections.Generic;
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
    }
    public void Fire()
    {
        //instantiate projectile or SFX
        print($"tower {name} shoot on {EnemyToKill[EnemyToKill.Count - 1].name}");
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
