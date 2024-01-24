using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Based on Nathan Farrer's 'ProjectileLauncher.cs' script: https://gitlab.com/GameDevTV/unity-multiplayer/unity-multiplayer/-/blob/main/Assets/Scripts/Core/Player/ProjectileLauncher.cs?ref_type=heads 

/*
 Lari Basangov's addition:
- Communicating with 'DamageOnContact.cs' script to register the addition to the player score
 */

//This script is design to run the bullet's logic on the server side, while rendering the image on the client side.
//It is achieved by RPCs and using a separate prefab for a "server" bullet (no rendering), and a "client" bullet prefas.
public class BulletShooter : NetworkBehaviour
{

    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject serverBulletPrefab;
    [SerializeField] private GameObject clientBulletPrefab;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private PlayerScore playerScore;

    [Header("Gameplay Tweaks")]
    [SerializeField] private float bulletSpeed = 1f;
    [SerializeField] private float fireRate;

    private bool shouldFire;
    private bool isDead;
    private float previousFireTime;

    public override void OnNetworkSpawn()
    {
        if (IsOwner) 
        {
            isDead = false;
            inputReader.FireEvent += HandleFire; 
        }
        
    }

    public override void OnNetworkDespawn()
    {
        if (IsOwner) 
        {
            inputReader.FireEvent -= HandleFire; 
        }
        
    }

    private void Update()
    {
        if (IsOwner && shouldFire && !isDead) 
        {
            if (Time.time > (1 / fireRate) + previousFireTime) 
            {
                FireServerRpc(bulletSpawnPoint.position, bulletSpawnPoint.up);

                SpawnDummyBullet(bulletSpawnPoint.position, bulletSpawnPoint.up);

                previousFireTime = Time.time;

                Debug.Log(Time.time);
            }
        }

       

    }
    private void HandleFire(bool shouldFire)
    {
        this.shouldFire = shouldFire;
    }

    [ServerRpc]
    private void FireServerRpc(Vector3 spawnPosition, Vector3 direction)
    {
        GameObject bulletInstance = Instantiate(serverBulletPrefab, spawnPosition, Quaternion.identity);

        bulletInstance.transform.up = direction;

        Physics2D.IgnoreCollision(playerCollider, bulletInstance.GetComponent<Collider2D>());

        if(bulletInstance.TryGetComponent<DamageOnContact>(out DamageOnContact damageOnContact)){
            damageOnContact.SetOwner(OwnerClientId, playerScore);
        }
        
        if (bulletInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * bulletSpeed;
        }

        SpawnDummyBulletClientRpc(spawnPosition, direction);
    }

    [ClientRpc]
    private void SpawnDummyBulletClientRpc(Vector3 spawnPosition, Vector3 direction)
    {
        if (!IsOwner) 
        {
            SpawnDummyBullet(spawnPosition, direction);
        }

       
    }
    private void SpawnDummyBullet(Vector3 spawnPosition, Vector3 direction)
    {
        GameObject bulletInstance = Instantiate(clientBulletPrefab, spawnPosition, Quaternion.identity);

        bulletInstance.transform.up = direction;

        Physics2D.IgnoreCollision(playerCollider, bulletInstance.GetComponent<Collider2D>());   

        if(bulletInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * bulletSpeed;
        }
    }

    public void SetIsDead(bool isDead) { 
        this.isDead = isDead;
    }
}
