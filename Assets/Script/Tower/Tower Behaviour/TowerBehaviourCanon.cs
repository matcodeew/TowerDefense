using UnityEngine;

namespace Script.Tower.Tower_Behaviour
{
    public class TowerBehaviourCanon : global::Tower
    {
        protected override void Fire(GameObject enemyToKill)
        {
            if (towerData is not null)
            {
                Collider[] allHit = Physics.OverlapSphere(enemyToKill.transform.position, towerData.ZoneEffect.EffectRadius, layerAccept);
                foreach (var enemy in allHit)
                {
                    enemy.GetComponent<EnemyBehaviour>().TakeDamage(this, towerData.Damage);
                }
            }
        }
    }
}
