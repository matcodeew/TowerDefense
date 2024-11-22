using UnityEngine;
using UnityEngine.Events;

public static class EventsManager
{
    #region Tower Event
    public static event UnityAction<IBuildable, Vector3> OnTowerBuilt;
    public static event UnityAction<IUpgradeable> OnTowerUpgraded;
    public static event UnityAction<IShootable, GameObject> OnTowerShooting;

    public static void TowerBuilt(IBuildable tower, Vector3 position)
    {
        OnTowerBuilt?.Invoke(tower, position);
    }

    public static void TowerUpgraded(IUpgradeable tower)
    {
        OnTowerUpgraded?.Invoke(tower);
    }

    public static void TowerFire(IShootable tower, GameObject enemyKill)
    {
        OnTowerShooting?.Invoke(tower, enemyKill);
    }
    #endregion
    #region WaveEvent
    public static event UnityAction<S_Enemy, float> OnWaveStart;

    public static void StartNewWave(S_Enemy enemy, float quantity)
    {
        OnWaveStart?.Invoke(enemy, quantity);
    }
    #endregion
    public static event UnityAction<int> OnModifieBaseLife;
    public static void ChangeBaseValue(int value)
    {
        OnModifieBaseLife?.Invoke(value);
    }
}
