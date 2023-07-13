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
    Quaternion rotation;

    public InputActionAsset playerInput;
    public InputAction moveAction;
    public InputAction lookAction;
    public Vector3 mousePos;
    // Start is called before the first frame update
    private void Awake()
    {
        moveAction = playerInput.FindActionMap("Player").FindAction("Move");
        lookAction = playerInput.FindActionMap("Player").FindAction("Look");
        rigidbody = GetComponent<Rigidbody2D>();
     
    }

    private void FixedUpdate()
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        rigidbody.velocity = moveVector * speed;
        
       
    }

 /*  private void RotateToCursorPosition()
    {
        mousePos = Input.mousePosition;
        rotation = Quaternion.LookRotation(mousePos, Vector3.up);
        transform.rotation = rotation;

    }
 */
}
