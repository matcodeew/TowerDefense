using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class T_Canon : MonoBehaviour, IShootable
{
    public Tower tower;
    private GameObject targetedEnemy;
    [SerializeField] private GameObject Bullet;
    private void Awake()
    {
        tower = GetComponent<Tower>();
    }
    public void Fire(GameObject enemyTarget)
    {
        if (tower.EnemyToKill.Count > 0)
        {
            targetedEnemy = enemyTarget;
            StartCoroutine(LunchProjectile(targetedEnemy));
        }
    }
    private IEnumerator LunchProjectile(GameObject targetedEnemy)
    {
        GameObject newBullet = Instantiate(Bullet);
        Destroy(newBullet, 1.5f);
        newBullet.transform.position = transform.position;

        while (Vector3.Distance(newBullet.transform.position, targetedEnemy.transform.position) > 0.1f)
        {
            newBullet.transform.position = Vector3.MoveTowards(
                newBullet.transform.position,
                targetedEnemy.transform.position,
                5f * Time.deltaTime
            );

            yield return null;
        }
        Collider[] AllHit = Physics.OverlapSphere(targetedEnemy.transform.position, tower.TowerData.ZoneEffect.EffectRadius, tower.layerAccept);
        foreach (var enemy in AllHit)
        {
            enemy.GetComponent<EnemyBehaviour>().TakeDamage(tower, tower.stat.Damage);
        }
        tower.EnemyTouched();
        Destroy(newBullet);
    }

    public void StartVfx(ParticleSystem VfxToUse)
    {
        if(targetedEnemy != null)
        {
            VfxToUse.gameObject.transform.position = targetedEnemy.transform.position;
            VfxToUse.transform.localScale = new Vector3(25, 25, 25);
            VfxToUse.Play();
            targetedEnemy = null;
        }
    }

    public void StartSfx(GameObject SoundToUse)
    {

    }
}
