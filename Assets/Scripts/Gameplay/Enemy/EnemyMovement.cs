using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

//Created by Lari Basangov

//This script makes sure that each enemy follows the player
public class EnemyMovement : NetworkBehaviour
{
    [SerializeField] private float speed;

    private GameObject player;
    private Rigidbody2D rb;
    private Vector2 currentPlayerPosition;


    void Awake()
    {
        currentPlayerPosition = Vector2.zero;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player(Clone)");
    }


    void FixedUpdate()
    {
        currentPlayerPosition = GetCurrentPlayerPosition();
        MoveEnemy();
        RotateToPlayer();

    }

    Vector2 GetCurrentPlayerPosition() 
    { 
        return new Vector2(player.transform.position.x, player.transform.position.y);
    }

    private void MoveEnemy()
    {
        Vector2 moveVector = new Vector2(currentPlayerPosition.x - transform.position.x, currentPlayerPosition.y - transform.position.y);
        rb.velocity = moveVector * speed;
    }

    private void RotateToPlayer()
    {
        Vector2 playerPos = GetCurrentPlayerPosition();
        playerPos.x = playerPos.x - transform.position.x;
        playerPos.y = playerPos.y  - transform.position.y;
        float angleToRotate = (Mathf.Atan2(playerPos.y , playerPos.x) * Mathf.Rad2Deg);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToRotate - 90));
    }


}
