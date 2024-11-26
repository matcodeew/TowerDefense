using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class T_FlameThrower : MonoBehaviour, IShootable
{
    public Tower tower;
    [SerializeField] private float activeDuration = 4.0f;
    [SerializeField] private float cooldownDurantion = 2.50f;
    private bool isFiring;
    private bool onCooldown;

    private float maxDistance;

  private void Awake()
    {
        tower = GetComponent<Tower>();
        maxDistance = tower.TowerData.ZoneEffect.EffectRadius;
    }

    public void Fire(GameObject enemyTarget)
    {
        if ( (!isFiring && !onCooldown) && tower.EnemyToKill.Count > 0)
        {
            StartCoroutine(BoxCastRoutine());
        }
    }
    public void StartVfx(ParticleSystem VfxToUse)
    {

    }

    public void StartSfx(GameObject SoundToUse)
    {

    }

    private IEnumerator BoxCastRoutine()
    {
        isFiring = true;
        float elapsedTime = 0f;

        while (elapsedTime < activeDuration)
        {
            elapsedTime += Time.deltaTime;

            RaycastHit[] AllTarget = Physics.RaycastAll(transform.position, transform.forward, maxDistance, tower.layerAccept);

            foreach (var enemy in AllTarget)
            {
                if (enemy.collider.TryGetComponent<EnemyBehaviour>(out var enemyBehaviour))
                {
                    enemyBehaviour.TakeDamage(tower, tower.TowerData.Damage);
                }
            }
            yield return null;
        }
        isFiring = false;
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldownDurantion);

        onCooldown = false;
    }
}
