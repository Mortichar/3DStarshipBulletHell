using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private new Rigidbody rigidbody;

    [Min(1)]
    public float movementForce = 10f;

    public float torque = 10f;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        gameObject.tag = "Player";
    }

    // Update is called once per frame
    void Update()
    {
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");


        if(moveVertical != 0)
        {
            rigidbody.AddForce(transform.up * movementForce * moveVertical);
        }

        if(moveHorizontal != 0)
        {
            var angle = Vector3.Cross(transform.up, transform.right);
            rigidbody.AddTorque(angle * torque * moveHorizontal);
        }

        
    }
}
