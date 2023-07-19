using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speed;

    private Rigidbody2D rigidbody;
    private Vector2 currentPlayerPosition;


    // Start is called before the first frame update
    void Awake()
    {
        currentPlayerPosition = Vector2.zero;
        rigidbody = GetComponent<Rigidbody2D>();
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
        return player.transform.position;
    }

    private void MoveEnemy()
    {
        Vector2 moveVector = new Vector2(currentPlayerPosition.x, currentPlayerPosition.y);
        rigidbody.velocity = moveVector * speed;
    }

    private void RotateToPlayer()
    {

        float angleToRotate = (Mathf.Atan2(currentPlayerPosition.y, currentPlayerPosition.x) * Mathf.Rad2Deg) - 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToRotate));
    }


}
