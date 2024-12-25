using System.Collections;
using UnityEngine;

namespace Script.Tower.Tower_Behaviour
{
    public class Tower_FlameThrower : global::Tower
    {
        [SerializeField] private float activeDuration = 4.0f;
        [SerializeField] private float cooldownDurantion = 2.50f;
        private bool isFiring;
        private bool onCooldown;
        protected override void Fire(GameObject enemyToKill)
        {
            if (!isFiring && !onCooldown)
            {
                StartCoroutine(BoxCastRoutine());
            }
        }

        private void OnDrawGizmos()
        {
            // Couleur pour la port�e de la zone d'effet
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f); // Orange translucide
            Gizmos.DrawWireSphere(transform.position, towerData != null ? towerData.ZoneEffect.EffectRadius : 1f);

            // Couleur pour indiquer la direction de l'attaque
            Gizmos.color = Color.red;
            Vector3 forward = transform.forward * (towerData != null ? towerData.ZoneEffect.EffectRadius : 1f);
            Vector3 start = transform.position;
            Gizmos.DrawLine(start, start + forward);

            // Dessiner un rectangle repr�sentant la BoxCast (port�e approximative)
            Gizmos.color = new Color(1f, 0f, 0f, 0.2f); // Rouge translucide
            Vector3 boxSize = new Vector3(2f, 2f, towerData != null ? towerData.ZoneEffect.EffectRadius : 1f);
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.forward * boxSize.z / 2, boxSize);
        }

        private IEnumerator BoxCastRoutine()
        {
            isFiring = true;
            float elapsedTime = 0f;

            while (elapsedTime < activeDuration)
            {
                elapsedTime += Time.deltaTime;

                RaycastHit[] AllTarget = Physics.RaycastAll(transform.position, transform.forward, towerData.ZoneEffect.EffectRadius, layerAccept);

                foreach (var enemy in AllTarget)
                {
                    if (enemy.collider.TryGetComponent<EnemyBehaviour>(out var enemyBehaviour))
                    {
                        enemyBehaviour.TakeDamage(this, stat.Damage);
                    }
                }
                yield return null;
            }
            isFiring = false;
            StartCoroutine(CooldownRoutine());
        }

        protected override void RotateTower()
        {
            print("this tower can't move");
        }

        private IEnumerator CooldownRoutine()
        {
            onCooldown = true;
            yield return new WaitForSeconds(cooldownDurantion);

            onCooldown = false;
        }
    }
}
