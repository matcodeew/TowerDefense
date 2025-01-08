using UnityEngine;
using static EnemyBehaviour;

public class MenusEnemy : MonoBehaviour
{

    [Header("Enemy Path")]
    [HideInInspector] public Vector3 _currentTarget;
    [HideInInspector] public bool IsCreate;
    [HideInInspector] public MenusSpawner Spawner;
    private int WaypointIndex;

    private void Update()
    {
        MoveEnemy();
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
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2);
        }

        transform.position = Vector3.MoveTowards(transform.position, _currentTarget, 1.5f * Time.deltaTime);

        if (Vector3.Distance(transform.position, _currentTarget) < 0.1)
        {
            WaypointIndex++;

            if (WaypointIndex >= Spawner.AllWaypoints.Count)
            {
                Destroy(gameObject);
            }

            _currentTarget = Spawner.GetNextWaypoint(WaypointIndex).transform.position;
        }
    }
    #endregion
}
