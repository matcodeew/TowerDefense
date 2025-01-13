using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tuto : MonoBehaviour
{
    public static Tuto Instance;
    [Header("Map UI")]
    [SerializeField] private GameObject waveButton;
    [SerializeField] private List<GameObject> towerButton;
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject sellButton;
    [SerializeField] private GameObject blockUiPanel;

    [Header("Tuto UI")]
    [SerializeField] private GameObject tutoPanel;
    [SerializeField] private TextMeshProUGUI tutoText;

    [Header("Tuto state")]
    private bool tutoIsFinished = false;
    public bool playerShouldExecuteAction;
    [SerializeField] private int currentSteps = -1;
    [TextArea]
    [SerializeField] private List<string> textToShow = new();

    [Header("Type writer effect")]
    [SerializeField] private float _delay;
    [SerializeField] private string _fullText;
    [SerializeField] private string _currentText = "";

    [Header("timer")]
    private float timer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        blockUiPanel.SetActive(true);
        waveButton.SetActive(false);
        foreach (var button in towerButton)
        {
            button.SetActive(false);
        }
        upgradeButton.SetActive(false);
        sellButton.SetActive(false);
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (Input.anyKeyDown && timer >= 0.3 && playerShouldExecuteAction == false && tutoIsFinished == false)
        {
            timer = 0;
            ShowAndUpdateTutoText();
        }
    }
    private void CheckTutoStep()
    {
        switch (currentSteps)
        {
            case 0:
                foreach (var button in towerButton)
                {
                    button.SetActive(true);
                }
                break;

            case 1:
                break;

            case 2:
                blockUiPanel.SetActive(false);
                playerShouldExecuteAction = true;
                tutoPanel.SetActive(false);
                break;

            case 3:
                tutoPanel.SetActive(true);
                foreach (var button in towerButton)
                {
                    button.SetActive(false);
                }
                upgradeButton.SetActive(true);
                break;

            case 4:
                playerShouldExecuteAction = true;
                tutoPanel.SetActive(false);
                break;

            case 5:
                tutoPanel.SetActive(true);
                upgradeButton.SetActive(false);
                upgradePanel.SetActive(false);
                TowerBuilderManager.Instance.CanUpgradeTower = false;

                sellButton.SetActive(true);
                break;

            case 6:
                playerShouldExecuteAction = true;
                tutoPanel.SetActive(false);
                break;

            case 7:
                waveButton.SetActive(true);
                TowerBuilderManager.Instance.CanDestroyTower = false;
                break;

            case 8:
                foreach (var button in towerButton)
                {
                    button.SetActive(true);
                }
                upgradeButton.SetActive(true);
                sellButton.SetActive(true);
                waveButton.SetActive(true);

                tutoPanel.SetActive(false);
                tutoIsFinished = true;
                break;

            default:
                break;
        }
    }

    #region ShowAndUpdateTutoText
    [ContextMenu("ShowAndUpdateTutoText")]
    public void ShowAndUpdateTutoText()
    {
       if (_currentText.Length != _fullText.Length) { return; }

        currentSteps++;
        if (currentSteps >= textToShow.Count) { tutoIsFinished = true; return; }

        tutoPanel.SetActive(true);
        UpdatePanelText();
        CheckTutoStep();
    }

    private void UpdatePanelText()
    {
        _fullText = textToShow[currentSteps];
        _currentText = string.Empty;
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        for (int i = 0; 0 < _fullText.Length; i++)
        {
            if (!tutoPanel.activeSelf) { ResetText(); break; }
            if (_currentText.Length == _fullText.Length) { break; }

            _currentText = _fullText.Substring(0, i);
            tutoText.text = _currentText;
            yield return new WaitForSeconds(_delay);
        }
    }

    private void ResetText()
    {
        _fullText = string.Empty;
        _currentText = string.Empty;
        tutoText.text = string.Empty;
    }
    #endregion
}
