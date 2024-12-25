using UnityEngine;

public class Building : MonoBehaviour
{
    public virtual void Upgrade()
    {
        print($"upgrade {gameObject.name}");
    }

    public virtual void Destroy()
    {
        Destroy(gameObject);
        print($"destroy {gameObject.name}");
    }

    public virtual GameObject Build(GameObject BuildToInstantiate, Vector3 position)
    {
        GameObject newBuilding = Instantiate(BuildToInstantiate);
        newBuilding.transform.position = position;
        return newBuilding;
    }
}

