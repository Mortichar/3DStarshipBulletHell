using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurret : MonoBehaviour
{
    // Projectile for the turret to shoot
    public GameObject projectile;

    // Transform to spawn the bullet from
    public Transform bulletSpawnPoint;

    public float projectileSpeed = 15;
    
    // Range in engine units (meters)
    public float range = 30f;

    // Time between shots
    public float fireRate = 1f;

    // time since last shot
    private float cooldown = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;

        var nearestTarget = FindNearestTarget();
        
        if(nearestTarget != null)
        {
            // hacky way to look straight at the target... all of my models start looking straight up lol
            var difference = nearestTarget.position - transform.position;
            transform.up = new Vector3(difference.x, difference.y, 0);

            if(cooldown <= 0f)
            {
                Shoot(nearestTarget);
                cooldown = 1f / fireRate;
            }
        }
        
        
    }

    void Shoot(Transform target)
    {
        GameObject bullet = (GameObject)Instantiate(projectile, bulletSpawnPoint.position, transform.rotation);

        var bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = transform.up * projectileSpeed;

        // Physics.IgnoreCollision(bullet.GetComponent<Collider>(), transform.parent.GetComponent<Collider>());

        // Destroy the bullet after 3 seconds
        Destroy(bullet, 3f);
        
    }

    Transform FindNearestTarget()
    {
        // get everything near us . . .
        var rayHits = Physics.OverlapSphere(transform.position, range);

        Transform nearestEnemy = null;


        // use squared distances to avoid expensive sqrt
        float closestDistanceSquared = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(var hit in rayHits)
        {
            if (hit.GetComponent<Collider>().gameObject.tag == "Player")
            {
                var target = hit.GetComponent<Collider>().gameObject.transform;
                var heading = target.position - currentPosition;
                var distanceSquared = heading.sqrMagnitude;
                if (distanceSquared < closestDistanceSquared)
                {
                    closestDistanceSquared = distanceSquared;
                    nearestEnemy = target;
                }
            }
        }
        return nearestEnemy;
    }
}
