using System.Collections;
using UnityEngine;

namespace Script.Tower.Tower_Behaviour
{
    public class TowerBehaviourFlameThrower : global::Tower
    {
        [Header("Needed Tower Info")]
        [SerializeField] private GameObject FiredPoint;
        [SerializeField] private Vector3 VfxScale = new Vector3(0.3f, 0.3f, 0.3f);

        [Header("Internal stat")]
        [SerializeField] private float activeDuration = 4.0f;
        [SerializeField] private float cooldownDurantion = 2.50f;
        private bool isFiring;
        private bool onCooldown;
        private ParticleSystem FiredVfx;
        private GameObject hittedEnemyVfx;

        protected override void InitializeTowerStats(S_Tower data)
        {
            base.InitializeTowerStats(data);
            hittedEnemyVfx = data.HitVfx;
            FiredVfx = FiredPoint.GetComponent<ParticleSystem>();
            FiredVfx.Pause();
        }
        protected override void StartHittedEnemyVfx(GameObject enemyToKill)
        {
            if (hittedEnemyVfx != null)
            {
                GameObject vfxInstance = Instantiate(hittedEnemyVfx, enemyToKill.transform.position, Quaternion.identity);
                vfxInstance.transform.localScale = VfxScale;
                vfxInstance.transform.parent = enemyToKill.transform;
                vfxInstance.GetComponent<ParticleSystem>().Play();
                Destroy(vfxInstance, DebuffLibrary.Instance.debuffs[DebuffLibrary.DebuffType.Fire].duration);
            }
        }
        protected override void Fire(GameObject enemyToKill)
        {
            if (!isFiring && !onCooldown)
            {
                StartCoroutine(BoxCastRoutine());
            }
        }
        private IEnumerator BoxCastRoutine()
        {
            isFiring = true;
            float elapsedTime = 0f;

            while (elapsedTime < activeDuration)
            {
                 FiredVfx.Play();
                elapsedTime += Time.deltaTime;

                float size = towerData.ZoneEffect.EffectRadius; 
                Vector3 boxSize;

                float angleY = FiredPoint.transform.eulerAngles.y; 
                if ((angleY >= 45f && angleY < 135f) || (angleY >= 225f && angleY < 315f))
                {
                    boxSize = new Vector3(0.9f, 1f, size);
                }
                else
                {
                    boxSize = new Vector3(size, 1f, 0.9f);
                }
                Vector3 boxCenter = FiredPoint.transform.position + FiredPoint.transform.forward * size / 2f;

                RaycastHit[] allTarget = Physics.BoxCastAll(boxCenter, boxSize / 2, FiredPoint.transform.forward, Quaternion.identity, towerData.ZoneEffect.EffectRadius, layerAccept);
                foreach (var enemy in allTarget)
                {
                    if (enemy.collider.TryGetComponent<EnemyBehaviour>(out var enemyBehaviour))
                    {
                        enemyBehaviour.HasDOT = true;
                        enemyBehaviour.ApplyDebuff(DebuffLibrary.DebuffType.Fire, stat.Damage);
                        StartHittedEnemyVfx(enemy.collider.gameObject);
                    }
                }
                yield return null;
            }
            isFiring = false;
            FiredVfx.Stop();
            StartCoroutine(CooldownRoutine());
        }
        protected override void RotateTower()
        {
            //This tower cant move.
        }

        private IEnumerator CooldownRoutine()
        {
            onCooldown = true;
            yield return new WaitForSeconds(cooldownDurantion);

            onCooldown = false;
        }
    }
}
