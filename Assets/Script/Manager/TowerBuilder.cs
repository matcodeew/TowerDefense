using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuilder : MonoBehaviour
{
    [System.Serializable]
    private struct ButtonTower
    {
       public TextMeshProUGUI text;
       public S_Tower towerType;
    }
    public static TowerBuilder Instance;
    public S_Tower tower;
    [HideInInspector] public bool CanDestroyTower = false;
    [HideInInspector] public bool CanUpgradeTower = false;
    [HideInInspector] public List<Tile> TilesOccupied = new();
    [HideInInspector] public List<Tower> AllTowerPosedOnMap = new();


    private bool UpdatePos;
    private GameObject preview;
    private Quaternion previewRotation;
    [SerializeField] private LayerMask DefaultLayer;

    [Space]
    [Header("UI")]
    [SerializeField] private List<ButtonTower> allButtonTower = new();
    [SerializeField] private Color LockColor;
    [SerializeField] private GameObject CancelBuildingButton;
    [SerializeField] private GameObject DestroyTowerButton;
    [SerializeField] private GameObject UpgradeTowerButton;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        foreach(var towerUI in allButtonTower)
        {
            towerUI.text.text = towerUI.towerType.Type.ToString();
        }
    }
    public void BuildTower(S_Tower data, Vector3 position)
    {
        CancelBuildingButton.SetActive(false);

        GameObject newTower = Instantiate(tower.Prefab, position, previewRotation);
        IBuildable buildableTower = newTower.GetComponent<IBuildable>();

        AllTowerPosedOnMap.Add(buildableTower as Tower);

        if (buildableTower != null)
        {
            newTower.GetComponent<Tower>().isPosed = true;
            buildableTower.Build(data, position);
            newTower.layer = DefaultLayer;
        }
    }
    #region Try To Pose Tower
    private void Update()
    {
        if (UpdatePos)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                preview.transform.position = hit.point + tower.PosOnMap;
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.collider.CompareTag("TowerTile"))
                    {
                        Tile tile = hit.collider.gameObject.GetComponent<Tile>();
                        if (tile != null && !tile.IsOccupied)
                        {
                            CancelPreview();
                            BuildTower(tower, hit.collider.transform.position + tower.PosOnMap);
                            TilesOccupied.Add(tile);
                            tile.IsOccupied = true;
                        }
                        else
                        {
                            print("bat on this tile ");
                        }
                    }
                    //else
                    //{
                    //    CancelPreview();
                    //}
                }
            }
        }
    }
    private void MakePreview()
    {
        if (preview == null)
        {
            preview = Instantiate(tower.PreviewPrefab);
        }
    }
    public void PosTower(S_Tower tower)
    {
        this.tower = tower;
        if (RessourceManager.Instance.HaveRessource(this.tower.Prefab.GetComponent<Tower>()))
        {
            this.tower.Prefab.GetComponent<Tower>().InitializeTower(tower);
            UpdatePos = true;
            MakePreview();
        }
        else
        {
            print($"{RessourceManager.Instance.currentGold} is less than {this.tower.GoldsCost}");
        }
    }
    public void CancelPreview()
    {
        if (preview != null)
        {
            previewRotation = preview.transform.rotation;
            Destroy(preview);
        }

        preview = null;
        UpdatePos = false;
    }
    #endregion

    public void ResetAction()
    {
        CanUpgradeTower = false;
        UpgradeTowerButton.GetComponent<Image>().color = Color.white;
        CanDestroyTower = false;
        DestroyTowerButton.GetComponent<Image>().color = Color.white;
    }
    public void CanDestroy()
    {
        CanUpgradeTower = false;
        UpgradeTowerButton.GetComponent<Image>().color = Color.white;

        CanDestroyTower = !CanDestroyTower;
        DestroyTowerButton.GetComponent<Image>().color = (CanDestroyTower ^ CanUpgradeTower) ? LockColor : Color.white;
    }
    public void CanUpgrade()
    {
        CanDestroyTower = false;
        DestroyTowerButton.GetComponent<Image>().color = Color.white;

        CanUpgradeTower = !CanUpgradeTower;
        UpgradeTowerButton.GetComponent<Image>().color = (CanUpgradeTower ^ CanDestroyTower) ? LockColor : Color.white;
    }
}