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
    [SerializeField] private GameObject previewTowerRange;
    private GameObject previewRangeTower;
    private Quaternion previewRotation;

    [HideInInspector] public bool CanDestroyTower;
    [HideInInspector] public bool CanUpgradeTower;

    [Header("UI")]
    [SerializeField] public List<TowerButton> AllButtonTower = new();
    [SerializeField] public Color LockColor;
    [SerializeField] public Color SelectedColor;
    [SerializeField] private GameObject CancelBuildingButton;
    [SerializeField] private Image DestroyTowerButton;
    [SerializeField] private Image UpgradeTowerButton;

    [Header("Rotate Tower")]
    private float _rotateTimer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void StartingDragTower(S_Tower towerData) //button clicked
    {
        if (!CanBuild(towerData)) return;
        if (previewTower is not null) CancelPreview();

        ResetUpgradeAndDestroy();
        DragTower = true;
        towerToBuild = towerData;
        if (towerToBuild.Prefab is null) { Debug.LogError($"Prefab on scriptable {towerToBuild} is null"); return; }
        tower = towerToBuild.Prefab.GetComponent<Tower>();
        MakePreview(towerData);
    }

    private bool CanBuild(S_Tower data) => RessourceManager.CurrentGold >= data.GoldsCost;

    private void BuildingTower(Transform position, Tile buildOnTile)
    {
        CancelPreview();
        AllTowerPosedOnMap.Add(tower);
        position.rotation = previewRotation;
        GameObject newtower = tower.BuildTower(towerToBuild, position, towerParent, buildOnTile);
        newtower.layer = 0;
        previewRangeTower.SetActive(false);
        previewRotation = Quaternion.Euler(0, 0, 0);
        yRotate = 0;
    }

    private void Update()
    {
        if (DragTower == false) { return; }

        if (Input.GetKeyUp(KeyCode.R) && previewTower != null)
        {
            yRotate += 90;
            previewTower.transform.rotation = Quaternion.Euler(0, yRotate, 0);
            previewRotation = previewTower.transform.rotation;
            print(previewRotation);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        print(scroll);
        _rotateTimer += Time.deltaTime;
        if (scroll != 0 && previewTower != null && _rotateTimer > 0.15)
        {
            if (scroll > 0) // Molette vers le haut
            {
                yRotate += 90;
            }
            else if (scroll < 0) // Molette vers le bas
            {
                yRotate -= 90;
            }
            _rotateTimer = 0;
            previewTower.transform.rotation = Quaternion.Euler(0, yRotate, 0);
            previewRotation = previewTower.transform.rotation;
            print(previewRotation);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            UpdatePreviewPosition(hit);
            if (Input.GetMouseButtonDown(1))
            {
                CancelPreview();
                return;
            }
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
                    BuildingTower(hit.collider.transform, tile);
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
        else if (Input.GetMouseButtonDown(0))
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
        previewRangeTower.transform.position = hit.point;
    }
    private void MakePreview(S_Tower towerData)
    {
        previewTower = Instantiate(towerData.PreviewPrefab, previewParent);
        previewRangeTower = Instantiate(previewTowerRange, previewTower.transform);
        previewRangeTower.transform.position = new Vector3(previewTower.transform.position.x, 0.5f, previewTower.transform.position.z);
        previewRangeTower.transform.localScale = new Vector3(towerToBuild.FireRange * 2, 0.1f, towerToBuild.FireRange * 2);
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
    }
    public void CanUpgrade()
    {
        if (TowerUpgrade.Instance.towerToUpgrade is not null) TowerUpgrade.Instance.towerToUpgrade.DestroyRange();
        TowerUpgrade.Instance.upgradePanel.SetActive(false);
        CanDestroyTower = false;
        DestroyTowerButton.color = Color.white;

        CanUpgradeTower = !CanUpgradeTower;
        UpgradeTowerButton.color = (CanUpgradeTower ^ CanDestroyTower) ? SelectedColor : Color.white;

        UiManager.Instance.TowerInfoPanelIsActive = true;
    }

    public void ResetUpgradeAndDestroy()
    {
        CanDestroyTower = false;
        DestroyTowerButton.color = Color.white;
        TowerUpgrade.Instance.upgradePanel.SetActive(false);
        if (TowerUpgrade.Instance.towerToUpgrade is not null) TowerUpgrade.Instance.towerToUpgrade.DestroyRange();

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