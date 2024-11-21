using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "S_Tower", menuName = "Scriptable Objects/S_Tower")]
public class S_Tower : ScriptableObject
{
    [Header("Tower Core")]
    public TowerType Type;
    public AreaEffect ZoneEffect;
    public GameObject Prefab;
    public GameObject PreviewPrefab;

    [Header("Tower Stats")]
    public float Damage;
    public float FireRate;
    public int GoldsCost;
    public Vector2 Size;

    public enum TowerType
    {
        Canon,
        ChemicalWeapon,
        sniper,
        MachineGun,
        Tesla,
        FlameThrower,

    }
    [System.Serializable]
    public struct AreaEffect
    {
        public float EffectRadius;
        public int MaxEnemyChain;

    }
}
