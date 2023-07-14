using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    [SerializeField] private float speed;
    private Vector2 smoothedMoveVector;
    private Vector2 currentVelocity;
    Quaternion rotation;

    public InputActionAsset playerInput;
    public InputAction moveAction;


    public Vector3 mousePos;
    private Camera mainCamera;
    private Vector3 objectPosition;
    // Start is called before the first frame update
    private void Awake()
    {
        moveAction = playerInput.FindActionMap("Player").FindAction("Move");


        rigidbody = GetComponent<Rigidbody2D>();
        rotation = Quaternion.identity;
        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
       MovePlayer();
       RotateToCursorPosition();

    }

   private void RotateToCursorPosition()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 0;

        objectPosition = mainCamera.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPosition.x;
        mousePos.y = mousePos.y - objectPosition.y; 

        float angleToRotate = (Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg) - 90;
        transform.rotation = Quaternion.Euler(new Vector3(0,0,angleToRotate));

    } 

    private void MovePlayer()
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        smoothedMoveVector = Vector2.SmoothDamp(smoothedMoveVector, moveVector, ref currentVelocity, 0.2f);
        rigidbody.velocity = smoothedMoveVector * speed;

    }

 
}
