using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health;
    private int maxHealth;
    private GameObject selfReference;

    //temporary fix before I get UI behavior to be a separate component
    [SerializeField] private TextMeshProUGUI healthField;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        selfReference = this.gameObject;
        Debug.Log(selfReference.name);
        //temporary fix before I get UI behavior to be a separate component
        UpdateUIPlayerHealth();
        
        

    }

    // Update is called once per frame
    void Update()
    {
        //temporary fix before I get UI behavior to be a separate component
        UpdateUIPlayerHealth();
    }

    public void GiveDamage(int damage)
    {
        if(health - damage > 0) 
        {
            health -= damage;
        }
        else
        {
            health = 0;
        }

        if(health == 0)
        {
            Destroy(selfReference);
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

    public int GetHealth()
    {
        return health;
    }

    public void UpdateUIPlayerHealth()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            healthField.text = "Health: " + health;
        }
    }
}
