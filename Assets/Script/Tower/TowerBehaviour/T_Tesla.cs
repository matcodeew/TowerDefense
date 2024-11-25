using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class T_Tesla : MonoBehaviour, IShootable
{
    public Tower tower;
    private List<GameObject> AllTarget = new();
    private int EnemyDivider = 0;
    private void Awake()
    {
        tower = GetComponent<Tower>();
    }
    public void Fire(GameObject enemyTarget)
    {
        AllTarget.Clear();
        EnemyDivider = 0;

        MakeChainEffect();
    }

    private void MakeChainEffect()
    {
        for (int i = 0; i < tower.TowerData.ZoneEffect.MaxEnemyChain; i++)
        {
            if(i >= tower.EnemyToKill.Count) { break; }
            AllTarget.Add(tower.EnemyToKill[i]);
        }

        foreach (var a in AllTarget)
        {
            EnemyDivider++;
            a.GetComponent<EnemyBehaviour>().TakeDamage(tower, a, tower.TowerData.Damage / EnemyDivider); 
        }
    }
}
