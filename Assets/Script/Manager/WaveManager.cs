using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    [System.Serializable]
    public struct EnemyTypeToSpawn
    {
        public S_Enemy Normal;
        public S_Enemy Elite;
        public S_Enemy Boss;
    }

    [Header("Wave Data")]
    [SerializeField] private EnemyTypeToSpawn EnemyType;
    [SerializeField] private AllEnemyMulticator multiplicator;
    public S_Enemy EnemyToSpawn;
    private float NumbsOfNormalTemp;
    private float NumbsOfEliteTemp;
    private float NumbsOfBossTemp;
    private float NumbsOfEnemyToSpawn;
    public int CurrentEnemyOnMap;
    [HideInInspector] public WaveMultiplicator enemyAugment;
    [SerializeField] private int _waveIndex;
    [SerializeField] private int _totalWave;

    [Header("Wave Complete Condition")]
    public int EnemyKillByTower;
    public float TimeToWait;
    public bool CanStartFirstWave = false;
    private float _timer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }
    private void Start()
    {
        StartFirstWave();
    }
    private void Update()
    {
        if (!WaveIsFinish() || Ok()) return;

        _timer += Time.deltaTime;
        if (_timer >= TimeToWait)
        {
            _timer = 0.0f;
            StartNextWave();
        }
    }

    public bool WaveIsFinish() => EnemyKillByTower == (int)NumbsOfEnemyToSpawn;
    public bool Ok() => _waveIndex == _totalWave;
    public void StartFirstWave()
    {
        _waveIndex++;
        NumbsOfNormalTemp = 25;
        EnemyToSpawn = EnemyType.Normal;
        NumbsOfEnemyToSpawn = NumbsOfNormalTemp;
        EventsManager.StartNewWave(EnemyToSpawn, NumbsOfEnemyToSpawn);
    }

    [ContextMenu("StartNextWave")]
    public void StartNextWave()
    {
        CurrentEnemyOnMap = 0;
        _waveIndex++;
        EnemyKillByTower = 0;

        if (_waveIndex % 3 == 0)
        {
            EnemyToSpawn = EnemyType.Elite;
            enemyAugment = multiplicator.Elite;
            NumbsOfEliteTemp = (int)_waveIndex / 3;
            NumbsOfEnemyToSpawn = NumbsOfEliteTemp;
        }
        else if (_waveIndex % 10 == 0)
        {
            EnemyToSpawn = EnemyType.Boss;
            enemyAugment = multiplicator.Boss;
            NumbsOfBossTemp = (int)_waveIndex / 10;
            NumbsOfEnemyToSpawn = NumbsOfBossTemp;
        }
        else
        {
            EnemyToSpawn = EnemyType.Normal;
            enemyAugment = multiplicator.normal;
            NumbsOfNormalTemp *= enemyAugment.numberMultiplicator;
            NumbsOfEnemyToSpawn = NumbsOfNormalTemp;
        }
        EventsManager.StartNewWave(EnemyToSpawn, NumbsOfEnemyToSpawn);
    }

    public void UpdateEnemyStat(EnemyBehaviour enemy, WaveMultiplicator multiplicator)
    {
        enemy.stat.MaxLife *= multiplicator.StatMultiplicator.MaxLife;
        enemy.stat.MoveSpeed *= multiplicator.StatMultiplicator.MoveSpeed;
        enemy.stat.Damage *= multiplicator.StatMultiplicator.Damage;
    }
}
#region Struct Multiplicator
[System.Serializable]
public struct AllEnemyMulticator
{
    public WaveMultiplicator normal;
    public WaveMultiplicator Elite;
    public WaveMultiplicator Boss;
}

[System.Serializable]
public struct WaveMultiplicator
{
    public float numberMultiplicator;
    public EnemyStatsMultiplicator StatMultiplicator;
}
[System.Serializable]
public struct EnemyStatsMultiplicator
{
    public float MoveSpeed;
    public float MaxLife;
    public int Damage;
}
#endregion
