using UnityEngine;

public class TowerFamily : MonoBehaviour
{
    [System.Serializable]
    public struct TowerOnFamily
    {
        public S_Tower FirstTower;
        public S_Tower SecondTower;
    }

    public TowerOnFamily allTowerOnFamily;
    public S_Tower TowerToInstantiate;
}
