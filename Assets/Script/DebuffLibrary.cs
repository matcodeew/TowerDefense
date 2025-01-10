using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffLibrary : MonoBehaviour
{
    public static DebuffLibrary Instance;

    [System.Serializable]
    public enum DebuffType
    {
        Fire,
        Slow,
    }
    [System.Serializable]
    public struct DebuffDuration
    {
        public DebuffType type;
        public float duration;
    }
    [SerializeField] private List<DebuffDuration> AllDOT = new();

    public Dictionary<DebuffType, DebuffDuration> debuffs = new();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < AllDOT.Count; i++)
        {
            debuffs.Add(AllDOT[i].type, AllDOT[i]);
        }
    }
}
