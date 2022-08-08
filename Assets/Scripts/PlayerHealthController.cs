using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    // when first activated, or reactivated
    // occurs before start function
    // Keeps gameObject consistent between scenes.
    private void Awake() {
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        
    }

    // [HideInInspector]
    public int currentHealth;
    public int maxHealth;

    // Reacting to Player Damage
    public float invincibilityLength;
    private float invincCounter;

    public float flashLength;
    private float flashCounter;

    public SpriteRenderer[] playerSprites;

    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (invincCounter > 0) {
            invincCounter -= Time.deltaTime;

            flashCounter -= Time.deltaTime;
            if (flashCounter < 0) {
                foreach (SpriteRenderer sr in playerSprites) {
                    sr.enabled = !sr.enabled;
                }
                flashCounter = flashLength;
            }

            if (invincCounter <= 0) {
                foreach (SpriteRenderer sr in playerSprites) {
                    sr.enabled = true;
                }
                flashCounter = 0f;
            }
        }
    }

    public void DamagePlayer(int damageAmount) {
        if (invincCounter <= 0){
            currentHealth -= damageAmount;

            // Death
            if(currentHealth <= 0 && !isDead) {
                currentHealth = 0;

                gameObject.GetComponent<HeroinePlayerController>().killPlayer();
                isDead = true;

                // gameObject.SetActive(false);
                
                RespawnController.instance.Respawn();

                AudioManager.instance.PlaySFX(8);
            }
            // Not dead? Temp Invincibility
            else {
                gameObject.GetComponent<HeroinePlayerController>().hurtPlayer();
                invincCounter = invincibilityLength;

                AudioManager.instance.PlaySFXAdjusted(11);
            }

            UIController.instance.UpdateHealth(currentHealth, maxHealth);
        }
    }

    public void FillHealth(){
        currentHealth = maxHealth;
        UIController.instance.UpdateHealth(currentHealth, maxHealth);
        isDead = false;
    }

    public void HealPlayer(int healAmount) {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }
}
