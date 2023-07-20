using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    private GameObject player;
    private Rigidbody2D rigidbody;
    private Vector2 currentPlayerPosition;


    // Start is called before the first frame update
    void Awake()
    {
        currentPlayerPosition = Vector2.zero;
        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentPlayerPosition = GetCurrentPlayerPosition();
        MoveEnemy();
        RotateToPlayer();
        Debug.Log("Player now at x: " + currentPlayerPosition.x + " y: " + currentPlayerPosition.y);
    }

    Vector2 GetCurrentPlayerPosition() 
    { 
        return new Vector2(player.transform.position.x, player.transform.position.y);
    }

    private void MoveEnemy()
    {
        Vector2 moveVector = new Vector2(currentPlayerPosition.x - transform.position.x, currentPlayerPosition.y - transform.position.y);
        rigidbody.velocity = moveVector * speed;
    }

    private void RotateToPlayer()
    {
        Vector2 playerPos = GetCurrentPlayerPosition();
        playerPos.x = playerPos.x - transform.position.x;
        playerPos.y = playerPos.y  - transform.position.y;
        float angleToRotate = (Mathf.Atan2(playerPos.y , playerPos.x) * Mathf.Rad2Deg);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToRotate));
    }


}
