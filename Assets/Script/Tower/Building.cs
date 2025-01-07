using UnityEngine;

public class Building : MonoBehaviour
{
    [System.Serializable]
    public struct BuildingStat
    {
        public int Health;
        public int GoldsCost;
    }
    public Tile tileOnGround;
    public BuildingStat buildingStat;
    public virtual void Upgrade()
    {
        print($"upgrade {gameObject.name}");
    }

    public virtual void DestroyBuilding()
    {
        RessourceManager.AddGold(buildingStat.GoldsCost);
        
        Destroy(gameObject);
        print($"destroy {gameObject.name}");
        tileOnGround.IsOccupied = false;
        tileOnGround.SetTileLayer();
        tileOnGround = null;
    }

    public virtual GameObject Build(GameObject BuildToInstantiate, Transform transform, int goldUse, Tile buildOnTile)
    {
        tileOnGround = buildOnTile;
        GameObject newBuilding = Instantiate(BuildToInstantiate);
        newBuilding.transform.position = transform.position;
        newBuilding.transform.rotation = transform.rotation;
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

