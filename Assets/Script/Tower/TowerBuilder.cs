using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    public static TowerBuilder Instance;
    public S_Tower tower;
    public bool UpdatePos;
    private GameObject preview;

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
        GameObject newTower = Instantiate(tower.Prefab, position, Quaternion.identity);
        IBuildable buildableTower = newTower.GetComponent<IBuildable>();

        AllTowerPosedOnMap.Add(buildableTower as Tower); // remove if destroy tower

        if (buildableTower != null)
        {
            buildableTower.Build(data, position);
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
                        CancelPreview();
                        BuildTower(tower, hit.collider.transform.position + new Vector3(0, 1, 0));
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
            preview.GetComponent<SphereCollider>().enabled = false;
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
        Destroy(preview);
        preview = null;
        UpdatePos = false;
    }
    #endregion
}