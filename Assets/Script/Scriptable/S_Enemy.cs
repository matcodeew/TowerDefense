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
    public float CurrentLife;
    public float MaxLife;
    public float Damage;
}
public enum EnemyType
{
    Normal,
    Elite,
    Boss,
}