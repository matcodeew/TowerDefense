using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerBuilderManager : MonoBehaviour
{
    [System.Serializable]
    public struct TowerButton
    {
        public Image Button;
        public S_Tower TowerOnButton;
    }
    public static TowerBuilderManager Instance;

    [Header("Maps")]
    [HideInInspector] public List<Tile> TilesOccupied = new();
    [HideInInspector] public List<Tower> AllTowerPosedOnMap = new();

    [Header("Raycast settings")]
    [SerializeField] private LayerMask groundAccepted;

    [Header("DragTower")]
    public bool DragTower;
    private S_Tower towerToBuild;
    private Tower tower;
    [SerializeField] private Transform towerParent;
    [SerializeField] private Transform previewParent;
    
    [Header("Preview Tower")]
    private float yRotate;
    private GameObject previewTower;
    private Quaternion previewRotation;

    [HideInInspector] public bool CanDestroyTower ;
    [HideInInspector] public bool CanUpgradeTower ;
    
    [Header("UI")]
    [SerializeField] private List<TowerButton> AllButtonTower = new();
    [SerializeField] public Color LockColor;
    [SerializeField] public Color SelectedColor;
    [SerializeField] private GameObject CancelBuildingButton;
    [SerializeField] private Image DestroyTowerButton;
    [SerializeField] private Image UpgradeTowerButton;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void StartingDragTower(S_Tower towerData) //button clicked
    {
        ResetUpgradeAndDestroy();
        DragTower = true;
        towerToBuild = towerData;
        if (towerToBuild.Prefab is null) { Debug.LogError($"Prefab on scriptable {towerToBuild} is null"); return; }
        tower = towerToBuild.Prefab.GetComponent<Tower>();
        MakePreview(towerData);
    }

    private void BuildingTower(Vector3 position, Tile buildOnTile)
    {
        CancelPreview();
        AllTowerPosedOnMap.Add(tower);
        GameObject newtower = tower.BuildTower(towerToBuild, position, towerParent, buildOnTile);
        newtower.transform.rotation = previewRotation;
        newtower.layer = 0;
    }

    private void Update()
    {
        if (DragTower == false) { return; }

        if (Input.GetKeyUp(KeyCode.R) && previewTower is not null)
        { 
            yRotate += 90;
            previewTower.transform.rotation = Quaternion.Euler(0, yRotate, 0);
            previewRotation = previewTower.transform.rotation;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            UpdatePreviewPosition(hit);
            if (Input.GetMouseButtonDown(0))
            {
                if (IsPointerOverUI())
                {
                    CancelPreview();
                    return;
                }
                Tile tile = hit.collider.gameObject.GetComponent<Tile>();
                if (tile is not null && !tile.IsOccupied)
                {
                    BuildingTower(hit.collider.transform.position, tile);
                    TilesOccupied.Add(tile);
                    tile.IsOccupied = true;
                    tile.gameObject.layer = 0;
                    // UiAnimation.Instance.ShowPanel();
                }
                else
                {
                    CancelPreview();
                }
            }
        }
        else if(Input.GetMouseButtonDown(0))
        { 
            CancelPreview();
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void UpdatePreviewPosition(RaycastHit hit)
    {
        previewTower.transform.position = hit.point;
    }
    private void MakePreview(S_Tower towerData)
    {
        previewTower = Instantiate(towerData.PreviewPrefab, previewParent);
    }

    private void CancelPreview()
    {
        Destroy(previewTower);
        previewTower = null;
        DragTower = false;
    }


    public void CanDestroy()
    {
        CanUpgradeTower = false;
        UpgradeTowerButton.color = Color.white;

        CanDestroyTower = !CanDestroyTower; 
        DestroyTowerButton.color = (CanDestroyTower ^ CanUpgradeTower) ? SelectedColor : Color.white;

        ChangeBuilderButtonColor();
    }
    public void CanUpgrade()
    {
        TowerUpgrade.Instance.upgradePanel.SetActive(false);
        CanDestroyTower = false;
        DestroyTowerButton.color = Color.white;

        CanUpgradeTower = !CanUpgradeTower;
        UpgradeTowerButton.color = (CanUpgradeTower ^ CanDestroyTower) ? SelectedColor : Color.white;

        ChangeBuilderButtonColor();
        UiManager.Instance.TowerInfoPanelIsActive = true;
    }

    public void ResetUpgradeAndDestroy()
    {
        CanDestroyTower = false;
        DestroyTowerButton.color = Color.white;
        TowerUpgrade.Instance.upgradePanel.SetActive(false);
        
        CanUpgradeTower = false;
        UpgradeTowerButton.color = Color.white;
    }
    public void ChangeBuilderButtonColor()
    {
        for (int i = 0; i < AllButtonTower.Count; i++)
        {
            if (RessourceManager.CurrentGold < AllButtonTower[i].TowerOnButton.GoldsCost)
            {
                AllButtonTower[i].Button.color = LockColor;
            }
            else
            {
                AllButtonTower[i].Button.color = Color.white;
            }
        }
    } 
}