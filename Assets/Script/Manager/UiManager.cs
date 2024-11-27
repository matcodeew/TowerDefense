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

    [System.Serializable]
    public struct TowerStatInfo
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
    [SerializeField] private TowerStatInfo Damage;
    [SerializeField] private TowerStatInfo FireRate;
    [SerializeField] private TowerStatInfo Range;
    [SerializeField] private TowerStatInfo UpgradeCount;

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
        UpdateLife();
    }
    private void OnEnable()
    {
        EventsManager.OnTowerBuilt += OnTowerBuild;
        EventsManager.OnModifieBaseLife += OnBaseLifeChanged;
        EventsManager.OnTowerDestroy += OnTowerDestroy;
        EventsManager.OnEnemieDie += OnEnemyDie;
        EventsManager.OnPanelOpen += BlockSlot;
    }
    private void OnDisable()
    {
        EventsManager.OnTowerBuilt -= OnTowerBuild;
        EventsManager.OnEnemieDie -= OnEnemyDie;
        EventsManager.OnModifieBaseLife -= OnBaseLifeChanged;
        EventsManager.OnTowerDestroy -= OnTowerDestroy;

        EventsManager.OnPanelOpen -= BlockSlot;
    }
    private void OnTowerBuild(IBuildable tower, Vector3 position)
    {
        UpdateGold();
    }
    private void OnEnemyDie()
    {
        UpdateGold();
    }
    private void OnBaseLifeChanged(int value)
    {
        UpdateLife();
    }
    private void OnTowerDestroy(Tower tower)
    {
        UpdateGold();
    }
    private void UpdateGold()
    {
        gold.text.text = instance.currentGold.ToString();
    }
    private void UpdateWave()
    {
        wave.text.text = $" {instance.CurrentWave}/{instance.MaxWave}";
    }
    private void UpdateLife()
    {
        life.text.text = instance.BaseLife.ToString();
    }

    private void BlockSlot(Tower tower, GameObject slot)
    {
        if (tower.stat.GoldsCost > RessourceManager.Instance.currentGold)
        {
            slot.GetComponent<Image>().color = Color.cyan;
            slot.GetComponent<Button>().enabled = false;
        }
        else
        {
            slot.GetComponent<Image>().color = Color.white;
            slot.GetComponent<Button>().enabled = true;
        }
    }

    public void ActivateTowerInfoPanel(Tower tower)
    {
        towerInfoPanel.SetActive(IsActive);
        towerName.text = tower.TowerData.Type.ToString();

        Damage.text.text = $"{tower.stat.Damage.ToString()} damage";
        //Damage.Image.sprite = ;
        FireRate.text.text = $"{tower.stat.FireRate.ToString("0.00")} /sec";
        //FireRate.Image.sprite = ;
        Range.text.text = $"{tower.stat.FireRange.ToString()}";
        //Range.Image.sprite = ;
        UpgradeCount.text.text = $"{tower.stat.GoldsCost.ToString()} (golds)";
        //UpgradeCount.Image.sprite = ;
    }
}   