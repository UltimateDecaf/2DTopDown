using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : NetworkBehaviour
{
 
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform playerLegs;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float speed;

    private Vector2 lastMoveInput;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) {  return; }

        inputReader.MoveEvent += HandleMove;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) { return; }
        inputReader.MoveEvent -= HandleMove;
    }

    private void Update()
    {
        if (!IsOwner) { return; }

      float targetAngle = Mathf.Atan2(lastMoveInput.y, lastMoveInput.x) * Mathf.Rad2Deg - 90f;
      playerLegs.eulerAngles = new Vector3(0f, 0f, targetAngle);
    }

    private void FixedUpdate()
    {


      if(!IsOwner) { return; }
       rb.velocity = speed * lastMoveInput;

    }

    public void HandleMove(Vector2 moveInput)
    {
        lastMoveInput = moveInput;
    }
}
