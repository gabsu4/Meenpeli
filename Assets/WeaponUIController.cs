using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class WeaponUIController : MonoBehaviour
{
   public int currentClip, maxClipSize = 10, currentAmmo, maxAmmoSize = 100;

   public TMP_Text ammoTextTMP;
   public Slider reloadSlider;

   void Start()
   {
        UpdateUI();
   }

   public void Reload()
   {
        int reloadAmount = maxClipSize - currentClip;
        reloadAmount = (currentAmmo - reloadAmount) >= 0 ? reloadAmount : currentAmmo;
        currentClip += reloadAmount;
        currentAmmo -= reloadAmount;

        UpdateUI();
   }
   public void AddAmmo(int ammoAmount)
   {
    currentAmmo += ammoAmount;
    if(currentAmmo > maxAmmoSize)
    {
        currentAmmo = maxAmmoSize;
    }
    UpdateUI();
   }
   void UpdateUI()
   {
        if(ammoTextTMP != null)
            ammoTextTMP.text = currentClip + " / " + currentAmmo;
        if(reloadSlider != null)
            reloadSlider.value = (float)currentClip / maxClipSize;
   }
}