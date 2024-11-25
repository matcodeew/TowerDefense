using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

[RequireComponent(typeof(Tower))]
public class T_FlameThrower : MonoBehaviour, IShootable
{
    public Tower tower;
    [SerializeField] private float activeDuration = 4.0f;
    [SerializeField] private float cooldownDurantion = 2.50f;
    private bool isFiring;
    private bool onCooldown;

    List<GameObject> Enemy;

    private void Awake()
    {
        tower = GetComponent<Tower>();
    }

    public void Fire(GameObject enemyTarget)
    {
        Enemy = tower.EnemyToKill;
        if (!isFiring && !onCooldown)
        {
            StartCoroutine(BoxCastRoutine());
        }
    }

    private IEnumerator BoxCastRoutine()
    {
        isFiring = true;
        float elapsedTime = 0f;

        while (elapsedTime < activeDuration)
        {
            elapsedTime += Time.deltaTime;

        foreach (var enemy in Enemy)
            {
                if (enemy.TryGetComponent<EnemyBehaviour>(out var enemyBehaviour))
                {
                    enemyBehaviour.TakeDamage(tower, enemy, tower.TowerData.Damage);
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
