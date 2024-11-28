using UnityEngine;

public interface IShootable
{
    public void Fire(GameObject enemyTarget);
    public void HitVfx(ParticleSystem VfxToUse);
    public void FireVfx(ParticleSystem VfxToUse);
    public void StartSfx(GameObject SoundToUse);
}
