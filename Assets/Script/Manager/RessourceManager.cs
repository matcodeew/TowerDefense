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
    }

    private void OnDisable()
    {
        EventsManager.OnTowerBuilt -= HandleTowerBuilt;
        EventsManager.OnModifieBaseLife -= ChangeBaseLife;
    }

    private void HandleTowerBuilt(IBuildable tower, Vector3 position)
    {
        Tower _tower  = tower as Tower;

        currentGold -= _tower.TowerData.GoldsCost;
        Debug.Log("Reducing resources for the tower construction.");
    }

    private void ChangeBaseLife(int value)
    {
        BaseLife += value;
    }
}
     