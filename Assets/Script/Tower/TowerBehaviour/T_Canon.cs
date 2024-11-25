using UnityEngine;

[RequireComponent(typeof(Tower))]
public class T_Canon : MonoBehaviour, IShootable
{
    public Tower tower;
    private GameObject targetedEnemy;
    private void Awake()
    {
        tower = GetComponent<Tower>();
    }
    public void Fire(GameObject enemyTarget)
    {
        targetedEnemy = enemyTarget;
        Collider[] AllHit = Physics.OverlapSphere(targetedEnemy.transform.position, tower.TowerData.ZoneEffect.EffectRadius, tower.layerAccept);
        foreach (var enemy in AllHit)
        {
            enemy.GetComponent<EnemyBehaviour>().TakeDamage(tower, enemy.gameObject, tower.TowerData.Damage);
        }
        targetedEnemy = null;
    }
    private void OnDrawGizmos()
    {
        if (targetedEnemy != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(targetedEnemy.transform.position, tower.TowerData.ZoneEffect.EffectRadius);
        }
    }
}
