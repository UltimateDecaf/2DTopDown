using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    private int minScore = 15;
    private int maxScore = 30;
    private ulong ownerClientId;
    private PlayerScore playerScore;
    
    public void SetOwner(ulong ownerClientId, PlayerScore playerScore)
    {
        this.ownerClientId = ownerClientId;
        this.playerScore = playerScore;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody == null) { return; }

       
        if(collision.attachedRigidbody.TryGetComponent<NetworkObject>(out NetworkObject networkObject))
        {
            if (ownerClientId == networkObject.OwnerClientId && networkObject.gameObject.CompareTag("Player"))
            { 
                return;
            }

            if (networkObject.gameObject.CompareTag("Player"))
            {
                return;
            }
        }

        if(collision.attachedRigidbody.TryGetComponent<Health>(out Health health))
        { 
            health.Damage(damage);
            playerScore.ModifyPlayerScore(Random.Range(minScore, maxScore));
        }
    }
}
