using UnityEngine;

public class UiAnimation : MonoBehaviour
{
    [SerializeField] private GameObject towerBuilderPanel;
    [SerializeField] private GameObject showPanelButton;
    [SerializeField] private Animator ShowPanelAnim;
    private bool showPanel = false;
    public void ShowPanel()
    {
        showPanel = !showPanel;
        ShowPanelAnim.SetBool("FirstState", showPanel);
    }
}
