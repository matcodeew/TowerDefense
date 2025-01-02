using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    [Header("Info Text To Update")]
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI wave;
    [SerializeField] private TextMeshProUGUI life;

    [Header("InfoPanel")]
    [SerializeField] private GameObject towerInfoPanel;
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private TextMeshProUGUI damage;
    [SerializeField] private TextMeshProUGUI fireRate;
    [SerializeField] private TextMeshProUGUI range;

    [Header("Wave Indication")]
    [SerializeField] private GameObject waveIndication;
    [SerializeField] public Image ProgressBar;
    
    public bool TowerInfoPanelIsActive;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
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
    }
    public void UpdateWave()
    {
        wave.text = $"Waves {RessourceManager.CurrentWave}/{RessourceManager.MaxWave}";
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
        }
    }
    public void ShowTowerInfoPanel()
    {
        if (towerInfoPanel != null)
        {
            towerInfoPanel.SetActive(TowerInfoPanelIsActive);
        }
    }
}