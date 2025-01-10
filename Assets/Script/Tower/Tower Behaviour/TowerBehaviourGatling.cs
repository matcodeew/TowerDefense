using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Tower.Tower_Behaviour
{
    public class TowerBehaviourGatling : global::Tower
    {
        [Header("Needed info")]
        [SerializeField] private ParticleSystem firedPointVfx;
        [SerializeField] private Transform rotatePoint;
        [SerializeField] private Vector3 scale = new Vector3(0.3f, 0.3f, 0.3f);

        [Header("Internal state")]
        private ParticleSystem FiredVfx;
        private GameObject hittedEnemyVfx;
        private bool rotateCanon;

        [Header("Rotation Settings")]
        [SerializeField] private float rotationSpeed = 360f; // Vitesse de rotation en degrés par seconde

        protected override void Update()
        {
            base.Update();

            if (rotateCanon)
            {
                rotatePoint.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            }
        }
        protected override void InitializeTowerStats(S_Tower data)
        {
            base.InitializeTowerStats(data);
            hittedEnemyVfx = data.HitVfx;
            FiredVfx = firedPointVfx;
        }
        protected override void Fire(GameObject enemyToKill)
        {
            base.Fire(enemyToKill);
            enemyToKill.GetComponent<EnemyBehaviour>().TakeDamage(stat.Damage);
        }
        protected override void StartingVfxInRange()
        {
            rotateCanon = true;
        }
        protected override void StopingVfxInRange()
        {
            rotateCanon = false;
        }
        protected override void StartHittedEnemyVfx(GameObject enemyToKill)
        {
            FiredVfx.Play();
            if (hittedEnemyVfx != null)
            {
                GameObject vfxInstance = Instantiate(hittedEnemyVfx, enemyToKill.transform.position, Quaternion.identity);
                vfxInstance.transform.localScale = scale;
                vfxInstance.GetComponent<ParticleSystem>().Play();
                Destroy(vfxInstance, 0.15f);
            }
        }
    }
}
