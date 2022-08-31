using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour, EnemyHealthInterface
{
    public int totalHealth = 3;

    public GameObject deathEffect;

    public Animator anim;

    public void DamageEnemy(int damageAmount){
        totalHealth -= damageAmount;

        if (anim != null) {
            anim.SetTrigger("isHurt");
        }

        if(totalHealth <= 0) {
            if (deathEffect != null) {
                Instantiate(deathEffect, transform.position, transform.rotation);
            }

            Destroy(gameObject);

            AudioManager.instance.PlaySFX(4);
        }
    }
}
