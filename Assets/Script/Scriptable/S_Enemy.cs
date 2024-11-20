using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]


public class S_Enemy : ScriptableObject
{
    public EnemyType type;
    public int goldValue;
    public float MoveSpeed;
}
public enum EnemyType
{
    Normal,
    Elite,
    Boss,
}