using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSoundAndMovement : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private AudioClip pressedSound;
    [SerializeField] private AudioClip highlightSound;
    [SerializeField] private float hoverEffectScale = 0.06f;

    private Vector3 originalScale;
    private bool isPointerDown = false;
    private bool isPointerInside = false;
    private Button buttonComponent;

    void Awake()
    {
        buttonComponent = GetComponent<Button>();
    }

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (buttonComponent.IsInteractable())
            PlayButtonPressedSound();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonComponent.IsInteractable())
        {
            transform.localScale = originalScale * (1f + hoverEffectScale);
            PlayButtonHighlightSound();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonComponent.IsInteractable() && !isPointerDown)
        {
                transform.localScale = originalScale;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (buttonComponent.IsInteractable())
        {
            isPointerDown = true;
            PlayButtonPressedSound();
            transform.localScale = originalScale * (1f - hoverEffectScale / 2f);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (buttonComponent.IsInteractable())
        {
            isPointerDown = false;
            transform.localScale = originalScale;
        }
    }

    private void PlayButtonHighlightSound()
    {
        if (highlightSound != null)
        {
            MainMenuButtonSoundManager.Instance.PlaySound(highlightSound, 0.8f);
        }
        else
        {
            MainMenuButtonSoundManager.Instance.PlayButtonHoverSound();
        }
    }

    private void PlayButtonPressedSound()
    {
        if (pressedSound != null)
        {
            MainMenuButtonSoundManager.Instance.PlaySound(pressedSound, 1f);
        }
        else
        {
            MainMenuButtonSoundManager.Instance.PlayButtonClickSound();
        }
    }
}