using UnityEngine;

[RequireComponent(typeof(Tower))]
public class T_Canon : MonoBehaviour, IShootable
{
    public Tower tower;
    private GameObject targetedEnemy;
    private void Awake()
    {
        tower = GetComponent<Tower>();
    }
    public void Fire(GameObject enemyTarget)
    {
        if (tower.EnemyToKill.Count > 0){
            targetedEnemy = enemyTarget;
            Collider[] AllHit = Physics.OverlapSphere(targetedEnemy.transform.position, tower.TowerData.ZoneEffect.EffectRadius, tower.layerAccept);
            foreach (var enemy in AllHit)
            {
                enemy.GetComponent<EnemyBehaviour>().TakeDamage(tower, tower.TowerData.Damage);
            }
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
