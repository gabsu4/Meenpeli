using UnityEngine;

public class MainMenuButtonSoundManager : MonoBehaviour
{
    public static MainMenuButtonSoundManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position);
    }

    public void PlayButtonHoverSound(Vector3 position)
    {
        
    }

    public void PlayButtonClickSound(Vector3 position)
    {
        
    }
}
