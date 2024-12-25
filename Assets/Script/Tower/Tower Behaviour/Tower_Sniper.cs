using UnityEngine;
public class Tower_Sniper : global::Tower
{
    protected override void Fire(GameObject enemyToKill)
    {
        print($"{gameObject.name} is going to be fired");
    }
}
