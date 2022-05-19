using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    InputWrapper input;

    private new Rigidbody rigidbody;
    private PlayerInput playerInput;

    [Min(1)]
    public float movementForce = 10f;

    public float torque = 10f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Player";
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        input = new();
    }

    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }


    public void Move(Vector2 movement)
    {
        rigidbody.AddForce(transform.up * movementForce * movement.y);
        var angle = Vector3.Cross(transform.up, transform.right);
        rigidbody.AddTorque(angle * torque * movement.x);

    }

    // Update is called once per frame
    void Update()
    {
        Move(input.Player.Move.ReadValue<Vector2>());
    }
}
