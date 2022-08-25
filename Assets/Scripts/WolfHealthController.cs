using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfHealthController : MonoBehaviour, EnemyHealthInterface
{
    public int totalHealth = 3;

    public Animator anim;

    public GameObject deathEffect;

    public void DamageEnemy(int damageAmount){
        totalHealth -= damageAmount;

        if(totalHealth <= 0) {

            if (deathEffect != null) {
                Instantiate(deathEffect, transform.position, transform.rotation);
            }
            Destroy(gameObject);

            AudioManager.instance.PlaySFX(4);
        }
    }
}
