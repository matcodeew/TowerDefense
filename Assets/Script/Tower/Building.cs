using UnityEngine;

public class Building : MonoBehaviour
{
    public Tile tileOnGround;
    public virtual void Upgrade()
    {
        print($"upgrade {gameObject.name}");
    }

    public virtual void DestroyBuilding()
    {
        Destroy(gameObject);
        print($"destroy {gameObject.name}");
        tileOnGround.IsOccupied = false;
        tileOnGround.SetTileLayer();
        tileOnGround = null;
    }

    public virtual GameObject Build(GameObject BuildToInstantiate, Vector3 position, int goldUse, Tile buildOnTile)
    {
        tileOnGround = buildOnTile;
        GameObject newBuilding = Instantiate(BuildToInstantiate);
        newBuilding.transform.position = position;
        RessourceManager.LoseGold(goldUse);
        return newBuilding;
    }
    public void OnMouseDown()
    {
        if (TowerBuilderManager.Instance.CanDestroyTower) //move this to a global script
        {
            DestroyBuilding();
        }
        else if (TowerBuilderManager.Instance.CanUpgradeTower)
        {
            Upgrade();
        }
    }
}

