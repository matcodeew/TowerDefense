using UnityEngine;

public interface IShootable
{
    public void Fire(GameObject enemyTarget);
    public void StartVfx(ParticleSystem VfxToUse);
    public void StartSfx(GameObject SoundToUse);
}
