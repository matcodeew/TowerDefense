using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    #region struct
    [System.Serializable]
    public struct RessourceInfoPanel
    {
        public Image Image;
        public TextMeshProUGUI text;
    }
    #endregion
    RessourceManager instance;
    public static UiManager Instance;

    [Header("Info Text To Update")]
    [SerializeField] private RessourceInfoPanel gold;
    [SerializeField] private RessourceInfoPanel wave;
    [SerializeField] private RessourceInfoPanel life;

    [Header("InfoPanel")]
    [SerializeField] private GameObject towerInfoPanel;
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private TextMeshProUGUI Damage;
    [SerializeField] private TextMeshProUGUI FireRate;
    [SerializeField] private TextMeshProUGUI Range;
    [SerializeField] private TextMeshProUGUI UpgradeCount;

    public bool IsActive;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        instance = RessourceManager.Instance;

        UpdateGold();
        UpdateWave();
        UpdateLife(0);
    }
    private void OnEnable()
    {
        EventsManager.OnEnemyDie += UpdateGold;
        EventsManager.OnEnemyReachEnd += UpdateLife;
        EventsManager.OnTowerBuild += UpdateOneTower;
        EventsManager.OnTowerDestroy += UpdateOneTower;
        EventsManager.OnWaveStart += Event_UpdateWave;
    }

    private void UpdateOneTower(Tower tower)
    {
        UpdateGold();
    }
    private void Event_UpdateWave(S_Enemy enemy, float quantity)
    {
        UpdateWave();
    }


    private void UpdateGold()
    {
        gold.text.text = instance.currentGold.ToString();
    }
    private void UpdateWave()
    {
        wave.text.text = $" {instance.CurrentWave}/{instance.MaxWave}";
    }
    private void UpdateLife(int value)
    {
        life.text.text = instance.BaseLife.ToString();
    }
    public void ActivateTowerInfoPanel(Tower tower)
    {
        towerInfoPanel.SetActive(IsActive);
        towerName.text = tower.TowerData.Type.ToString();

        Damage.text = $"{tower.stat.Damage.ToString()} dmg";
        FireRate.text = $"{tower.stat.FireRate.ToString("0.00")} /sec";
        Range.text = $"{tower.stat.FireRange.ToString()}";
        UpgradeCount.text = $"{tower.stat.GoldsCost.ToString()} (golds)";
    }
}   