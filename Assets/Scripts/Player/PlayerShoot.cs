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
    // Start is called before the first frame update
    void Start()
    {
        fireAction = playerInput.FindActionMap("Player").FindAction("Fire");
        rigidbody = GetComponent<Rigidbody2D>();
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
        Instantiate(bullet, this.transform.position, Quaternion.identity, this.transform);
        rigidbody.velocity = new Vector2(transform.position.x * speed, transform.position.y * speed);

    }
}
