using UnityEngine;


[RequireComponent(typeof(Tower))]
public class T_OneByOne : MonoBehaviour, IShootable
{
    public Tower tower;
    private GameObject targetedEnemy;
    private void Awake()
    {
        tower = GetComponent<Tower>();
    }
    public void Fire(GameObject enemyTarget)
    {
        if (enemyTarget != null && tower.EnemyToKill.Count > 0)
        {
            targetedEnemy = enemyTarget;
            enemyTarget.GetComponent<EnemyBehaviour>().TakeDamage(tower, tower.stat.Damage);
            tower.EnemyTouched();
        }
    }
    public void HitVfx(ParticleSystem VfxToUse)
    {
        VfxToUse.gameObject.transform.position = targetedEnemy.transform.position;
        VfxToUse.Play();
        if (tower.TowerData.Type == S_Tower.TowerType.Gatling)
        {
            VfxToUse.transform.localScale = new Vector3(25, 25, 25);    
        }

        targetedEnemy = null;
    }

    public void FireVfx(ParticleSystem VfxToUse)
    {
    }
    public void StartSfx(GameObject SoundToUse)
    {

    }
}
