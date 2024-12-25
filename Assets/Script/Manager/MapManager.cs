using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [Header("MoveCase")]
    [SerializeField] private float hoverHeight = 0.2f;
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private Material SelectedMat;
    [SerializeField] private Material currentMat;
    private Transform currentTile;

    private Transform lastHoveredTile;
    private Vector3 targetPosition;
    private Vector3 originalPosition;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }
    void Update()
    {
        if (TowerBuilderManager.Instance is null || TowerBuilderManager.Instance.DragTower == false) return;


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, gridLayer))
        {
            currentTile = hit.transform;
            if (currentTile != lastHoveredTile)
            {
                if (lastHoveredTile is not null)
                {
                    lastHoveredTile.position = originalPosition;
                    lastHoveredTile.GetComponent<MeshRenderer>().material = currentMat;
                }
                lastHoveredTile = currentTile;
                originalPosition = currentTile.position;
                targetPosition = originalPosition + new Vector3(0, hoverHeight, 0);
                currentTile.position = targetPosition;
                currentTile.GetComponent<MeshRenderer>().material = SelectedMat;
            }
        }
        else
        {
            ResetHeightTile();
        }
    }

    public void ResetHeightTile()
    {
        if (lastHoveredTile is not null)
        {
            lastHoveredTile.GetComponent<MeshRenderer>().material = currentMat;
            lastHoveredTile.position = originalPosition;
            lastHoveredTile = null;
        }
    }
}
