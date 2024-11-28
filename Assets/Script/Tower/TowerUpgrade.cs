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
    [SerializeField] private Tower towerToUpgrade;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject BuilderPanel;
    [SerializeField] private List<UpgradeChoice> AllUpgradeChoice;
    [SerializeField] private TextMeshProUGUI towerCount;
    [SerializeField] private List<GameObject> AllUpgradeButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void HideTowerInfoPanel()
    {
        towerToUpgrade.HideInfoPanel();
    }
    private void CheckIndicator()
    {
        ResetIndicators();

        if (towerToUpgrade != null)
        {
            // Vérifier les améliorations de dégâts
            for (int i = 0; i < towerToUpgrade.DmgUpgradecount; i++)
            {
                AllUpgradeChoice[0].UpgradeIndicator[i].GetComponent<Image>().color = Color.green;
            }

            // Vérifier les améliorations de cadence de tir
            for (int i = 0; i < towerToUpgrade.FireRateUpgradecount; i++)
            {
                AllUpgradeChoice[1].UpgradeIndicator[i].GetComponent<Image>().color = Color.green;
            }

            // Vérifier les améliorations de portée
            for (int i = 0; i < towerToUpgrade.RangeUpgradecount; i++)
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
        UiManager.Instance.UpdateTowerInfoPanel(towerToUpgrade);
    }
    public void SelectTowerToUpgrade(Tower tower)
    {
        ShowPanel();
        towerToUpgrade = tower;
        CheckIndicator();
        UiManager.Instance.ShowTowerInfoPanel();
        towerCount.text = $"{towerToUpgrade.stat.GoldsCost.ToString()}";
        ChangeUpgradeButtonColor();
    }
    public void ShowPanel()
    {
        upgradePanel.SetActive(true);
        BuilderPanel.SetActive(false);
        towerToUpgrade = null;
    }

    public void UpgradeDamage()
    {
        if (CanUpgrade(towerToUpgrade.DmgUpgradecount))
        {
            UpdateGoldValue();
            towerToUpgrade.stat.Damage += towerToUpgrade.TowerData.UpgradeDamage;
            UpdateUIChoiceIndicator(towerToUpgrade.DmgUpgradecount, 0);
            towerToUpgrade.DmgUpgradecount++;
        }
    }
    public void UpgradeFireSpeed()
    {
        if (CanUpgrade(towerToUpgrade.FireRateUpgradecount))
        {
            UpdateGoldValue();
            towerToUpgrade.stat.FireRate -= towerToUpgrade.TowerData.UpgradeFireRate;
            UpdateUIChoiceIndicator(towerToUpgrade.FireRateUpgradecount, 1);
            towerToUpgrade.FireRateUpgradecount++;
        }
    }
    public void UpgradeRange()
    {
        if (CanUpgrade(towerToUpgrade.RangeUpgradecount))
        {
            UpdateGoldValue();
            towerToUpgrade.stat.FireRange += towerToUpgrade.TowerData.UpgradeFireRange;
            UpdateUIChoiceIndicator(towerToUpgrade.RangeUpgradecount, 2);
            towerToUpgrade.RangeUpgradecount++;
        }
    }

    private bool CanUpgrade(int UpgradeCount) => UpgradeCount < MaxUpgradePerType && RessourceManager.Instance.HaveRessource(towerToUpgrade);
    private void UpdateGoldValue()
    {
        EventsManager.TowerBuild(towerToUpgrade);
        int totalTowerUpgrade = towerToUpgrade.RangeUpgradecount + towerToUpgrade.DmgUpgradecount + towerToUpgrade.FireRateUpgradecount;
        towerToUpgrade.stat.GoldsCost += towerToUpgrade.TowerData.GoldsCost * (int)Mathf.Pow(1.1f, totalTowerUpgrade);
        towerCount.text = $"{towerToUpgrade.stat.GoldsCost.ToString()}";
        ChangeUpgradeButtonColor();
    }

    private void ChangeUpgradeButtonColor()
    {
        foreach(GameObject upButton in AllUpgradeButton)
        {
            if(RessourceManager.Instance.currentGold < towerToUpgrade.stat.GoldsCost)
            {
                upButton.GetComponent<Image>().color = TowerBuilder.Instance.LockColor;
            }
            else
            {
                upButton.GetComponent<Image>().color = Color.white;
            }
        }
    }
}
