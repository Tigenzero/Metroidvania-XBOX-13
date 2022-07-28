using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float timeToExplode = .5f;
    public GameObject explosion;

    // Destructable stuff
    public float blastRange;
    public LayerMask whatIsDestructible;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeToExplode -= Time.deltaTime;
        if (timeToExplode <= 0) {
            // is something assigned to explosion?
            if (explosion != null) {
                Instantiate(explosion, transform.position, transform.rotation);
            }

            Destroy(gameObject);

            Collider2D[] objectsToRemove = Physics2D.OverlapCircleAll(transform.position, blastRange, whatIsDestructible);

            if (objectsToRemove.Length > 0) {
                foreach (Collider2D item in objectsToRemove)
                {
                    Destroy(item.gameObject);
                }
            }

        }
    }
}
