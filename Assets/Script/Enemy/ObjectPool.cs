using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    #region Struct
    [System.Serializable]
    public struct EnemyParentPool
    {
        public PoolEnemy Normal;
        public PoolEnemy Elite;
        public PoolEnemy Boss;
    }
    [System.Serializable]
    public struct PoolEnemy
    {
        public GameObject Parent;
        public int InitialPoolSize;
        public GameObject Prefab;
        public EnemyType Type;
    }
    #endregion

    [Header("Enemy parent on Hierarchy")]
    [SerializeField] private EnemyParentPool EnemyParent;
    [SerializeField] private List<S_Enemy> AllType;

    public Dictionary<EnemyType, List<GameObject>> pools = new Dictionary<EnemyType, List<GameObject>>();
    private void Start()
    {
        InitializePool(EnemyParent.Normal);
        InitializePool(EnemyParent.Elite);
        InitializePool(EnemyParent.Boss);

        EventsManager.OnWaveStart += UpdateStat;
    }
    private void UpdateStat(S_Enemy _enemy, float _quantity)
    {
        foreach (S_Enemy enemy in AllType)
        {
            for (int i = 0; i < pools[enemy.type].Count; i++)
            {
                EnemyBehaviour current = pools[enemy.type][i].GetComponent<EnemyBehaviour>();

                int wave = WaveManager.Instance._waveIndex;
                if(wave != 0)
                {
                    current.stat.MaxLife = enemy.MaxLife * (enemy.MaxLifeMultiplicator == 1 ? 1 : (wave * enemy.MaxLifeMultiplicator));
                    current.stat.MoveSpeed = enemy.MoveSpeed * (enemy.MoveSpeedMultiplicator == 1 ? 1 : (wave * enemy.MoveSpeedMultiplicator));
                    current.stat.Damage = enemy.Damage * (enemy.DamageMultiplicator == 1 ? 1 : (wave * enemy.DamageMultiplicator));
                    UpdateCurrentLife(current);
                }
                else
                {
                    current.stat.MaxLife = enemy.MaxLife;
                    current.stat.MoveSpeed = enemy.MoveSpeed;
                    current.stat.Damage = enemy.Damage;
                    UpdateCurrentLife(current);
                }
            }
        }
    }

    public void UpdateCurrentLife(EnemyBehaviour current)
    {
        current.stat.CurrentLife = current.stat.MaxLife;
    }
    private void InitializePool(PoolEnemy poolData)
    {
        if (poolData.Prefab == null || poolData.Parent == null)
        {
            return;
        }

        if (!pools.ContainsKey(poolData.Type))
        {
            pools[poolData.Type] = new List<GameObject>();
        }

        for (int i = 0; i < poolData.InitialPoolSize; i++)
        {
            GameObject newEnemy = Instantiate(poolData.Prefab);
            newEnemy.name = $"{poolData.Type} ({i})";
            newEnemy.transform.parent = poolData.Parent.transform;
            newEnemy.SetActive(false);
            pools[poolData.Type].Add(newEnemy);
        }
    }

    public GameObject GetObject(EnemyType type)
    {
        if (!pools.ContainsKey(type))
        {
            return null;
        }

        foreach (GameObject enemy in pools[type])
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.SetActive(true);
                return enemy;
            }
        }

        PoolEnemy poolData = GetPoolData(type);
        if (poolData.Prefab != null)
        {
            GameObject newEnemy = Instantiate(poolData.Prefab);
            newEnemy.transform.parent = poolData.Parent.transform;
            newEnemy.SetActive(true);
            pools[type].Add(newEnemy);
            return newEnemy;
        }
        return null;
    }

    public void ReturnObject(GameObject enemy, EnemyType type)
    {
        if (!pools.ContainsKey(type))
        {
            return;
        }

        enemy.SetActive(false);
    }

    private PoolEnemy GetPoolData(EnemyType type)
    {
        return type switch
        {
            EnemyType.Normal => EnemyParent.Normal,
            EnemyType.Elite => EnemyParent.Elite,
            EnemyType.Boss => EnemyParent.Boss,
            _ => default, //  _ = wilcard = tout ce qui n'est pas deja indiquer = return default
        };
    }
}
