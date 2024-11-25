using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [System.Serializable]
    public struct EnemyTypeToInstantiate
    {
        public S_Enemy Normal;
        public S_Enemy Elite;
        public S_Enemy Boss;
    }

    [Header("Wave Data")]
    private float NumbsOfNormalTemp;
    private float NumbsOfEliteTemp;
    private float NumbsOfBossTemp;
    private float NumbsOfEnemyToSpawn;
    public int CurrentEnemyOnMap;
    [SerializeField] private EnemyTypeToInstantiate EnemyToInstantiate;
    private S_Enemy EnemyData;

    public int _waveIndex = -1;
    [SerializeField] private int _totalWave;

    [Header("Wave Complete Condition")]
    [SerializeField] private float TimeToWait;
    private float _timer;
    public int EnemyKill;
    public bool CanStartFirstWave = false;
    private bool IsFirstWave = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        IsFirstWave = false;
        NumbsOfNormalTemp = 25;
        NumbsOfEnemyToSpawn = NumbsOfNormalTemp;
        EnemyData = EnemyToInstantiate.Normal;
        EventsManager.StartNewWave(EnemyData, NumbsOfEnemyToSpawn);
    }
    private void Update()
    {
        if (!WaveIsFinish() || LevelFinished()) return;

        _timer += Time.deltaTime;
        if (_timer >= TimeToWait)
        {
            _timer = 0.0f;
            StartNextWave();
        }
    }

    public bool WaveIsFinish() => EnemyKill == (int)NumbsOfEnemyToSpawn;
    public bool LevelFinished() => _waveIndex == _totalWave;
    public void StartNextWave()
    {
        CurrentEnemyOnMap = 0;
        EnemyKill = 0;
        if(!IsFirstWave)
        {
            if (_waveIndex % 3 == 0) // spawn elite
            {
                _waveIndex++;
                EnemyData = EnemyToInstantiate.Elite;
                NumbsOfEliteTemp = (int)_waveIndex / 3;
                NumbsOfEnemyToSpawn = NumbsOfEliteTemp;
            }
            else if (_waveIndex % 10 == 0) // spawn boss
            {
                _waveIndex++;
                EnemyData = EnemyToInstantiate.Boss;
                NumbsOfBossTemp = (int)_waveIndex / 10;
                NumbsOfEnemyToSpawn = NumbsOfBossTemp;
            }
            else // spawn Normal
            {
                _waveIndex++;
                EnemyData = EnemyToInstantiate.Normal;
                NumbsOfNormalTemp *= 1.5f;
                NumbsOfEnemyToSpawn = NumbsOfNormalTemp;
            }

        }
        EventsManager.StartNewWave(EnemyData, NumbsOfEnemyToSpawn);
    }
}