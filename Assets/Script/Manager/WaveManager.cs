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
    private float progressValue;

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
        if (!StartingWaveTimer() || LevelFinished()) return;
        _timer += Time.deltaTime;

        progressValue = _timer / TimeToWait;
        UiManager.Instance.ProgressBar.fillAmount = progressValue;
        if (_timer >= TimeToWait * 0.50f && _timer < TimeToWait)
        {
            UiAnimation.Instance.StartWaveButtonAnim();
        }
        if (_timer >= TimeToWait)
        {
            progressValue = 0;
            UiManager.Instance.ProgressBar.fillAmount = progressValue;
            _timer = 0.0f;
            StartNextWave();
        }
    }

    public void LunchGame()
    {
        CanStartFirstWave = true;
        progressValue = 0;
        UiManager.Instance.ProgressBar.fillAmount = progressValue;
        UiAnimation.Instance.StopWaveButtonAnim();
    }

    public bool StartingWaveTimer() => EnemyKill >= (int)NumbsOfEnemyToSpawn / 2;

    public bool LevelFinished() => _waveIndex >= RessourceManager.Instance.MaxWave;
    public void StartNextWave()
    {
        UiAnimation.Instance.StopWaveButtonAnim();
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
                NumbsOfNormalTemp += 15;
                NumbsOfEnemyToSpawn = NumbsOfNormalTemp;
            }
        }
        EventsManager.WaveStarted(EnemyData, NumbsOfEnemyToSpawn);
    }
}