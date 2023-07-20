using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float speed;

    public InputActionAsset playerInput;
    public InputAction fireAction;

    private Rigidbody2D rigidbody;
    private Vector3 mousePos;
    private Vector3 playerPosition;
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        fireAction = playerInput.FindActionMap("Player").FindAction("Fire");
        rigidbody = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (fireAction.triggered)
        {
            Fire();
        }
    }

    public void Fire()
    {
        float angleToRotateBullet = CalculateAngleToRotate();
        Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, angleToRotateBullet - 90)), transform);
        rigidbody.velocity = new Vector2(transform.position.x * speed, transform.position.y * speed);

    }

    private float CalculateAngleToRotate()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 0;

        playerPosition = mainCamera.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - playerPosition.x;
        mousePos.y = mousePos.y - playerPosition.y;

        float angleToRotate = (Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg) - 90;
        return angleToRotate;
    }
}
