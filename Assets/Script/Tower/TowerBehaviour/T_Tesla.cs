using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class T_Tesla : MonoBehaviour, IShootable
{
    public Tower tower;
    [SerializeField] private List<GameObject> AllTarget = new();
    [SerializeField] private GameObject currentTarget = null;
    private int EnemyDivider = 0;
    private void Awake()
    {
        tower = GetComponent<Tower>();
    }
    public void Fire(GameObject enemyTarget)
    {
        AllTarget.Clear();
        EnemyDivider = 0;
        if(tower.EnemyToKill.Count > 0)
        {
            MakeChainEffect();
        }
    }
    public void StartVfx(ParticleSystem VfxToUse)
    {
        VfxToUse.gameObject.transform.position = currentTarget.transform.position;
        VfxToUse.Play();
    }
    public void EndVfx(ParticleSystem VfxToUse)
    {
        VfxToUse.Stop();
        VfxToUse.gameObject.transform.position = Vector3.zero;
    }

    public void StartSfx(GameObject SoundToUse)
    {

    }
    private void MakeChainEffect()
    {
        for (int i = 0; i < tower.TowerData.ZoneEffect.MaxEnemyChain; i++)
        {
            if (i >= tower.EnemyToKill.Count) { break; }
            AllTarget.Add(tower.EnemyToKill[i]);
        }
        StartCoroutine(MakeDamage());
    }

    private IEnumerator MakeDamage()
    {
        foreach (var enemy in AllTarget)
        {
            EnemyDivider++;
            currentTarget = enemy;
            StartVfx(tower.towerParticleSystem);
            enemy.GetComponent<EnemyBehaviour>().TakeDamage(tower, tower.stat.Damage / EnemyDivider);

            yield return new WaitForSeconds(0.2f);
            EndVfx(tower.towerParticleSystem);
        }
        currentTarget = null;
    }
}
