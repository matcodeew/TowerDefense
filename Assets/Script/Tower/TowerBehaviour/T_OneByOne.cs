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
            enemyTarget.GetComponent<EnemyBehaviour>().TakeDamage(tower, tower.TowerData.Damage);
            EventsManager.TowerFire(tower as IShootable, enemyTarget);
        }
    }
    public void StartVfx(ParticleSystem VfxToUse)
    {
        VfxToUse.gameObject.transform.position = targetedEnemy.transform.position;
        VfxToUse.Play();

        targetedEnemy = null;
    }

    public void StartSfx(GameObject SoundToUse)
    {

    }
}
