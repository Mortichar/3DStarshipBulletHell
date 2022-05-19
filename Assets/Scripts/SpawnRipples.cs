using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpawnRipples : MonoBehaviour
{
    public GameObject shieldRipples;
    public Transform shieldTransform;
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            // instantiate the ripples on the same transform as the shield
            var ripples = Instantiate(shieldRipples, shieldTransform) as GameObject;
            var shieldRipplesVfx = shieldRipples.GetComponent<VisualEffect>();
            shieldRipplesVfx.SetVector3("CollisionCenter", collision.contacts[0].point);

            // destroy in short delay
            Destroy(ripples, 2);
        }
    }
}
