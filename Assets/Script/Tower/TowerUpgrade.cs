using UnityEngine;

public class TowerUpgrade : MonoBehaviour
{
    [Header("Ui")]
    [SerializeField] private GameObject UpgradePanel;
    private bool isActive;

    private void OnMouseDown()
    {
        print("ok");
        isActive = !isActive;
        UpgradePanel.SetActive(isActive); // !!!! layer ignore raycast => care drag n drop 
    }
}
