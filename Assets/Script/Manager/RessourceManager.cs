using UnityEngine;

public class RessourceManager : MonoBehaviour
{
    public int currentGold = 0;
    public int BaseLife = 30;
    public int MaxWave = 30;
    public int CurrentWave;
    public static RessourceManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void OnEnable()
    {
        EventsManager.OnTowerDestroy += DestroyTower;
        EventsManager.OnEnemyReachEnd += EnemyMakeDamageOnBase;
        EventsManager.OnTowerBuild += HandleTowerBuilt;
        EventsManager.OnWaveStart += IncrementeWaveIndex;

    }
    private void HandleTowerBuilt(Tower tower)
    {
        currentGold -= tower.stat.GoldsCost;
    }
    private void DestroyTower(Tower tower)
    {
        currentGold += Mathf.FloorToInt(tower.stat.GoldsCost * 0.75f);
    }
    private void EnemyMakeDamageOnBase(int value)
    {
        BaseLife += value;
    }
    private void IncrementeWaveIndex(S_Enemy enemy, float quantiry)
    {
        CurrentWave++;
    }
    public bool HaveRessource(Tower tower) => currentGold >= tower.stat.GoldsCost;
}
     