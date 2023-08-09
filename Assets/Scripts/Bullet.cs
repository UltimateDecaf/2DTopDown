using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (!(collision.gameObject.CompareTag("Player")))
        {
            GameObject affectedGameObject = collision.gameObject;
            affectedGameObject.SendMessage("GiveDamage", 10);
            int scoreToAdd = Random.Range(5, 11);
            GameManager.Instance.AddScore(scoreToAdd);
            Destroy(this.gameObject);
        } 

    }
}
