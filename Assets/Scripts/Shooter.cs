using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject projectileprefab;
    [SerializeField] private Transform target;

    [SerializeField] private float shootrate;
    [SerializeField] private float projectileMovespeed;
    private float shootTimer;
    private void Update()
    {
        shootTimer -= Time.deltaTime;
        if(shootTimer <= 0)
        {
            shootTimer = shootrate;
            Projectile projectile = Instantiate(projectileprefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
            projectile.InitializedProjectile(target, projectileMovespeed);
        }
    }
}
