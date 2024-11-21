using UnityEngine;
using UnityEngine.Events;

public static class EventsManager
{
    #region Tower Event
    public static event UnityAction<IBuildable, Vector3> OnTowerBuilt;
    public static event UnityAction<IUpgradeable> OnTowerUpgraded;
    public static event UnityAction<IShootable> OnTowerShooting;

    public static void TowerBuilt(IBuildable tower, Vector3 position)
    {
        OnTowerBuilt?.Invoke(tower, position);
    }

    public static void TowerUpgraded(IUpgradeable tower)
    {
        OnTowerUpgraded?.Invoke(tower);
    }

    public static void TowerFire(IShootable tower)
    {
        OnTowerShooting?.Invoke(tower);
    }
    #endregion
}
