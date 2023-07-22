using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health;
    private int maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveDamage(int damage)
    {
        if(health - damage > 0) 
        {
            health -= damage;
        }
        else
        {
            health = 0;
        }
 
    }

    public void Heal(int heal)
    {
        if (health + heal < maxHealth) 
        {
            health += heal;
        }
        else
        {
            health = maxHealth;
        }
    }
}
