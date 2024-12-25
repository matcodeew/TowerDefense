using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerBuilderManager : MonoBehaviour
{
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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void StartingDragTower(S_Tower towerData) //button clicked
    {
        DragTower = true;
        towerToBuild = towerData;
        if (towerToBuild.Prefab is null) { Debug.LogError($"Prefab on scriptable {towerToBuild} is null"); return; }
        tower = towerToBuild.Prefab.GetComponent<Tower>();
    }

    private void BuildingTower(Vector3 position)
    {
        DragTower = false;
        AllTowerPosedOnMap.Add(tower);
        tower.BuildTower(towerToBuild, position);
    }

    private void Update()
    {
        if (DragTower == false) { return; }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundAccepted))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsPointerOverUI())
                {
                    return;
                }
                Tile tile = hit.collider.gameObject.GetComponent<Tile>();
                if (tile != null && !tile.IsOccupied)
                {
                    //CancelPreview();
                    BuildingTower(hit.collider.transform.position);
                    TilesOccupied.Add(tile);
                    tile.IsOccupied = true;
                    tile.gameObject.layer = 0;
                    // UiAnimation.Instance.ShowPanel();
                }
                else
                {
                    print("Case déjà occupée !");
                }
            }
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }














    //=================
    //Elder Class======
    //=================




    //[System.Serializable]
    //private struct ButtonTower
    //{
    //    public TextMeshProUGUI text;
    //    public S_Tower towerType;
    //}
    //public static TowerBuilder Instance;
    //public S_Tower TowerData;
    //private Tower tower;
    //[HideInInspector] public bool CanDestroyTower = false;
    //[HideInInspector] public bool CanUpgradeTower = false;
    //[HideInInspector] public List<Tile> TilesOccupied = new();
    //[HideInInspector] public List<Tower> AllTowerPosedOnMap = new();


    //public bool DragTower;
    //private GameObject preview;
    //private Quaternion previewRotation;
    //[SerializeField] private LayerMask DefaultLayer;
    //[SerializeField] public LayerMask TileGround;

    //[Space]
    //[Header("UI")]
    //[SerializeField] public Color LockColor;
    //[SerializeField] public Color SelectedColor;
    //[SerializeField] private List<ButtonTower> allButtonTower = new();
    //[SerializeField] private List<GameObject> builderButtonTower = new();
    //[SerializeField] private List<S_Tower> towerToInstantiate = new();
    //[SerializeField] private GameObject CancelBuildingButton;
    //[SerializeField] private GameObject DestroyTowerButton;
    //[SerializeField] private GameObject UpgradeTowerButton;


    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //    }
    //    foreach (var towerUI in allButtonTower)
    //    {
    //        towerUI.text.text = towerUI.towerType.Type.ToString();
    //    }
    //}
    //public void BuildTower(S_Tower data, Vector3 position)
    //{
    //    CancelBuildingButton.SetActive(false);
    //    GameObject newTower = Instantiate(TowerData.Prefab, position, previewRotation);
    //    IBuildable buildableTower = newTower.GetComponent<IBuildable>();

    //    AllTowerPosedOnMap.Add(buildableTower as Tower);

    //    if (buildableTower != null)
    //    {
    //        newTower.GetComponent<Tower>().isPosed = true;
    //        buildableTower.Build(data, position);
    //        newTower.layer = DefaultLayer;
    //    }
    //    ChangeBuilderButtonColor();
    //}
    //#region Try To Pose Tower
    //private void Update()
    //{
    //    if (DragTower)
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, TileGround))
    //        {
    //            preview.transform.position = hit.point;
    //            tower.newRange.transform.position = hit.point;

    //            // Vérifie si un clic est effectué
    //            if (Input.GetMouseButtonDown(0))
    //            {
    //                // Ignore le clic si la souris est au-dessus d'un élément UI
    //                if (IsPointerOverUI())
    //                {
    //                    return;
    //                }

    //                // Vérifie si le clic est sur une case de type "TowerTile"
    //                if (hit.collider.CompareTag("TowerTile"))
    //                {
    //                    Tile tile = hit.collider.gameObject.GetComponent<Tile>();
    //                    if (tile != null && !tile.IsOccupied)
    //                    {
    //                        CancelPreview();
    //                        BuildTower(TowerData, hit.collider.transform.position + TowerData.PosOnMap);
    //                        TilesOccupied.Add(tile);
    //                        tile.IsOccupied = true;
    //                        tile.gameObject.layer = DefaultLayer;
    //                        UiAnimation.Instance.ShowPanel();
    //                    }
    //                    else
    //                    {
    //                        print("Case déjà occupée !");
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    //private bool IsPointerOverUI()
    //{
    //    return EventSystem.current.IsPointerOverGameObject();
    //}
    //private void MakePreview()
    //{
    //    if (preview == null)
    //    {
    //        preview = Instantiate(TowerData.PreviewPrefab);
    //        preview.GetComponent<BoxCollider>().enabled = false;
    //        tower.rangeCreated = true;
    //        tower.ShowRange();
    //        DragTower = true;
    //    }
    //}
    //public void PosTower(S_Tower _tower)
    //{
    //    TowerData = _tower;
    //    this.tower = TowerData.Prefab.GetComponent<Tower>();
    //    if (RessourceManager.Instance.HaveRessource(this.tower))
    //    {
    //        tower.InitializeTower(_tower);
    //        MakePreview();
    //    }
    //    else
    //    {
    //        print($"{RessourceManager.Instance.currentGold} is less than {this.TowerData.GoldsCost}");
    //    }
    //    ChangeBuilderButtonColor();
    //}
    //public void CancelPreview()
    //{
    //    if (preview != null)
    //    {
    //        tower.DestroyRange();
    //        previewRotation = preview.transform.rotation;
    //        Destroy(preview);
    //        preview = null;
    //        DragTower = false;
    //        MapManager.Instance.ResetHeightTile();
    //    }
    //}
    //#endregion

    //public void ResetAction()
    //{
    //    UiManager.Instance.TowerInfoPanelIsActive = false;
    //    CanUpgradeTower = false;
    //    UpgradeTowerButton.GetComponent<Image>().color = Color.white;
    //    CanDestroyTower = false;
    //    DestroyTowerButton.GetComponent<Image>().color = Color.white;

    //    ChangeBuilderButtonColor();
    //}
    //public void CanDestroy()
    //{
    //    CanUpgradeTower = false;
    //    UpgradeTowerButton.GetComponent<Image>().color = Color.white;

    //    CanDestroyTower = !CanDestroyTower;
    //    DestroyTowerButton.GetComponent<Image>().color = (CanDestroyTower ^ CanUpgradeTower) ? SelectedColor : Color.white;

    //    ChangeBuilderButtonColor();
    //}
    //public void CanUpgrade()
    //{
    //    CanDestroyTower = false;
    //    DestroyTowerButton.GetComponent<Image>().color = Color.white;

    //    CanUpgradeTower = !CanUpgradeTower;
    //    UpgradeTowerButton.GetComponent<Image>().color = (CanUpgradeTower ^ CanDestroyTower) ? SelectedColor : Color.white;

    //    ChangeBuilderButtonColor();
    //    UiManager.Instance.TowerInfoPanelIsActive = true;
    //}

    //public void ChangeBuilderButtonColor()
    //{
    //    for (int i = 0; i < towerToInstantiate.Count; i++)
    //    {
    //        if (RessourceManager.currentGold < towerToInstantiate[i].GoldsCost)
    //        {
    //            builderButtonTower[i].GetComponent<Image>().color = LockColor;
    //        }
    //        else
    //        {
    //            builderButtonTower[i].GetComponent<Image>().color = Color.white;
    //        }
    //    }
    //}
}