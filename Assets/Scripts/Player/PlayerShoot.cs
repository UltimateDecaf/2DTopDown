using System.Collections;
using System.Collections.Generic;
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
    // Start is called before the first frame update
    void Start()
    {
        fireAction = playerInput.FindActionMap("Player").FindAction("Fire");
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
        //PREVIOUS IMPLEMENTATION)
        /* GameObject bullet = Instantiate(bulletPrefab, pointerOffset.position, pointerOffset.rotation);
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
        rigidbody.velocity = speed * transform.up * Time.deltaTime; */
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(pointerOffset.position.x, pointerOffset.position.y), Vector2.up);
        if(hit.collider != null)
        {
            Destroy(hit.collider.gameObject);
        }

    }

}
