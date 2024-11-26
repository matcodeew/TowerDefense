using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    public static TowerBuilder Instance;

    public S_Tower tower;
    public bool UpdatePos;
    private GameObject preview;
    [SerializeField] private LayerMask TowerLayer;
    public Quaternion previewRotation;

    public bool CanDestroyTower = false;

    public List<Tile> TilesOccupied = new();
    public List<Tower> AllTowerPosedOnMap = new();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private bool HaveRessource() => RessourceManager.Instance.currentGold >= tower.GoldsCost;
    public void BuildTower(S_Tower data, Vector3 position)
    {
        GameObject newTower = Instantiate(tower.Prefab, position, previewRotation);
        IBuildable buildableTower = newTower.GetComponent<IBuildable>();

        AllTowerPosedOnMap.Add(buildableTower as Tower);

        if (buildableTower != null)
        {
            newTower.GetComponent<Tower>().isPosed = true;
            buildableTower.Build(data, position);
            newTower.layer = TowerLayer;
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
                preview.transform.position = hit.point;
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.collider.CompareTag("TowerTile"))
                    {
                        Tile tile = hit.collider.gameObject.GetComponent<Tile>();
                        if (tile != null && !tile.IsOccupied)
                        {
                            CancelPreview();
                            BuildTower(tower, hit.collider.transform.position + new Vector3(0, 1, 0));
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
        if (HaveRessource())
        {
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
        if(preview != null)
        {
            previewRotation = preview.transform.rotation;
            Destroy(preview);
        }

        preview = null;
        UpdatePos = false;
    }
    #endregion

    public void CanDestroy() => CanDestroyTower = !CanDestroyTower;
}