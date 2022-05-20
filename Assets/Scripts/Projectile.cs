using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Projectile : MonoBehaviour
{
    public GameObject spawnOnCollision;
    // Start is called before the first frame update

    public void OnCollisionEnter(Collision collision)
    {
        // get the angle of reflection
        var reflectionVector = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
        if (spawnOnCollision != null)
        {
            var rotation = Quaternion.LookRotation(Vector3.forward, reflectionVector);
            var onCollisionObj = Instantiate(spawnOnCollision, collision.contacts[0].point, rotation) as GameObject;

            Destroy(onCollisionObj, 2);
        }

        // destroy the projectile on impact
        Destroy(gameObject);
    }
}
