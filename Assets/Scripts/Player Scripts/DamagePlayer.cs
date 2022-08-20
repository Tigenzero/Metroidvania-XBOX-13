using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{

    public int damageAmount = 1;
    // Did we touch?
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            DealDamage();
        }
    }

    // Did I hit player?
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player"){
            DealDamage();
        }
    }

    void DealDamage(){
        PlayerHealthController.instance.DamagePlayer(damageAmount);

    }
}