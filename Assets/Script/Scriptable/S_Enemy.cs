using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]


public class S_Enemy : ScriptableObject
{
    [Header("Enemy Core")]
    public EnemyType type;
    public GameObject Prefab;

    [Header("Enemy stats")]
    public int goldValue;
    public float MoveSpeed;
    public float MaxLife;
    public int Damage;

    [Header("Stat Multiplicator")]
    public float MoveSpeedMultiplicator;
    public float MaxLifeMultiplicator;
    public int DamageMultiplicator;
}
public enum EnemyType
{
    Normal,
    Elite,
    Boss,
}