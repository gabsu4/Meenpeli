using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private float moveSpeed;
    private float distanceToTarget = 1f;

    // Update is called once per frame
    private void Update()
    {
        Vector3 moveDirNormalized = (target.position - transform.position).normalized;
        transform.position += moveDirNormalized * moveSpeed;
        if (Vector3.Distance(transform.position, target.position) < distanceToTarget)
        {
            Destroy(gameObject);
        }
    }
    public void InitializedProjectile(Transform target, float moveSpeed)
    {
        this.target = target;
        this.moveSpeed = moveSpeed;
    }
}
