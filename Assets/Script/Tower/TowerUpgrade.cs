using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrade : MonoBehaviour
{
    public static TowerUpgrade Instance;

    [SerializeField] private Tower towerToUpgrade;
    [SerializeField] private GameObject upgradePanel;
    private bool isActive;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void SelectTowerToUpgrade(Tower tower)
    {
        ShowPanel();
        towerToUpgrade = tower;
    }
    public void ShowPanel()
    {
        isActive = !isActive;
        upgradePanel.SetActive(isActive);
        towerToUpgrade = null;
    }

    public void UpgradeDamage()
    {
        if (CanUpgrade(towerToUpgrade.DmgUpgradecount))
        {
            UpdateGoldValue();
            towerToUpgrade.stat.Damage += towerToUpgrade.TowerData.UpgradeDamage;
            towerToUpgrade.DmgUpgradecount++;
        }
    }
    public void UpgradeFireSpeed()
    {
        if (CanUpgrade(towerToUpgrade.FireRateUpgradecount))
        {
            UpdateGoldValue();
            towerToUpgrade.stat.FireRate -= towerToUpgrade.TowerData.UpgradeFireRate;
            towerToUpgrade.FireRateUpgradecount++;
        }
    }
    public void UpgradeRange()
    {
        if (CanUpgrade(towerToUpgrade.RangeUpgradecount))
        {
            UpdateGoldValue();
            towerToUpgrade.stat.FireRange += towerToUpgrade.TowerData.UpgradeFireRange;
            towerToUpgrade.RangeUpgradecount++;
        }
    }

    private bool CanUpgrade(int UpgradeCount) => UpgradeCount < towerToUpgrade.TowerData.MaxUpgrade && RessourceManager.Instance.HaveRessource(towerToUpgrade);
    private void UpdateGoldValue()
    {
        EventsManager.TowerBuild(towerToUpgrade);
        towerToUpgrade.stat.GoldsCost += (towerToUpgrade.TowerData.GoldsCost / 2) + 30;
    }
}
