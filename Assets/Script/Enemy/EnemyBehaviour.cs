using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static DebuffLibrary;

public class EnemyBehaviour : MonoBehaviour
{
    #region Struct
    [System.Serializable]
    public class Stat
    {
        public float MaxLife;
        public float CurrentLife;
        public float Damage;
        public float MoveSpeed;
    }
    #endregion

    private DebuffLibrary debuffLib;

    [Header("Enemy Stats")]
    [SerializeField] private Canvas canva;
    [SerializeField] private Image enemyHealth;
    public S_Enemy EnemyData;
    public float totalDistanceToGoal;
    public Stat stat;
    public bool HasDOT;
    private bool applyDot;

    [Header("Enemy Path")]
    [HideInInspector] public Vector3 _currentTarget;
    [HideInInspector] public bool IsCreate;
    [HideInInspector] public EnemySpawner Spawner;
    private int WaypointIndex;
    private void Awake()
    {
        debuffLib = DebuffLibrary.Instance;
    }
    private void Update()
    {
        if (IsCreate)
        {
            MoveEnemy();
            UpdateDistanceToGoal();
        }
    }
    public void ApplyDebuff(DebuffType type, float damage)
    {
        StartCoroutine(ApplyDebuffRoutine(type, damage));
    }
    private IEnumerator ApplyDebuffRoutine(DebuffType type, float damage)
    {
        if (HasDOT && !applyDot)
        {
            if (debuffLib.debuffs.TryGetValue(type, out DebuffDuration debuffStats))
            {
                switch (type)
                {
                    case DebuffType.Fire:
                        applyDot = true;
                        for (int i = 0; i < debuffStats.duration; i++)
                        {
                            yield return new WaitForSeconds(1f);
                            TakeDamage(damage);
                        }
                        HasDOT = false;
                        applyDot = false;
                        break;
                }
            }
        }
    }
    public void TakeDamage(float damage)
    {
        if (stat.CurrentLife <= stat.MaxLife && stat.CurrentLife > 0)
        {
            stat.CurrentLife = Mathf.Clamp(stat.CurrentLife - damage, 0, stat.MaxLife);
            enemyHealth.fillAmount = stat.CurrentLife / stat.MaxLife;
            if (stat.CurrentLife <= 0)
            {
                Die();
            }
        }
    }
    private void Die()
    {
        RessourceManager.AddGold(EnemyData.goldValue);
        WaveManager.Instance.EnemyKill++;
        Spawner.ReturnEnemyToPool(gameObject, EnemyData.type);
        HasDOT = false;
        applyDot = false;
        enemyHealth.fillAmount = 1;
        //   EventsManager.EnemyDie();
    }

    #region Enemy Movement
    public void ResetEnemy()
    {
        WaypointIndex = 0;
        _currentTarget = Spawner.GetNextWaypoint(WaypointIndex).transform.position;
    }
    private void MoveEnemy()
    {
        Vector3 direction = (_currentTarget - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float currentAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle + 90, Time.deltaTime * 10);
            transform.eulerAngles = new Vector3(0, currentAngle, 0);
            canva.transform.LookAt(Camera.main.transform);
        }


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
                RessourceManager.BaseTakeDamage((int)stat.Damage);
                Spawner.ReturnEnemyToPool(gameObject, GetComponent<EnemyBehaviour>().EnemyData.type);
                WaveManager.Instance.EnemyKill++;
            }
        }
    }

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
    #endregion
}