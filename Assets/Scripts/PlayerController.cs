using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D theRB;

    public float moveSpeed;
    public float jumpForce;

    public Transform groundPoint;
    private bool isOnGround;
    private bool canDoubleJump;
    public LayerMask whatIsGround;

    public Animator anim;

    public float dashSpeed, dashTime;

    public SpriteRenderer theSR, afterImage;

    public float afterImageLifetime, timeBetweenAfterImages;

    private float afterImageCounter;

    public Color afterImageColor;

    private float dashCounter;
    
    public float waitAfterDashing;
    
    private float dashRechargeCounter;


    private PlayerAbilityTracker abilities;
    
    

    // Start is called before the first frame update
    void Start()
    {
        abilities = GetComponent<PlayerAbilityTracker>();
    }

    // Update is called once per frame
    void Update()
    {
    
    	if (dashRechargeCounter > 0) {
    		dashRechargeCounter -= Time.deltaTime;
    	} else {
    		if (Input.GetButtonDown("Fire2") && abilities.canDash) {
            dashCounter = dashTime;

            ShowAfterImage();
        	}
    	}

        
        if (dashCounter > 0) {
            dashCounter = dashCounter - Time.deltaTime;

            theRB.velocity = new Vector2(dashSpeed * transform.localScale.x, theRB.velocity.y);

            afterImageCounter -= Time.deltaTime;
            if (afterImageCounter <= 0) {
                ShowAfterImage();
            }
            
            dashRechargeCounter = waitAfterDashing;
        }
        else {
            moveCharacter(Input.GetAxisRaw("Horizontal"));
        }
        


        // Am I on the Ground?
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, .2f, whatIsGround);


        // Jumping
        if(Input.GetButtonDown("Jump") && (isOnGround || (canDoubleJump && abilities.canDoubleJump))) {
            if (isOnGround) {
                canDoubleJump = true;
            } else {
                canDoubleJump = false;
                anim.SetTrigger("doubleJump");
            }
            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
        }







        // Set Animation params
        anim.SetBool("isOnGround", isOnGround);
        anim.SetFloat("speed", Mathf.Abs( theRB.velocity.x));


    }

    void moveCharacter(float Horizontal) {
        // Move Sideways
        theRB.velocity = new Vector2(Horizontal * moveSpeed,theRB.velocity.y);

        // Handle direction Change
        if (theRB.velocity.x < 0) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } else if (theRB.velocity.x > 0) {
            transform.localScale = Vector3.one;
        }
    }


    public void ShowAfterImage() {
        SpriteRenderer image = Instantiate(afterImage, transform.position, transform.rotation);
        image.sprite = theSR.sprite;
        image.transform.localScale = transform.localScale;
        image.color = afterImageColor;

        Destroy(image.gameObject, afterImageLifetime);

        afterImageCounter = timeBetweenAfterImages;
    }
}
