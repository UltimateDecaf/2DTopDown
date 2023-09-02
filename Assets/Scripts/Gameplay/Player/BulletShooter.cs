using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletShooter : NetworkBehaviour
{

    //bullets do not render properly on the host's side!!!

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
    private float previousFireTime;

    public override void OnNetworkSpawn()
    {
        if (IsOwner) 
        { 
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
        if (IsOwner && shouldFire) 
        {
            if (Time.time > (1 / fireRate) + previousFireTime) 
            {
                FireServerRpc(bulletSpawnPoint.position, bulletSpawnPoint.up);

                SpawnDummyBullet(bulletSpawnPoint.position, bulletSpawnPoint.up);

                previousFireTime = Time.time;
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
}
