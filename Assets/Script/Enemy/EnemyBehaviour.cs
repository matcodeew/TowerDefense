using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private float _moveSpeed = 5f;

    [Header("Enemy Path")]
    public Vector3 _currentTarget; 
    private float _distanceThreshold = 0.1f;
    public bool IsCreate = false;

    public EnemySpawner Spawner;
    private int WaypointIndex = 0;

    private void Update()
    {
        if(IsCreate)
        {
            MoveEnemy();
        }
    }

    public void ResetEnemy()
    {
        WaypointIndex = 0;
        _currentTarget = Spawner.GetNextWaypoint(WaypointIndex).transform.position;
    }

    private void MoveEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentTarget, _moveSpeed * Time.deltaTime);

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
}