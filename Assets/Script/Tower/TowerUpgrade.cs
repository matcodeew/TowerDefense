using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgrade : MonoBehaviour
{
    public static TowerUpgrade Instance;
    [System.Serializable]
    public struct UpgradeChoice
    {
        public List<GameObject> UpgradeIndicator;
    }
    private int MaxUpgradePerType = 3; 
    public Tower towerToUpgrade;
    [SerializeField] public GameObject upgradePanel;
    [SerializeField] private List<UpgradeChoice> AllUpgradeChoice;
    [SerializeField] private TextMeshProUGUI towerCount;
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private List<GameObject> AllUpgradeButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void CheckIndicator()
    {
        ResetIndicators();

        if (towerToUpgrade != null)
        {
            for (int i = 0; i < towerToUpgrade.dmgUpgradecount; i++)
            {
                AllUpgradeChoice[0].UpgradeIndicator[i].GetComponent<Image>().color = Color.green;
            }
            for (int i = 0; i < towerToUpgrade.fireRateUpgradecount; i++)
            {
                AllUpgradeChoice[1].UpgradeIndicator[i].GetComponent<Image>().color = Color.green;
            }
            for (int i = 0; i < towerToUpgrade.rangeUpgradecount; i++)
            {
                AllUpgradeChoice[2].UpgradeIndicator[i].GetComponent<Image>().color = Color.green;
            }
        }
    }
    private void ResetIndicators()
    {
        foreach (var choice in AllUpgradeChoice)
        {
            foreach (var indicator in choice.UpgradeIndicator)
            {
                indicator.GetComponent<Image>().color = Color.white;
            }
        }
        ChangeUpgradeButtonColor();
    }
    private void UpdateUIChoiceIndicator(int upCount, int ButtonIndex)
    {
        for (int i = 0; i < AllUpgradeChoice[ButtonIndex].UpgradeIndicator.Count; i++)
        {
            AllUpgradeChoice[ButtonIndex].UpgradeIndicator[upCount].GetComponent<Image>().color = Color.green;
        }
       // UiManager.Instance.UpdateTowerInfoPanel(towerToUpgrade);
    }
    public void SelectTowerToUpgrade(Tower tower)
    {
        if (towerToUpgrade is not null) towerToUpgrade.DestroyRange();
        ShowPanel();
        towerToUpgrade = tower;
        towerToUpgrade.ShowRange();
        CheckIndicator();
        towerCount.text = $"{towerToUpgrade.buildingStat.GoldsCost.ToString()}";
        towerName.text = $"{towerToUpgrade.towerData.Type.ToString()}";
        ChangeUpgradeButtonColor();
    }
    public void ShowPanel()
    {
        upgradePanel.SetActive(true);
        towerToUpgrade = null;
    }

    public void UpgradeDamage()
    {
        if (CanUpgrade(towerToUpgrade.dmgUpgradecount))
        {
            UpdateGoldValue();
            towerToUpgrade.stat.Damage += towerToUpgrade.towerData.UpgradeDamage;
            UpdateUIChoiceIndicator(towerToUpgrade.dmgUpgradecount, 0);
            towerToUpgrade.dmgUpgradecount++;
        }
    }
    public void UpgradeFireSpeed()
    {
        if (CanUpgrade(towerToUpgrade.fireRateUpgradecount))
        {
            UpdateGoldValue();
            towerToUpgrade.stat.FireRate -= towerToUpgrade.towerData.UpgradeFireRate;
            UpdateUIChoiceIndicator(towerToUpgrade.fireRateUpgradecount, 1);
            towerToUpgrade.fireRateUpgradecount++;
        }
    }
    public void UpgradeRange()
    {
        if (CanUpgrade(towerToUpgrade.rangeUpgradecount))
        {
            UpdateGoldValue();
            towerToUpgrade.stat.FireRange += towerToUpgrade.towerData.UpgradeFireRange;
            UpdateUIChoiceIndicator(towerToUpgrade.rangeUpgradecount, 2);
            towerToUpgrade.rangeUpgradecount++;
        }
    }

    private bool CanUpgrade(int UpgradeCount) => UpgradeCount < MaxUpgradePerType && towerToUpgrade.buildingStat.GoldsCost <= RessourceManager.CurrentGold;
    private void UpdateGoldValue()
    {
        RessourceManager.LoseGold(towerToUpgrade.buildingStat.GoldsCost);
        int totalTowerUpgrade = towerToUpgrade.rangeUpgradecount + towerToUpgrade.dmgUpgradecount + towerToUpgrade.fireRateUpgradecount;
        towerToUpgrade.buildingStat.GoldsCost += (int) ((towerToUpgrade.towerData.GoldsCost / 2) * Mathf.Pow(1.1f, totalTowerUpgrade));
        towerCount.text = $"{towerToUpgrade.buildingStat.GoldsCost.ToString()}";
        ChangeUpgradeButtonColor();
    }

    private void ChangeUpgradeButtonColor()
    {
        foreach(GameObject upButton in AllUpgradeButton)
        {
            if(RessourceManager.CurrentGold <= towerToUpgrade.buildingStat.GoldsCost)
            {
                upButton.GetComponent<Image>().color = TowerBuilderManager.Instance.LockColor;
            }
            else
            {
                upButton.GetComponent<Image>().color = Color.white;
            }
        }
    }
}
