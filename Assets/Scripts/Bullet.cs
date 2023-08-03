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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!(collision.gameObject.CompareTag("Player")))
        {
            GameObject affectedGameObject = collision.gameObject;
            affectedGameObject.SendMessage("GiveDamage", 10);
            int scoreToAdd = Random.Range(5, 11);
            GameManager.Instance.AddScore(scoreToAdd);
            //changes the color when the bullet hits the object
            SpriteRenderer spriteRenderer = affectedGameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.white;
            Debug.Log(spriteRenderer.color);



        } else if (collision.gameObject.CompareTag("GameBoundary"))
        {
            Destroy(this.gameObject);
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!(collision.gameObject.CompareTag("Player"))) 
        {
           GameObject affectedGameObject = collision.gameObject;

           //changes the color when the bullet hits the object
           SpriteRenderer spriteRenderer = affectedGameObject.GetComponent<SpriteRenderer>();
           spriteRenderer.color = new Color32(166, 7, 7, 255);
            Destroy(this.gameObject);
            Debug.Log(spriteRenderer.color);
        }
    
    }
}
