using UnityEngine;
using TMPro;

public class AmmoDisplay : MonoBehaviour
{
    public TextMeshProUGUI clipAmmoText;

    public TextMeshProUGUI reserveAmmoText;

    [SerializeField]
    private IWeapon currentWeapon;

    public void SetCurrentWeapon(IWeapon newWeapon)
    {
        if (currentWeapon != null)
        {
            currentWeapon.OnAmmoChanged -= UpdateAmmoUI;
            SetUITexts(string.Empty, string.Empty);
        }

        currentWeapon = newWeapon;

        if (currentWeapon != null)
        {
            currentWeapon.OnAmmoChanged += UpdateAmmoUI;
        
            currentWeapon.ForceRegister(); 
        }
    }

    private void UpdateAmmoUI(int clip, int reserve)
    {
        Debug.Log($"AmmoDisplay: Vastaanotettu päivitys: {clip} / {reserve}. Päivitetään UI.");
        SetUITexts(clip.ToString(), "/ " + reserve.ToString());
    }

    private void SetUITexts(string clip, string reserve)
    {
        if (clipAmmoText != null)
        {
            clipAmmoText.text = clip;
        }

        if (reserveAmmoText != null)
        {
            reserveAmmoText.text = reserve;
        }
    }

    void OnDestroy()
    {
        if (currentWeapon != null)
        {
            currentWeapon.OnAmmoChanged -= UpdateAmmoUI;
        }
    }
}
