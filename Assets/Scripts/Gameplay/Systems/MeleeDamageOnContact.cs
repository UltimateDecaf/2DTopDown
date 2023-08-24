using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MeleeDamageOnContact : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody != null && collision.gameObject.tag.Equals("Player"))
        { 
            if (collision.rigidbody.TryGetComponent<Health>(out Health health))
            {
                health.Damage(damage);
            }
        }

    }
}
