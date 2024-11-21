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

    [Header("Info Text To Update")]
    [SerializeField] private InfoPanel gold;
    [SerializeField] private InfoPanel wave;
    [SerializeField] private InfoPanel life;

    [Header("current stats")]
    [HideInInspector] public int CurrentWave;
    [HideInInspector] public int CurrentLife;

    private void Start()
    {
        UpdateGold();
        UpdateWave();
        UpdateLife();
    }
    private void OnEnable()
    {
        EventsManager.OnTowerBuilt += OnTowerBuild;
    }
    private void OnDisable()
    {
        EventsManager.OnTowerBuilt -= OnTowerBuild;
    }
    #region Event Update UI
    private void OnTowerBuild(IBuildable tower, Vector3 position)
    {
        UpdateGold();
    }
    #endregion
    #region Update Info Panel
    private void UpdateGold()
    {
        gold.text.text = RessourceManager.Instance.currentGold.ToString();
    }
    private void UpdateWave()
    {
        wave.text.text = CurrentWave.ToString();
    }
    private void UpdateLife()
    {
        life.text.text = CurrentLife.ToString();
    }
    #endregion
}