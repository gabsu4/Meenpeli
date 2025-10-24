using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HotbarController : MonoBehaviour
{
    public Image[] icons;
    public TMP_Text[] countTexts;

    private int selectedIndex = 0;

    void Update()
    {
        if (input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
        if (input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
    }
}
