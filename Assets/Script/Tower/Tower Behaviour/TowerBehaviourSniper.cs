using UnityEngine;
using UnityEngine.UIElements;
public class TowerBehaviourSniper : global::Tower
{
    [Header("Needed info")]
    [SerializeField] private Vector3 scale = new Vector3(0.3f, 0.3f, 0.3f);

    [Header("Internal state")]
    private GameObject hittedEnemyVfx;

    protected override void InitializeTowerStats(S_Tower data)
    {
        base.InitializeTowerStats(data);
        hittedEnemyVfx = data.HitVfx;
    }
    protected override void Fire(GameObject enemyToKill)
    {
        base.Fire(enemyToKill);
        enemyToKill.GetComponent<EnemyBehaviour>().TakeDamage(stat.Damage);
    }
    protected override void StartHittedEnemyVfx(GameObject enemyToKill)
    {
        if (hittedEnemyVfx != null)
        {
            GameObject vfxInstance = Instantiate(hittedEnemyVfx, enemyToKill.transform.position, Quaternion.identity);
            vfxInstance.transform.localScale = scale;
            vfxInstance.GetComponent<ParticleSystem>().Play();
            Destroy(vfxInstance, 0.15f);
        }
    }
}
