using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Character_Controller controller;

    public int playerHealth = 3;
    public int numOfHearts = 3;
    public float damageImmunityTime = 1f;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    float damageImmuneTimeStamp = 0f;

    public Animator player_animation;
    public SpriteRenderer player_SpriteRenderer;

    // Blinking Animation variables
    public float spriteBlinkingTimer = 0.0f;
    public float spriteBlinkingMiniDuration = 0.2f;
    public float spriteBlinkingTotalTimer = 0.0f;
    public float spriteBlinkingTotalDuration = 1.0f;
    public bool startBlinking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startBlinking)
            SpriteBlinkingEffect();

        if (playerHealth > numOfHearts)
            playerHealth = numOfHearts;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerHealth)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
            if (i < numOfHearts)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
        if (playerHealth <= 0)
        {
            controller.GameOver();
        }
    }

    public void TakeDamage(int damage)
    {
        if (damageImmuneTimeStamp < Time.time)
        {
            playerHealth -= damage;
            Debug.Log(damage + " damage taken");
            damageImmuneTimeStamp = Time.time + damageImmunityTime;
            startBlinking = true;
            controller.DamageKnockback();
        }
    }

    private void SpriteBlinkingEffect()
    {
        spriteBlinkingTotalTimer += Time.deltaTime;
        if (spriteBlinkingTotalTimer >= spriteBlinkingTotalDuration)
        {
            startBlinking = false;
            spriteBlinkingTotalTimer = 0.0f;
            Debug.Log("test");
            player_SpriteRenderer.enabled = true;   // according to 
                                                                             //your sprite
            return;
        }

        spriteBlinkingTimer += Time.deltaTime;
        if (spriteBlinkingTimer >= spriteBlinkingMiniDuration)
        {
            spriteBlinkingTimer = 0.0f;
            if (player_SpriteRenderer.enabled == true)
            {
                player_SpriteRenderer.enabled = false;  //make changes
            }
            else
            {
                player_SpriteRenderer.enabled = true;   //make changes
            }
        }
    }
}
