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
        EventsManager.OnTowerBuilt += HandleTowerBuilt;
        EventsManager.OnModifieBaseLife += ChangeBaseLife;
        EventsManager.OnTowerDestroy += DestroyTower;
    }

    private void OnDisable()
    {
        EventsManager.OnTowerBuilt -= HandleTowerBuilt;
        EventsManager.OnModifieBaseLife -= ChangeBaseLife;
        EventsManager.OnTowerDestroy -= DestroyTower;
    }

    private void HandleTowerBuilt(IBuildable tower, Vector3 position)
    {
        Tower _tower  = tower as Tower;

        currentGold -= _tower.stat.GoldsCost;
    }
    private void DestroyTower(Tower tower)
    {
        currentGold += Mathf.FloorToInt(tower.stat.GoldsCost * 0.75f);
    }

    private void ChangeBaseLife(int value)
    {
        BaseLife += value;
    }

    public bool HaveRessource(Tower tower) => currentGold >= tower.stat.GoldsCost;
}
     