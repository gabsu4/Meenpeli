using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float targetCameraSize = 8f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private GameObject bossGameObject;
    private BossTransformation bossTransformer;
    private float soundVolumeBoost = 0.5f;
    private bool isTriggered = false;
    public AudioClip Enter;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        if (bossGameObject != null)
        {
            bossGameObject.SetActive(false);
            bossTransformer = bossGameObject.GetComponent<BossTransformation>();
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
            if(bossTransformer != null)
            {
                bossTransformer.PlayerEnteredArena();
            }
            GetComponent<Collider2D>().enabled = false;
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
