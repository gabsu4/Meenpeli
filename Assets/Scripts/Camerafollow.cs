using UnityEngine;

public class Camerafollow : MonoBehaviour
{
    public Transform target; 
    
    public float smoothSpeed = 0.125f; 
    public Vector3 offset = new Vector3(0f, 0f, -10f); 

    void LateUpdate()
    {
        if (target == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                target = playerObject.transform;
            }
            // If still null, stop here.
            if (target == null)
                return;
        }

        // 2. Perform smooth follow movement
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
