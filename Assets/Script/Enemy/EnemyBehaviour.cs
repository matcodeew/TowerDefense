using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [System.Serializable]
    public class Stat
    {
        public float MaxLife;
        public float CurrentLife;
        public int Damage;
        public float MoveSpeed;
    }

    [Header("Enemy Stats")]
    /*[HideInInspector]*/
    public S_Enemy EnemyData;
    public float totalDistanceToGoal;
    public Stat stat;

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
            UpdateDistanceToGoal();
        }
    }
    public void TakeDamage(Tower tower, float damage)
    {
        EnemyBehaviour enemyBehaviour = GetComponent<EnemyBehaviour>();

        if (enemyBehaviour.stat.CurrentLife <= enemyBehaviour.stat.MaxLife && enemyBehaviour.stat.CurrentLife > 0)
        {
            enemyBehaviour.stat.CurrentLife -= damage;
            if (enemyBehaviour.stat.CurrentLife < 0)
            {
                Die(tower);
            }
        }
        else
        {
            Die(tower);
        }
    }
    private void Die(Tower tower)
    {
        RessourceManager.AddGold(EnemyData.goldValue);
        tower.RemoveEnemyForAllTower(gameObject);
        Spawner.ReturnEnemyToPool(gameObject, EnemyData.type);
        WaveManager.Instance.EnemyKill++;
        EventsManager.EnemyDie();
    }
    #region Enemy Movement
    public void ResetEnemy()
    {
        WaypointIndex = 0;
        _currentTarget = Spawner.GetNextWaypoint(WaypointIndex).transform.position;
    }
    private void MoveEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentTarget, stat.MoveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _currentTarget) < _distanceThreshold)
        {
            WaypointIndex++;

            if (WaypointIndex < Spawner.AllWaypoints.Count)
            {
                _currentTarget = Spawner.GetNextWaypoint(WaypointIndex).transform.position;
            }
            else
            {
                EventsManager.ApplyBaseDamage(-stat.Damage);
                Spawner.ReturnEnemyToPool(gameObject, GetComponent<EnemyBehaviour>().EnemyData.type);
                WaveManager.Instance.EnemyKill++;
            }
        }
    }
    #endregion

    private void UpdateDistanceToGoal()
    {
        totalDistanceToGoal = 0f;
        for (int i = WaypointIndex; i < Spawner.AllWaypoints.Count - 1; i++)
        {
            totalDistanceToGoal += Vector3.Distance(Spawner.AllWaypoints[i].transform.position, Spawner.AllWaypoints[i + 1].transform.position);
        }
        if (WaypointIndex < Spawner.AllWaypoints.Count)
        {
            totalDistanceToGoal += Vector3.Distance(transform.position, Spawner.AllWaypoints[WaypointIndex].transform.position);
        }
    }
}