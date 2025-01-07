using UnityEngine;

namespace Script.Tower.Tower_Behaviour
{
    public class TowerBehaviourGatling : global::Tower
    {
        protected override void Fire(GameObject enemyToKill)
        {
            enemyToKill.GetComponent<EnemyBehaviour>().TakeDamage(this, stat.Damage);
        }
    }
}
