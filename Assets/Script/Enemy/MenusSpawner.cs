using System.Collections.Generic;
using UnityEngine;

public class MenusSpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Waypoints")]
    public List<GameObject> AllWaypoints = new();

    private float _timer = 5;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= 3.5f)
        {
            _timer = 0;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab);
        newEnemy.transform.position = transform.position;
        MenusEnemy enemyScript = newEnemy.GetComponent<MenusEnemy>();

        if (enemyScript != null)
        {
            enemyScript.Spawner = this;
            enemyScript.ResetEnemy();
        }
    }

    public GameObject GetNextWaypoint(int index)
    {
        if (index < AllWaypoints.Count)
        {
            return AllWaypoints[index];
        }

        return AllWaypoints[AllWaypoints.Count - 1];
    }
}
