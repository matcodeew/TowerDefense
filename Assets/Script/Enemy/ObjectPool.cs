using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Enemy parent on Hierarchy")]
    [SerializeField] private GameObject EnemyParent;

    [HideInInspector] public GameObject prefab;
    [HideInInspector] public int initialPoolSize;
    private List<GameObject> pool = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject newEnemy = Instantiate(prefab);
            newEnemy.transform.parent = EnemyParent.transform;
            newEnemy.SetActive(false);
            pool.Add(newEnemy);
        }
    }

    public GameObject GetObject()
    {
        foreach (GameObject enemy in pool)
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.SetActive(true);
                return enemy;
            }
        }
        GameObject newEnemy = Instantiate(prefab);
        newEnemy.transform.parent = EnemyParent.transform;
        newEnemy.SetActive(true);
        pool.Add(newEnemy);
        return newEnemy;
    }

    public void ReturnObject(GameObject enemy)
    {
        enemy.SetActive(false);
    }
}
