using UnityEngine;


[RequireComponent(typeof(Tower))]
public class T_OneByOne : MonoBehaviour, IShootable
{
    public Tower tower;
    private void Awake()
    {
        tower = GetComponent<Tower>();
    }
    public void Fire(GameObject enemyTarget)
    {
        if (enemyTarget != null)
        {
            enemyTarget.GetComponent<EnemyBehaviour>().TakeDamage(tower, enemyTarget, tower.TowerData.Damage);
            EventsManager.TowerFire(tower as IShootable, enemyTarget);
        }
    }

}
