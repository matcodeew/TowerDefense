using UnityEngine;

public class UiAnimation : MonoBehaviour
{
    public static UiAnimation Instance;
    [SerializeField] private GameObject towerBuilderPanel;
    [SerializeField] private GameObject showPanelButton;
    [SerializeField] private Animator ShowPanelAnim;
    [SerializeField] private Animator WaveIndication;
    private bool showPanel = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    public void StopWaveButtonAnim()
    {
        WaveIndication.SetBool("isActive", true);
    }
    public void StartWaveButtonAnim()
    {
        WaveIndication.SetBool("isActive", false);
    }
    public void ShowPanel()
    {
        showPanel = !showPanel;
        ShowPanelAnim.SetBool("FirstState", showPanel);
    }
}
