using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Tower : Building
{
    [System.Serializable]
    public class TowerStat
    {
        public float FireRate;
        public float FireRange;
        public float GoldsCost;
        public float Damage;
    }

    [Header("Tower Stats")]
    [SerializeField] public TowerStat stat = new TowerStat();
    [SerializeField] public S_Tower towerData;

    [Header("Tower Data")]
    [HideInInspector] protected List<GameObject> EnemyToKill = new();
    [SerializeField] protected LayerMask layerAccept;

    [Header("Internal Variables")]
    private float yRotate;
    private Vector3 direction;
    private float _fireTimer;

    private void Awake()
    {
        layerAccept = EnemySpawner.Instance.EnemyMask;
    }
    private void Update()
    {
        UpdateEnemyList();
        HandleFiring();

        if (Input.GetKeyUp(KeyCode.R))
        {
            yRotate += 90;
            transform.rotation = Quaternion.Euler(0, yRotate, 0);
        }
    }
    protected virtual void InitializeTowerStats(S_Tower data)
    {
        towerData = data;
        stat = data.GetTowerStats();
    }
    protected abstract void Fire(GameObject enemyToKill);


    public virtual void BuildTower(S_Tower towerToInstantiate, Vector3 position)
    {
        GameObject newTower = Build(towerToInstantiate.Prefab, position + towerToInstantiate.PosOnMap);
        Tower towerBehaviour = newTower.GetComponent<Tower>();
        if (towerBehaviour is not null)
        {
            towerBehaviour.InitializeTowerStats(towerToInstantiate);
        }
    }

    private void UpdateEnemyList()
    {
        Collider[] hittedObject = Physics.OverlapSphere(transform.position, stat.FireRange, layerAccept);
        EnemyToKill.Clear();

        if (hittedObject.Length <= 0) { return; }

        foreach (var hit in hittedObject)
        {
            if (!EnemyToKill.Contains(hit.gameObject))
            {
                EnemyToKill.Add(hit.gameObject);
            }
        }

        EnemyToKill.RemoveAll(enemy => enemy is null || !enemy.activeInHierarchy);
        EnemyToKill = EnemyToKill.OrderBy((enemyToFocus) => enemyToFocus.GetComponent<EnemyBehaviour>().totalDistanceToGoal).ToList();
        RotateTower();
    }

    protected virtual void RotateTower()
    {
        if (EnemyToKill.Count > 0)
        {
            direction = EnemyToKill[0].transform.position - transform.position;
        }
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float rotationSpeed = 5f;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void RemoveEnemyForAllTower(GameObject enemy)
    {

    }

    private void HandleFiring()
    {
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= stat.FireRate && EnemyToKill.Count > 0)
        {
            _fireTimer = 0.0f;
            Fire(EnemyToKill[0]);
        }
    }
}
