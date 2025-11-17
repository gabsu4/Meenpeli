using UnityEngine;

[CreateAssetMenu(fileName = "GunItemData", menuName = "Scriptable Objects/GunItemData")]
public class GunItemData : ItemData
{
    public int maxClipSize = 10;
    public int maxTotalAmmo = 100;
    public float reloadTime = 1.5f;
    public GameObject bulletPrefab;
}