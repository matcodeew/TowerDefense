using UnityEngine;
using UnityEngine.Events;

public static class EventsManager
{
    #region Tower Event
    public static event UnityAction<IBuildable, Vector3> OnTowerBuilt;
    public static event UnityAction<Tower> OnTowerUpgraded;
    public static event UnityAction<IShootable, GameObject> OnTowerShooting;
    public static event UnityAction<Tower> OnTowerDestroy;

    public static event UnityAction<int> OnModifieBaseLife;
    public static event UnityAction OnEnemieDie;

    public static event UnityAction<S_Enemy, float> OnWaveStart;

    public static event UnityAction<Tower, GameObject> OnPanelOpen;

    public static void OpenPanel(Tower tower, GameObject slot)
    {
        OnPanelOpen?.Invoke(tower, slot);
    }

    public static void TowerBuilt(IBuildable tower, Vector3 position)
    {
        OnTowerBuilt?.Invoke(tower, position);
    }

    public static void TowerUpgraded(Tower tower)
    {
        OnTowerUpgraded?.Invoke(tower);
    }

    public static void TowerFire(IShootable tower, GameObject enemyKill)
    {
        OnTowerShooting?.Invoke(tower, enemyKill);
    }
    public static void TowerDestroy(Tower tower)
    {
        OnTowerDestroy?.Invoke(tower);
    }
    #endregion

    #region WaveEvent
    public static void StartNewWave(S_Enemy enemy, float quantity)
    {
        OnWaveStart?.Invoke(enemy, quantity);
    }
    #endregion


    public static void ChangeBaseValue(int value)
    {
        OnModifieBaseLife?.Invoke(value);
    }
    public static void EnemieDie()
    {
        OnEnemieDie?.Invoke();
    }
}
