using UnityEngine;
public class TowerBehaviourSniper : global::Tower
{
    protected override void Fire(GameObject enemyToKill)
    {
        print($"{gameObject.name} is going to be fired");
    }
}
