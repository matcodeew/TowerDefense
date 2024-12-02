using UnityEngine;

[RequireComponent(typeof(Tower))]
public class T_Canon : MonoBehaviour, IShootable
{
    public Tower tower;
    private GameObject targetedEnemy;
    private Vector3 EnemyPos = Vector3.zero;
    [SerializeField] private GameObject FireFlash;
    private void Awake()
    {
        tower = GetComponent<Tower>();
    }
    public void Fire(GameObject enemyTarget)
    {
        if (tower.EnemyToKill.Count > 0)
        {
            targetedEnemy = enemyTarget;
            EnemyPos = targetedEnemy.transform.position;
            Collider[] AllHit = Physics.OverlapSphere(targetedEnemy.transform.position, tower.TowerData.ZoneEffect.EffectRadius, tower.layerAccept);
            foreach (var enemy in AllHit)
            {
                enemy.GetComponent<EnemyBehaviour>().TakeDamage(tower, tower.TowerData.Damage);
            }
            tower.EnemyTouched();
        }
    }
    public void HitVfx(ParticleSystem VfxToUse)
    {
        if (targetedEnemy != null)
        {
            VfxToUse.gameObject.transform.position = EnemyPos;
            VfxToUse.transform.localScale = new Vector3(25, 25, 25);
            VfxToUse.Play();
            targetedEnemy = null;
        }
    }
    public void FireVfx(ParticleSystem VfxToUse)
    {
        
    }

    public void StartSfx(GameObject SoundToUse)
    {

    }
}
