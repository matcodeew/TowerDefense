using UnityEngine;

public class RessourceManager : MonoBehaviour
{
    public int currentGold = 0;
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
    }

    private void OnDisable()
    {
        EventsManager.OnTowerBuilt -= HandleTowerBuilt;
    }

    private void HandleTowerBuilt(IBuildable tower, Vector3 position)
    {
        Tower _tower  = tower as Tower;

        currentGold -= _tower.TowerData.GoldsCost;
        Debug.Log("Reducing resources for the tower construction.");
    }

}
     