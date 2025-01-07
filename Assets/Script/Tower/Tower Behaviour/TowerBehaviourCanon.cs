using UnityEngine;

namespace Script.Tower.Tower_Behaviour
{
    public class TowerBehaviourCanon : global::Tower
    {
        [Header("Needed info")]
        [SerializeField] private GameObject firedPoint;
        [SerializeField] private Vector3 scale = new Vector3(0.3f, 0.3f, 0.3f);

        [Header("Internal state")]
        private ParticleSystem FiredVfx;
        private GameObject hittedEnemyVfx;

        protected override void InitializeTowerStats(S_Tower data)
        {
            base.InitializeTowerStats(data);
            hittedEnemyVfx = data.HitVfx;
            FiredVfx = firedPoint.GetComponent<ParticleSystem>();
            scale = new Vector3(data.ZoneEffect.EffectRadius, data.ZoneEffect.EffectRadius, data.ZoneEffect.EffectRadius);
        }
        protected override void Fire(GameObject enemyToKill)
        {
            base.Fire(enemyToKill);
            if (towerData is not null)
            {
                Collider[] allHit = Physics.OverlapSphere(enemyToKill.transform.position, towerData.ZoneEffect.EffectRadius, layerAccept);
                foreach (var enemy in allHit)
                {
                    enemy.GetComponent<EnemyBehaviour>().TakeDamage(towerData.Damage);
                }
            }
        }
        protected override void StartHittedEnemyVfx(GameObject enemyToKill)
        {
            FiredVfx.Play();
            if (hittedEnemyVfx != null)
            {
                GameObject vfxInstance = Instantiate(hittedEnemyVfx, enemyToKill.transform.position, Quaternion.identity);
                vfxInstance.transform.localScale = scale;
                vfxInstance.GetComponent<ParticleSystem>().Play();
                Destroy(vfxInstance, 0.3f);
            }
        }
    }
}
