using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Tower.Tower_Behaviour
{
    public class TowerBehaviourTesla : global::Tower
    {
        [Header("Needed Tower Info")]
        [SerializeField] private GameObject FiredPoint;
        [SerializeField] private Vector3 VfxScale = new Vector3(0.3f, 0.3f, 0.3f);

        [Header("Internal Info")]
        private List<GameObject> AllTarget = new();
        private int EnemyDivider = 0;
        private GameObject hittedEnemyVfx;
        private ParticleSystem FiredVfx;

        protected override void InitializeTowerStats(S_Tower data)
        {
            base.InitializeTowerStats(data);
            hittedEnemyVfx = data.HitVfx;
            FiredVfx = FiredPoint.GetComponent<ParticleSystem>();
            FiredVfx.Pause();
        }
        protected override void Fire(GameObject enemyToKill)
        {
            AllTarget.Clear();
            EnemyDivider = 0;
            MakeChainEffect();
        }

        protected override void StartingVfxInRange()
        {
            FiredVfx.Play();
        }
        protected override void StopingVfxInRange()
        {
            FiredVfx.Stop();
        }
        protected override void StartHittedEnemyVfx(GameObject enemyToKill)
        {
            if (hittedEnemyVfx != null)
            {
                GameObject vfxInstance = Instantiate(hittedEnemyVfx, enemyToKill.transform.position, Quaternion.identity);
                vfxInstance.transform.localScale = VfxScale;
                vfxInstance.GetComponent<ParticleSystem>().Play();
                Destroy(vfxInstance, 0.15f);
            }
        }
        private void MakeChainEffect()
        {
            for (int i = 0; i < towerData.ZoneEffect.MaxEnemyChain; i++)
            {
                if (i >= EnemyToKill.Count) { break; }
                AllTarget.Add(EnemyToKill[i]);
            }
            StartCoroutine(MakeDamage());
        }

        private IEnumerator MakeDamage()
        {
            foreach (var enemy in AllTarget)
            {
                EnemyDivider++;
                StartHittedEnemyVfx(enemy);
                enemy.GetComponent<EnemyBehaviour>().TakeDamage(stat.Damage / EnemyDivider);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
