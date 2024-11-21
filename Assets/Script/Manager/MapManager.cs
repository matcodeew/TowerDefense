using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("All tile on map")]
    [SerializeField] public List<GameObject> AllTileOnMap = new List<GameObject>();
    [Header("Tile Material")]
    [SerializeField] private Material EnnemyPathMat;
    [SerializeField] private Material TowerTileMat;

    private void Start()
    {
        AlocateTileTag();
    }


    #region Give Tile Tag
    private void AlocateTileTag()
    {
        foreach (GameObject tile in AllTileOnMap)
        {
            Material tileMat = tile.GetComponentInChildren<MeshRenderer>().sharedMaterial;
            if (tileMat == EnnemyPathMat)
            {
                tile.tag = "Path";
            }
            else if (tileMat == TowerTileMat)
            {
                tile.tag = "TowerTile";
            }
        }
    }
    #endregion
}
