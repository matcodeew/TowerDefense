using UnityEngine;
using UnityEngine.EventSystems;

public class TowerButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private S_Tower associatedTower;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (associatedTower != null)
        {
            UiManager.Instance.TowerInfoPanelIsActive = true;
            UiManager.Instance.DescriptionInfoPanel(associatedTower); 
            UiManager.Instance.ShowTowerInfoPanel();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UiManager.Instance.TowerInfoPanelIsActive = false; 
        UiManager.Instance.ShowTowerInfoPanel();
    }

    public void SetAssociatedTower(S_Tower tower)
    {
        associatedTower = tower;
    }
}