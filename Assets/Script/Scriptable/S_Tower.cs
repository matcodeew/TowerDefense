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
    public Vector3 PosOnMap;

    [Header("VFX")]
    public GameObject HitVfx;
    public GameObject TowerFireVfx;

    [Header("Tower Stats")]
    public float Damage;
    public float FireRate;
    public float FireRange;
    public int GoldsCost;
    public Vector2 Size;

    [Header("UpgradeValue")]
    public float UpgradeDamage;
    public float UpgradeFireRate;
    public float UpgradeFireRange;

    public Tower.TowerStat GetTowerStats()
    {
        return new Tower.TowerStat
        {
            FireRate = FireRate,
            FireRange = FireRange,
            Damage = Damage,
        };
    }

    public Building.BuildingStat GetBuildingStats()
    {
        return new Building.BuildingStat
        {
            GoldsCost = GoldsCost,
        };
    }
    public enum TowerType
    {
        Canon,
        ChemicalWeapon,
        sniper,
        Gatling,
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
