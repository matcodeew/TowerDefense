using System.Runtime.CompilerServices;
using UnityEngine;

public class TowerFire : MonoBehaviour
{
    public static TowerFire Instance;
    public Tower tower;
    private float _timer;
    private GameObject targetedEnemy;
    private void Awake()
    {
        tower = GetComponent<Tower>();
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Update()
    {
        if ((tower.isPosed))
        {
            _timer += Time.deltaTime;
            if (_timer >= tower.TowerData.FireRate && tower.EnemyToKill.Count > 0) // metre timer a zero quand pose tour
            {
                _timer = 0.0f;
                TowerShooting();
            }
        }
    }
    private void TowerShooting()
    {
        targetedEnemy = tower.EnemyToKill[0];
        if(targetedEnemy != null)
        {
            targetedEnemy.GetComponent<EnemyBehaviour>().TakeDamage(tower, targetedEnemy);
            tower.Fire();
            EventsManager.TowerFire(tower, targetedEnemy);
        }
    }
}
