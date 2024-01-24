using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created by Lari Basangov

//This script sends massage to register damage to the player upon collision with them
public class EnemyDamage : MonoBehaviour
{
    [SerializeField] int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject gameObject = collision.gameObject;
            gameObject.SendMessage("GiveDamage", damage);
        }
    }
}
