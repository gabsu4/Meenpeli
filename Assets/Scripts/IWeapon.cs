using System;

public interface IWeapon
{
    int currentClip { get; }
    int currentAmmo { get; }
    
    event Action<int, int> OnAmmoChanged;
    
    void ForceRegister();
}
