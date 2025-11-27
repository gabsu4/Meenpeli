using UnityEngine;

public class AudioHelper : MonoBehaviour
{
    public static void PlayClip2D(AudioClip clip, Vector3 position, float volumeScale = 3f)
    {
        if (clip == null) return;

        GameObject audioObject = new GameObject("TempAudio");
        audioObject.transform.position = position;

        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.volume = volumeScale;
        audioSource.spatialBlend = 0f; 
        audioSource.playOnAwake = false;

        audioSource.Play();

        Object.Destroy(audioObject, clip.length + 0.1f); 
    }
}