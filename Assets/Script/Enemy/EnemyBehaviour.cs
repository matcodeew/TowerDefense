using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    #region Struct
    [System.Serializable]
    public class Stat
    {
        public float MaxLife;
        public float CurrentLife;
        public int Damage;
        public float MoveSpeed;
    }
    #endregion

    [Header("Enemy Stats")]
    public S_Enemy EnemyData;
    public float totalDistanceToGoal;
    public Stat stat;

    [Header("Enemy Path")]
    [HideInInspector] public Vector3 _currentTarget;
    [HideInInspector] public bool IsCreate;
    [HideInInspector] public EnemySpawner Spawner; 
    private int WaypointIndex;
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
        if (stat.CurrentLife <= stat.MaxLife && stat.CurrentLife > 0)
        {
            stat.CurrentLife = Mathf.Clamp(stat.CurrentLife - damage, 0, stat.MaxLife);

            //print($"{(damage < 0 ? $"receive health {-damage}" : $"receive damage {damage}")}," +
            //      $" health remaining {stat.CurrentLife} / { stat.MaxLife}");
            if (stat.CurrentLife <= 0)
            {
                Die(tower);
            }
        }
    }
    private void Die(Tower tower)
    {
        RessourceManager.AddGold(EnemyData.goldValue);
        WaveManager.Instance.EnemyKill++;
        
        tower.RemoveEnemyForAllTower(gameObject);
        Spawner.ReturnEnemyToPool(gameObject, EnemyData.type);
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

        if (Vector3.Distance(transform.position, _currentTarget) < 0.1)
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