using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    Transform Player;

    [SerializeField]
    float AgroRange;

    [SerializeField]
    Transform CastPos;

    [SerializeField]
    Transform CastPosleft;

    [SerializeField]
    float MoveSpeed;

    Rigidbody2D Rb;

    bool isfacingleft;

    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float DistToPlayer = Vector2.Distance(transform.position, Player.position);
        Debug.Log("He is this far from me" + DistToPlayer);
        if (SightRange(AgroRange))
        {

            PursuePlayer();
        }
        else
        {
            StopPursuit();
        }
    }


    void PursuePlayer()
    {
        if (transform.position.x < Player.position.x)
            // If enemy is to the left of player move right
        {
            Rb.velocity = new Vector2(MoveSpeed, 0);
            transform.localScale = new Vector2(1, 1);
            isfacingleft = false;
        }
        else
        // If to the right of player move left
        {
            Rb.velocity = new Vector2(-MoveSpeed, 0);
            transform.localScale = new Vector2(-1, 1);
            isfacingleft = true;
        }
    }

    void StopPursuit()
    {
        Rb.velocity = new Vector2(0, 0);
    }

   bool SightRange(float Distance)
    {
        bool sight = false;
        float castDist = Distance;
        //Vector2 endPos = CastPos.position + Vector3.right * castDist;
        Vector2 endPosL = CastPosleft.position + Vector3.left * castDist;
       

        //RaycastHit2D hit = Physics2D.Linecast(CastPos.position, endPos, 1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D hitleft = Physics2D.Linecast(CastPosleft.position, endPosL, 1 << LayerMask.NameToLayer("Player"));
        if ( hitleft.collider !=null)
        {
            if ( hitleft.collider.gameObject.CompareTag("Player"))
            {
                // If the raycast hits the player chase otherwise do nothing
                sight = true;
            }
            else
            {
                sight = false;
                
            }
            Debug.DrawLine(CastPos.position, hitleft.point, Color.red);

        }
        else
        {
            Debug.DrawLine(CastPos.position, endPosL, Color.green);
        }
        Debug.DrawLine(CastPos.position, endPosL, Color.green);
        return sight;
    }
   

    
}
