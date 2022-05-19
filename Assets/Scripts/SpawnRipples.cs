using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpawnRipples : MonoBehaviour
{
    public GameObject shieldRipples;
    private VisualEffect shieldRipplesVfx;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            var ripples = Instantiate(shieldRipples, transform) as GameObject;
            shieldRipplesVfx = shieldRipples.GetComponent<VisualEffect>();
            shieldRipplesVfx.SetVector3("CollisionCenter", collision.contacts[0].point);

            // destroy in short delay
            Destroy(ripples, 2);
        }
    }
}
