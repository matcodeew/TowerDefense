using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    #region struct
    [System.Serializable]
    public struct InfoPanel
    {
        public Image Image;
        public TextMeshProUGUI text;
    }
    #endregion
    RessourceManager instance;

   [Header("Info Text To Update")]
    [SerializeField] private InfoPanel gold;
    [SerializeField] private InfoPanel wave;
    [SerializeField] private InfoPanel life;

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
    }
    private void OnDisable()
    {
        EventsManager.OnTowerBuilt -= OnTowerBuild;
        EventsManager.OnEnemieDie -= OnEnemyDie;
        EventsManager.OnModifieBaseLife -= OnBaseLifeChanged;
        EventsManager.OnTowerDestroy -= OnTowerDestroy;
    }
    #region Event Update UI
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
    #endregion
    #region Update Info Panel
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
    #endregion
}