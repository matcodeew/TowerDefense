using UnityEngine;

namespace Script.Tower.Tower_Behaviour
{
    public class Tower_Canon : global::Tower
    {
        protected override void Fire(GameObject enemyToKill)
        {
            if (towerData != null)
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
