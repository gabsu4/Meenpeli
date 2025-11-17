using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    public TMP_Text ammoText;
    public PlayerAimAndShoot gun;

    private void Update()
    {
        if (gun.enabled)
        {
            ammoText.text = gun.GetClip() + " / " + gun.GetTotal();
        }
        else
        {
            ammoText.text = "";
        }
    }
}
