using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    public static TowerBuilder Instance;
    public S_Tower towerPrefab;
    public bool UpdatePos;
    private GameObject preview;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private bool HaveRessource() => RessourceManager.Instance.currentGold >= towerPrefab.GoldsCost;
    public void BuildTower(S_Tower data, Vector3 position)
    {
        GameObject newTower = Instantiate(towerPrefab.Prefab, position, Quaternion.identity);
        IBuildable buildableTower = newTower.GetComponent<IBuildable>();

        if (buildableTower != null)
        {
            buildableTower.Build(data, position);
            EventsManager.TowerBuilt(buildableTower, position);
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
                        BuildTower(towerPrefab, hit.collider.transform.position + new Vector3(0, 1, 0));
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
            preview = Instantiate(towerPrefab.PreviewPrefab);
        }
    }
    public void PosTower(S_Tower tower)
    {
        towerPrefab = tower;
        if (HaveRessource())
        {
            UpdatePos = true;
            MakePreview();
        }
        else
        {
            print($"{RessourceManager.Instance.currentGold} is less than {towerPrefab.GoldsCost}");
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