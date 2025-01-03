using NUnit.Framework;
using UnityEngine;
using UnityEngine.Timeline;

public class Tile : MonoBehaviour
{
    public bool IsOccupied;

    public void SetTileLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("TileGround");
    }
}
