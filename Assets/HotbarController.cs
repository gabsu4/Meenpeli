using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HotbarController : MonoBehaviour
{
    public Image[] slots;
    public TMP_Text[] countTexts;

    private int selectedIndex = 0;

    void Update()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;
        if (keyboard != null)
        {
            if (keyboard.digit1Key.wasPressedThisFrame) SelectSlot(0);
            if (keyboard.digit2Key.wasPressedThisFrame) SelectSlot(1);
            if (keyboard.digit3Key.wasPressedThisFrame) SelectSlot(2);
            if (keyboard.digit4Key.wasPressedThisFrame) SelectSlot(3);
            if (keyboard.digit5Key.wasPressedThisFrame) SelectSlot(4);
        }
        if (mouse != null)
        {
            float scroll = mouse.scroll.ReadValue().y;
            if (scroll > 0f)
                SelectNextSlot();
            else if (scroll < 0f)
                SelectPreviousSlot();
        }
    }
    void SelectNextSlot()
    {
        int nextIndex = (selectedIndex + 1) % slots.Length;
        SelectSlot(nextIndex);
    }
    void SelectPreviousSlot()
    {
        int prevIndex = (selectedIndex - 1 + slots.Length) % slots.Length;
        SelectSlot(prevIndex);
    }
    void SelectSlot(int index)
    {
        selectedIndex = index;
        for (int i = 0; i < slots.Length; i++)
        {
            if (i == selectedIndex)
                slots[i].color = new Color(0f, 0f, 0f, 0.8f);
            else
                slots[i].color = new Color(26f/255f, 26f/255f, 26f/255f, 0.4f);
        }
    }
}