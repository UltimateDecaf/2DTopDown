using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    [SerializeField] private float speed;
    private Vector2 movementVector;

    public InputActionAsset playerInput;
    public InputAction moveAction;

    // Start is called before the first frame update
    private void Awake()
    {
        moveAction = playerInput.FindActionMap("Player").FindAction("Move");
        rigidbody = GetComponent<Rigidbody2D>();
     
    }

    private void FixedUpdate()
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        movementVector = moveVector.normalized;
        rigidbody.velocity = moveVector * speed;
    }

   /* private void OnEnable()
    {
        playerInput.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        playerInput.FindActionMap("Player").Disable();
    }
   */
}
