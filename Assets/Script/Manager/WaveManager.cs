using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    
    #region Struct
    [Serializable]
    public struct EnemyData
    {
        public S_Enemy Normal;
        [Space(1)]
        public S_Enemy Elite;
        [Space(1)]
        public S_Enemy Boss;
    }

    #endregion
    
    [Header("Enemy Data")]
    [SerializeField] public EnemyData typeEnemyToSpawn;
    private S_Enemy _currentEnemytoSpawn;

    [Header("Nums Enemy To Spawn")]
    private Dictionary<S_Enemy, int> _tempEnemyNumbs = new Dictionary<S_Enemy, int>();

    [Header("Wave Management")]
    private int _wave;
    public int EnemyKill;
    public int CurrentEnemyOnMap;
    [Space(5)]
    [SerializeField] private float timetoWaitBeforeNextWave;
    private bool firstWaveStarted;
    private float timer;
    public bool shouldCreateEnemy = false;
    private void Awake()
    {
        if(Instance is null) { Instance = this; }
    }
    private void Start()
    {
        InitializeDictionary();
    }
    private void InitializeDictionary()
    {
        foreach (var field in typeof(EnemyData).GetFields())
        {
            S_Enemy enemy = (S_Enemy)field.GetValue(typeEnemyToSpawn);
            _tempEnemyNumbs.Add(enemy, 0);
        }
    }
    public bool WaveFinished() => EnemyKill >= _tempEnemyNumbs[_currentEnemytoSpawn];
    public bool LevelFinished() => RessourceManager.CurrentWave >= RessourceManager.MaxWave;
    public void LunchGame()
    {
        UiManager.Instance.ProgressBar.fillAmount = 0;
        UiAnimation.Instance.StopWaveButtonAnim();
        StartNextWave();
        UiManager.Instance.waveIndication.GetComponent<Button>().enabled = false;
        firstWaveStarted = true;
    }

    private void Update()
    {
        if (!firstWaveStarted) return;
        if (WaveFinished() && !LevelFinished())
        {
            timer += Time.deltaTime;

            UiManager.Instance.ProgressBar.fillAmount = timer / timetoWaitBeforeNextWave;
            if (timer >= timetoWaitBeforeNextWave * 0.50f && timer < timetoWaitBeforeNextWave)
            {
                UiAnimation.Instance.StartWaveButtonAnim();
            }

            if (timer >= timetoWaitBeforeNextWave)
            {
                UiManager.Instance.ProgressBar.fillAmount = 0;
                timer = 0.0f;
                StartNextWave();
            }
        }
    }

    [ContextMenu("StartNewWave")]
    public void StartNextWave()
    {
        if (RessourceManager.StartNewWave())
        {
            CurrentEnemyOnMap = 0;
            EnemyKill = 0;
            UiAnimation.Instance.StopWaveButtonAnim();
            _wave = RessourceManager.CurrentWave;
            if (_wave % 10 == 0) //Boss
            {
                _currentEnemytoSpawn = typeEnemyToSpawn.Boss;
                _tempEnemyNumbs[_currentEnemytoSpawn] += 1;
            }
            else if (_wave % 4 == 0) //Elite
            {
                _currentEnemytoSpawn = typeEnemyToSpawn.Elite;
                _tempEnemyNumbs[_currentEnemytoSpawn] += 1;
            }
            else // Normal
            {
                _currentEnemytoSpawn = typeEnemyToSpawn.Normal;
                _tempEnemyNumbs[_currentEnemytoSpawn] += 10;
            }

            print($"enemy to instantiate {_currentEnemytoSpawn.name} on this quantity {_tempEnemyNumbs[_currentEnemytoSpawn]}");
            EventsManager.StartNewWave(_currentEnemytoSpawn, _tempEnemyNumbs[_currentEnemytoSpawn]);
            shouldCreateEnemy = true;
        }
    }
}