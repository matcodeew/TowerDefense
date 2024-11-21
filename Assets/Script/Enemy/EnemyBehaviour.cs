using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Enemy Stats")]
    [HideInInspector] public S_Enemy EnemyData;

    [Header("Enemy Path")]
    [HideInInspector] public Vector3 _currentTarget;
    [HideInInspector] private float _distanceThreshold = 0.1f;
    [HideInInspector] public bool IsCreate = false;
    [HideInInspector] public EnemySpawner Spawner;
    [HideInInspector] private int WaypointIndex = 0;

    private void Awake()
    {
        EventsManager.OnTowerShooting += TakeDamage;

    }
    private void Update()
    {
        if(IsCreate)
        {
            MoveEnemy();
        }
    }

    public void TakeDamage(IShootable tower, GameObject enemyKill)
    {
        Tower _tower = tower as Tower;
        if (_tower != null)
        {
            if (EnemyData.CurrentLife <= EnemyData.MaxLife)
            {
                EnemyData.CurrentLife -= Mathf.Clamp(_tower.TowerData.Damage, 0, EnemyData.MaxLife);
            }
            else
            {
                Die();
            }
        }
    }
    private void Die()
    {
        Destroy(gameObject);
        RessourceManager.Instance.currentGold += EnemyData.goldValue;
    }
    #region Enemy Movement
    public void ResetEnemy()
    {
        WaypointIndex = 0;
        _currentTarget = Spawner.GetNextWaypoint(WaypointIndex).transform.position;
    }
    private void MoveEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentTarget, EnemyData.MoveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _currentTarget) < _distanceThreshold)
        {
            WaypointIndex++;

            if (WaypointIndex < Spawner.AllWaypoints.Count)
            {
                _currentTarget = Spawner.GetNextWaypoint(WaypointIndex).transform.position;
            }
            else
            {
                Spawner.ReturnEnemyToPool(gameObject);
            }
        }
    }
    #endregion
}