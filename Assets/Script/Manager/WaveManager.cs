using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void StartWave()
    {
        StartCoroutine(WaveManagerCoroutine());
    }
    private IEnumerator WaveManagerCoroutine()
    {
        while (RessourceManager.StartNewWave())
        {
            if (_currentEnemytoSpawn is null) //First Wave
            {
                StartNextWave();
            }
            else // remains
            {
                yield return new WaitWhile(() => EnemyKill >= CurrentEnemyOnMap);
                StartNextWave();
            }
        }
    }
    [ContextMenu("StartNewWave")]
    public void StartNextWave()
    {
        UiAnimation.Instance.StopWaveButtonAnim();
            _wave = RessourceManager.CurrentWave;
            if (_wave % 10 == 0) //Boss
            {
                _currentEnemytoSpawn = typeEnemyToSpawn.Boss;
                _tempEnemyNumbs[_currentEnemytoSpawn] += 1;
            }
            else if (_wave % 3 == 0) //Elite
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
           CurrentEnemyOnMap = _tempEnemyNumbs[_currentEnemytoSpawn];
    }
}