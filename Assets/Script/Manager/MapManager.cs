using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    [SerializeField] private List<GameObject> _tiles = new();
    [SerializeField] private List<GameObject> _decords = new();
    [SerializeField] private Vector3 offsett;

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
        if (Instance is null)
            Instance = this;
    }
    private void Start()
    {
        CheckOccupiedTileOnMap();
    }

    private void CheckOccupiedTileOnMap()
    {
        HashSet<Vector3> decordPositions = new HashSet<Vector3>();
        foreach (var decord in _decords)
        {
            decordPositions.Add(decord.transform.position);
        }

        foreach (var tile in _tiles)
        {
            Vector3 tilePositionWithOffset = tile.transform.position + offsett;
            if (decordPositions.Contains(tilePositionWithOffset))
            {
                var tileComponent = tile.GetComponent<Tile>();
                if (tileComponent != null)
                {
                    tileComponent.IsOccupied = true;
                }
                tile.layer = 0;
            }
        }
    }
    void Update()
    {
        if (TowerBuilderManager.Instance is null || TowerBuilderManager.Instance.DragTower == false)
        {
            ResetHeightTile();
            return;
        }


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
