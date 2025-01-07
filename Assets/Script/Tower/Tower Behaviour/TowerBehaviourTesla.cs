using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Tower.Tower_Behaviour
{
    public class TowerBehaviourTesla : global::Tower
    {
        [SerializeField] private List<GameObject> AllTarget = new();
        private int EnemyDivider = 0;

        protected override void Fire(GameObject enemyToKill)
        {
            AllTarget.Clear();
            EnemyDivider = 0;
            MakeChainEffect();
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
                enemy.GetComponent<EnemyBehaviour>().TakeDamage(this, stat.Damage / EnemyDivider);

                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
