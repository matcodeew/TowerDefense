using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public static class EventsManager
{
    public static event UnityAction OnEnemyDie;
    public static event UnityAction<int> OnEnemyReachEnd;

    public static event UnityAction<S_Enemy, float> OnWaveStart;

    public static event UnityAction<Tower> OnTowerBuild;
    public static event UnityAction<Tower> OnTowerDestroy;
    public static void EnemyDie()
    {
        OnEnemyDie?.Invoke();
    }
    public static void ApplyBaseDamage(int EnemyDamage)
    {
        OnEnemyReachEnd?.Invoke(EnemyDamage);
    }
    public static void WaveStarted(S_Enemy enemy, float quantity)
    {
        OnWaveStart?.Invoke(enemy, quantity);
    }
    public static void TowerBuild(Tower tower)
    {
        OnTowerBuild?.Invoke(tower);
    }
    public static void TowerDestroy(Tower tower)
    {
        OnTowerDestroy?.Invoke(tower);
    }
}
