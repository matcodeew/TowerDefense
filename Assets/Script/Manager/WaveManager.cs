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
    [SerializeField] public EnemyTypeToInstantiate EnemyToInstantiate;
    private S_Enemy EnemyData;

    public int _waveIndex = -1;

    [Header("Wave Complete Condition")]
    [SerializeField] private float TimeToWait;
    private float _timer;
    public bool CanStartFirstWave = false;
    private bool IsFirstWave = true;

    [Header("Debug Enemy")]
    [SerializeField] private float NumbsOfEnemyToSpawn;
    public int CurrentEnemyOnMap;
    public int EnemyKill;

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
        EventsManager.WaveStarted(EnemyData, NumbsOfEnemyToSpawn);
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

    public bool WaveIsFinish() => EnemyKill >= (int)NumbsOfEnemyToSpawn;
    public bool LevelFinished() => _waveIndex >= RessourceManager.Instance.MaxWave;
    public void StartNextWave()
    {
        CurrentEnemyOnMap = 0;
        EnemyKill = 0;
        if (!IsFirstWave)
        {
            _waveIndex++;
            if (_waveIndex % 3 == 0) // spawn elite
            {
                EnemyData = EnemyToInstantiate.Elite;
                NumbsOfEliteTemp = (int)_waveIndex / 3;
                NumbsOfEnemyToSpawn = NumbsOfEliteTemp;
            }
            else if (_waveIndex % 10 == 0) // spawn boss
            {
                EnemyData = EnemyToInstantiate.Boss;
                NumbsOfBossTemp = (int)_waveIndex / 10;
                NumbsOfEnemyToSpawn = NumbsOfBossTemp;
            }
            else // spawn Normal
            {
                EnemyData = EnemyToInstantiate.Normal;
                NumbsOfNormalTemp *= 1.5f;
                NumbsOfEnemyToSpawn = NumbsOfNormalTemp;
            }
        }
        EventsManager.WaveStarted(EnemyData, NumbsOfEnemyToSpawn);
    }
}