using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [Header("MoveCase")]
    [SerializeField] private float hoverHeight = 0.2f;
    [SerializeField] private float hoverSpeed = 5f;
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private Material SelectedMat;
    [SerializeField] private Material currentMat;
    private Transform currentTile;

    private Transform lastHoveredTile;
    private Vector3 targetPosition;
    private Vector3 originalPosition;

    //[Header("Grid Value")]
    //[SerializeField] private int width = 10;
    //[SerializeField] private int height = 10;
    //[SerializeField] private float YPos = 0.8f;
    //[SerializeField] private float cellSize = 1f;
    //[SerializeField] private Material lineMaterial; 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }
    void Update()
    {
        if (TowerBuilder.Instance == null || TowerBuilder.Instance.DragTower == false) return;


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, gridLayer))
        {
            currentTile = hit.transform;
            if (currentTile != lastHoveredTile)
            {
                if (lastHoveredTile != null)
                {
                    lastHoveredTile.position = originalPosition;
                    //lastHoveredTile.GetComponent<MeshRenderer>().material = currentMat;
                }

                lastHoveredTile = currentTile;
                originalPosition = currentTile.position;
                targetPosition = originalPosition + new Vector3(0, hoverHeight, 0);
                currentTile.position = targetPosition;
                currentTile.GetComponent<MeshRenderer>().material = null /*SelectedMat*/;
            }

        }
        else
        {
            ResetHeightTile();
        }
    }

    public void ResetHeightTile()
    {
        if (lastHoveredTile != null)
        {
            lastHoveredTile.GetComponent<MeshRenderer>().material = currentMat;
            lastHoveredTile.position = originalPosition;
            lastHoveredTile = null;
        }
    }


    //Line Renderer Grid

    //private void Start()
    //{
    //    CreateGrid();
    //}

    //private void CreateGrid()
    //{
    //    for (int x = 0; x <= width; x++)
    //    {
    //        CreateLine(
    //            new Vector3(x * cellSize, YPos, 0),
    //            new Vector3(x * cellSize, YPos, height * cellSize)
    //        );
    //    }

    //    for (int z = 0; z <= height; z++)
    //    {
    //        CreateLine(
    //            new Vector3(0, YPos, z * cellSize),
    //            new Vector3(width * cellSize, YPos, z * cellSize)
    //        );
    //    }
    //}
    //private void CreateLine(Vector3 start, Vector3 end)
    //{
    //    GameObject line = new GameObject("GridLine");
    //    line.transform.parent = transform;

    //    LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
    //    lineRenderer.material = lineMaterial;
    //    lineRenderer.startWidth = 0.05f;
    //    lineRenderer.endWidth = 0.05f;
    //    lineRenderer.positionCount = 2;
    //    lineRenderer.SetPosition(0, start);
    //    lineRenderer.SetPosition(1, end);
    //    lineRenderer.useWorldSpace = true;
    //}
}
