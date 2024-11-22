using UnityEngine;

public class RessourceManager : MonoBehaviour
{
    public int currentGold = 0;
    public int BaseLife = 30;
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

        currentGold -= _tower.TowerData.GoldsCost;
    }
    private void DestroyTower(Tower tower)
    {
        currentGold += Mathf.FloorToInt(tower.TowerData.GoldsCost * 0.75f);
    }

    private void ChangeBaseLife(int value)
    {
        BaseLife += value;
    }
}
     