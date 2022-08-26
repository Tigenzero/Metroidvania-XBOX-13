using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttackController : MonoBehaviour
{

    public int timeAlive;

    public int damageAmount = 1;

    private void start() {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyHealthInterface>().DamageEnemy(damageAmount);
        }
        // if (impactEffect != null)
        // {
        //     Instantiate(impactEffect, transform.position, Quaternion.identity);
        // }

        AudioManager.instance.PlaySFXAdjusted(3);

        gameObject.SetActive(false);
        
        
    }
}
