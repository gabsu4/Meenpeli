using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float targetCameraSize = 8f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private GameObject bossGameObject;
    private float soundVolumeBoost = 0.5f;
    private bool isTriggered = false;
    public AudioClip Enter;

    void Start()
    {
        if (bossGameObject != null)
        {
            bossGameObject.SetActive(false);
        }
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            AudioHelper.PlayClip2D(Enter, transform.position, soundVolumeBoost);
            isTriggered = true;
            if(bossGameObject != null)
            {
                bossGameObject.SetActive(true);
            }
        }
    }

    void Update()
    {
        if(isTriggered && mainCamera != null)
        {
            mainCamera.orthographicSize = Mathf.Lerp(
                mainCamera.orthographicSize, 
                targetCameraSize, 
                Time.deltaTime * zoomSpeed
            );
        }
    }
}
