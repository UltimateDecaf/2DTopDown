using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI scoreUI;
    private int score;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScoreUI();    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!(collision.gameObject.CompareTag("Player")))
        {
            GameObject affectedGameObject = collision.gameObject;
            affectedGameObject.SendMessage("GiveDamage", 10);
            AddToScore(15);
            //changes the color when the bullet hits the object
            SpriteRenderer spriteRenderer = affectedGameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.white;
            Debug.Log(spriteRenderer.color);



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

    public void AddToScore(int scoreToAdd)
    {
        score += scoreToAdd;
        Debug.Log("Method Called - current score is " + score);
    }

    public void UpdateScoreUI()
    {
        scoreUI.text = "Score: " + score;
    }
}
