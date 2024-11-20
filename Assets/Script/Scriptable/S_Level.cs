using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/Level")]
public class S_Level : ScriptableObject
{
    public int WaveID;
    public List<EnemyOnMap> NumbOfEnemy = new();
    public int MaxWave;




    [System.Serializable]
    public struct EnemyOnMap
    {
        public S_Enemy Enemy;
        public int Quantity;
    }
}
