using UnityEngine;

public class Tower : MonoBehaviour, IBuildable, IUpgradeable, IShootable
{
    [SerializeField] public S_Tower TowerData;

    public float radius = 2f; 
    public float maxDistance = 10f;

    private void Start()
    {
        Fire();
    }
    public void Build(S_Tower data, Vector3 position)
    {
        transform.position = position;
        print("Tower Build succesful");
    }
    public void Fire()
    {

    }
    private void Update()
    {
        Vector2 direction = Vector2.right;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, direction, maxDistance);

        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log("Hit: " + hit.collider.name);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius); 
    }
    public void Upgrade()
    {
        //Upgrate Tower 
    }

    private void InitializeTower(S_Tower data)
    {
        TowerData = data;
    }
}
