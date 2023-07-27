using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float speed;
    [SerializeField] private Transform pointerOffset;

    public InputActionAsset playerInput;
    public InputAction fireAction;

    private Rigidbody2D rigidbody;
    private Vector3 mousePos;
    private Vector3 playerPosition;
    private Camera mainCamera;

    //mouse position for raycasting
    // Start is called before the first frame update
    void Start()
    {
        fireAction = playerInput.FindActionMap("Player").FindAction("Fire");
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
        //PREVIOUS IMPLEMENTATION
        /* GameObject bullet = Instantiate(bulletPrefab, pointerOffset.position, pointerOffset.rotation);
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
        rigidbody.velocity = speed * transform.up * Time.deltaTime; */
        Vector3 playerGunPosition = pointerOffset.position;
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = playerGunPosition.z;
        Vector3 shootDirection = mousePosition - playerGunPosition;
        shootDirection.Normalize();

        RaycastHit2D hit = Physics2D.Raycast(playerGunPosition, shootDirection, 1000f);
        Debug.DrawRay(playerGunPosition, shootDirection * 1000f, Color.red, 1f);
        if (hit.collider.CompareTag("Shootable"))
        {
            hit.collider.gameObject.SendMessage("GiveDamage", 5);
            Debug.Log("Damage received -5");
        
        }

    }

}
