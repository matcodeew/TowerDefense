using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    [Header("Info Text To Update")]
    [SerializeField]
    private TextMeshProUGUI gold;

    [SerializeField] private TextMeshProUGUI wave;
    [SerializeField] private TextMeshProUGUI life;

    [Header("InfoPanel")][SerializeField] private GameObject towerInfoPanel;
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private TextMeshProUGUI damage;
    [SerializeField] private TextMeshProUGUI fireRate;
    [SerializeField] private TextMeshProUGUI range;
    [SerializeField] private TextMeshProUGUI goldCount;

    [Header("Wave Indication")]
    [SerializeField]
    public GameObject waveIndication;

    [SerializeField] public Image ProgressBar;

    [Header("Tower Range")]
    [SerializeField]
    public GameObject towerRange;

    [Header("UI description")]
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private TextMeshProUGUI descriptionText;

    public bool TowerInfoPanelIsActive;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        // for (int i = 0; i < uiNeededDescription.Count; i++)
        // {
        //     UIDescription.Add(uiNeededDescription[i], descriptionForUi[i]);
        // }
    }

    private void Start()
    {
        UpdateGold();
        UpdateWave();
        UpdateLife();
    }

    public void UpdateGold()
    {
        gold.text = RessourceManager.CurrentGold.ToString();
        TowerBuilderManager.Instance.ChangeBuilderButtonColor();
        TowerBuilderManager.Instance.ShowUpgradableTower();
    }

    public void UpdateWave()
    {
        wave.text = $"{RessourceManager.CurrentWave}/{RessourceManager.MaxWave}";
    }

    public void UpdateLife()
    {
        life.text = RessourceManager.BaseLife.ToString();
    }

    public void UpdateTowerInfoPanel(Tower tower)
    {
        if (tower != null)
        {
            towerName.text = tower.towerData.Type.ToString();
            damage.text = $"{tower.stat.Damage.ToString("0.00")} dmg";
            fireRate.text = $"{tower.stat.FireRate.ToString("0.00")} /sec";
            range.text = $"{tower.stat.FireRange.ToString("0.0")}";
            goldCount.text = $"{tower.buildingStat.GoldsCost.ToString()}";
        }
    }
    public void DescriptionInfoPanel(S_Tower tower)
    {
        if (tower != null)
        {
            towerName.text = tower.Type.ToString();
            damage.text = $"{tower.Damage.ToString("0.00")} dmg";
            fireRate.text = $"{tower.FireRate.ToString("0.00")} /sec";
            range.text = $"{tower.FireRange.ToString("0.0")}";
            goldCount.text = $"{tower.GoldsCost.ToString()}";
        }
    }

    public void ShowTowerInfoPanel()
    {
        if (towerInfoPanel != null)
        {
            towerInfoPanel.SetActive(TowerInfoPanelIsActive);
        }
    }
    public void ShowDescriptionPanel(string text, Vector3 position)
    {
        if (descriptionPanel != null && descriptionText != null)
        {
            descriptionText.text = text;
            descriptionPanel.SetActive(true);
            descriptionPanel.transform.position = position;
        }
    }

    public void HideDescriptionPanel()
    {
        if (descriptionPanel != null)
        {
            descriptionPanel.SetActive(false);
        }
    }
}