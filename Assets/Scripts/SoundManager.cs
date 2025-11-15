using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }

    private AudioSource Source;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return; 
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        Source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip _sound)
    {
        Source.PlayOneShot(_sound);
    }

}
