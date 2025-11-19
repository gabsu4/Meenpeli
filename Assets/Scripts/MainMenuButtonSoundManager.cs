using UnityEngine;

public class MainMenuButtonSoundManager : MonoBehaviour
{
    public static MainMenuButtonSoundManager Instance;
    public AudioSource audioSource;
    public AudioClip defaultClickSound;
    public AudioClip defaultHoverSound;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }

    public void PlayButtonHoverSound()
    {
        PlaySound(defaultHoverSound, 0.8f);
    }

    public void PlayButtonClickSound()
    {
        PlaySound(defaultClickSound, 1f);
    }
}
