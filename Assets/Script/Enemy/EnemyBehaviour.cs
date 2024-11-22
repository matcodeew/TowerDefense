using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [System.Serializable]
    public struct EnemyStat
    {
        public float MoveSpeed;
        public float CurrentLife;
        public float MaxLife;
        public int Damage;
    }

    [Header("Enemy Stats")]
    [HideInInspector] public S_Enemy EnemyData;
    public EnemyStat stat;

    [Header("Enemy Path")]
    [HideInInspector] public Vector3 _currentTarget;
    [HideInInspector] private float _distanceThreshold = 0.1f;
    [HideInInspector] public bool IsCreate = false;
    [HideInInspector] public EnemySpawner Spawner;
    [HideInInspector] private int WaypointIndex = 0;
    private void Update()
    {
        if (IsCreate)
        {
            MoveEnemy();
        }
    }

    private void Awake()
    {
        EventsManager.OnWaveStart += UpdateStats;
    }
    private void Start()
    {
        stat.MaxLife = EnemyData.MaxLife;
        stat.CurrentLife = stat.MaxLife;
        stat.MoveSpeed = EnemyData.MoveSpeed;
        stat.Damage = EnemyData.Damage;
    }

    private void UpdateStats(S_Enemy enemy, float quantity)
    {
        WaveManager.Instance.UpdateEnemyStat(this, WaveManager.Instance.enemyAugment);
    }
    public void TakeDamage(IShootable tower, GameObject enemyKill)
    {
        Tower _tower = tower as Tower;
        EnemyBehaviour enemyBehaviour = enemyKill.GetComponent<EnemyBehaviour>();

        if (_tower != null)
        {
            if (stat.CurrentLife <= EnemyData.MaxLife)
            {
                if (enemyBehaviour.stat.CurrentLife > 0)
                {
                    enemyBehaviour.stat.CurrentLife -= Mathf.Clamp(_tower.TowerData.Damage, 0, EnemyData.MaxLife);
                }
            }
            if(enemyBehaviour.stat.CurrentLife <= 0)
            {
                Die(_tower, enemyKill);
            }
        }
    }
    private void Die(Tower tower, GameObject enemyKill)
    {
        Spawner.ReturnEnemyToPool(enemyKill, enemyKill.GetComponent<EnemyBehaviour>().EnemyData.type);
        RessourceManager.Instance.currentGold += EnemyData.goldValue;
        tower.RemoveEnemyForAllTower(enemyKill);
        WaveManager.Instance.EnemyKillByTower++;
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
                EventsManager.ChangeBaseValue(-stat.Damage);
                Spawner.ReturnEnemyToPool(gameObject, gameObject.GetComponent<EnemyBehaviour>().EnemyData.type);
            }
        }
    }
    #endregion
}