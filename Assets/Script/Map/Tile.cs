using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsOccupied;

    public void SetTileLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("TileGround");
    }
}
