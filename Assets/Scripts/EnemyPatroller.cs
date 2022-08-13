using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
    public Transform[] patrolPoints;

    public Transform groundPoint;
    
    public LayerMask whatIsGround;

    private int currentPoint;

    public float moveSpeed, waitAtPoints;
    private float waitCounter;

    public float jumpForce;

    public Rigidbody2D theRB;

    public Animator anim;

    private bool isOnGround;

    // Start is called before the first frame update
    void Start()
    {
        waitCounter = waitAtPoints;

        foreach(Transform pPoint in patrolPoints) {
            pPoint.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(transform.position.x - patrolPoints[currentPoint].position.x) > .2f) {
            if(transform.position.x < patrolPoints[currentPoint].position.x)
            {
                theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            } else {
                theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);
                transform.localScale = Vector3.one;
            }

            // Am I on the Ground?
            isOnGround = Physics2D.OverlapCircle(groundPoint.position, .2f, whatIsGround);


            if (transform.position.y < patrolPoints[currentPoint].position.y - .5f  && isOnGround) {
                theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
            }
        } else {
            theRB.velocity = new Vector2(0f, theRB.velocity.y);
            waitCounter -= Time.deltaTime;
            if(waitCounter <= 0) {
                waitCounter = waitAtPoints;
                currentPoint++;
                currentPoint = currentPoint % (patrolPoints.Length); // hopefully this is modulo....
            }
        }

        anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
    }
}
