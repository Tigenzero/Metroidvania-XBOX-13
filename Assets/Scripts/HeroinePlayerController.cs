using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroinePlayerController : MonoBehaviour
{
    public Rigidbody2D theRB;

    public float moveSpeed;
    public float jumpForce;

    public Transform groundPoint;
    private bool isOnGround;
    private bool canDoubleJump;
    public LayerMask whatIsGround;

    public Animator anim;

    // attacking
    public Transform swordAttackPoint;
    public PlayerAttackController attackController;
    public GameObject cutBlob;
    public float attackTime;
    private float attackCounter;

    public float dashSpeed, dashTime;

    public SpriteRenderer theSR, afterImage;

    public float afterImageLifetime, timeBetweenAfterImages;

    private float afterImageCounter;

    public Color afterImageColor;

    private float dashCounter;

    public float waitAfterDashing;

    private float dashRechargeCounter;

    public float dashRange;

    public LayerMask whatIsDashDestructible;

    // private HeroineAbilityTracker abilities;

    private PlayerAbilityTracker abilities;

    //Player hurt settings

    public float hurtTime;

    private float hurtCounter;

    private bool isDead;

    // Shooting

    // public BulletController shotToFire;
    // public Transform ShotPoint;

    // Ball Mode
    public GameObject standing;//, ball;
    // public float waitToBall;
    // private float ballCounter;
    // public Animator ballAnim;

    // Ball Bomb
    // public Transform bombPoint;
    // public GameObject bomb;

    public bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        // abilities = GetComponent<HeroineAbilityTracker>();
        abilities = GetComponent<PlayerAbilityTracker>();
        canMove = true;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Dash
        if (canMove && Time.timeScale != 0 && !isDead)
        {

            if (dashRechargeCounter > 0)
            {
                dashRechargeCounter -= Time.deltaTime;
            }
            else
            {
                if (Input.GetButtonDown("Fire2") && abilities.canDash)
                {
                    dashCounter = dashTime;
                    anim.SetTrigger("dashing");

                    // ShowAfterImage();

                    AudioManager.instance.PlaySFXAdjusted(7);
                }
            }

            // The "player cannot move" conditions
            // Player is Dashing
            if (dashCounter > 0)
            {
                dashCounter = dashCounter - Time.deltaTime;

                theRB.velocity = new Vector2(dashSpeed * transform.localScale.x, theRB.velocity.y);

                // afterImageCounter -= Time.deltaTime;
                // if (afterImageCounter <= 0) {
                //     ShowAfterImage();
                // }

                dashRechargeCounter = waitAfterDashing;

                Collider2D[] objectsToRemove = Physics2D.OverlapCircleAll(transform.position, dashRange, whatIsDashDestructible);

                if (objectsToRemove.Length > 0)
                {
                    foreach (Collider2D item in objectsToRemove)
                    {
                        Destroy(item.gameObject);
                    }
                }

                //TODO: Add enemy damage
            }
            // Player is attacking
            else if (attackCounter > 0)
            {
                attackCounter = attackCounter - Time.deltaTime;
                if (attackCounter <= 0)
                {
                    attackCounter = 0;
                    cutBlob.SetActive(false);
                }
            }
            // Player is hurt
            else if (hurtCounter > 0)
            {
                hurtCounter -= Time.deltaTime;
                if (hurtCounter <= 0)
                {
                    hurtCounter = 0;
                    Debug.Log("Player No Longer Hurt, setting isUp.");
                    anim.SetTrigger("isUp");
                }
            }
            else
            {
                moveCharacter(Input.GetAxisRaw("Horizontal"));
            }



            // Am I on the Ground?
            isOnGround = Physics2D.OverlapCircle(groundPoint.position, .2f, whatIsGround);

            // If character is hurt, do not jump or attack
            if (hurtCounter <= 0)
            {

                // Jumping
                if (Input.GetButtonDown("Jump") && isOnGround && attackCounter == 0)
                {
                    AudioManager.instance.PlaySFXAdjusted(12);
                    theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                }



                if (Input.GetButtonDown("Fire1") && attackCounter == 0)
                {
                    cutBlob.SetActive(true);
                    // Instantiate(attackController, swordAttackPoint.position, swordAttackPoint.rotation);
                    anim.SetTrigger("attacking");
                    attackCounter = attackTime;

                    // Instantiate(shotToFire, ShotPoint.position, ShotPoint.rotation).moveDir = new Vector2(transform.localScale.x, 0f);
                    // anim.SetTrigger("shotFired");

                    AudioManager.instance.PlaySFXAdjusted(14);

                    if (Mathf.Abs(theRB.velocity.y) < 0.1)
                    {
                        theRB.velocity = new Vector2(0, 0);
                    }
                }

            }

        }
        else
        {
            theRB.velocity = Vector2.zero;
        }



        // Set Animation params
        if (standing.activeSelf)
        {
            anim.SetBool("isOnGround", isOnGround);
            anim.SetFloat("speedHoriz", Mathf.Abs(theRB.velocity.x));
            anim.SetFloat("speedVert", theRB.velocity.y);
        }
    }

    void moveCharacter(float Horizontal)
    {
        // Move Sideways
        theRB.velocity = new Vector2(Horizontal * moveSpeed, theRB.velocity.y);

        // Handle direction Change
        if (theRB.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (theRB.velocity.x > 0)
        {
            transform.localScale = Vector3.one;
        }
    }


    public void ShowAfterImage()
    {
        SpriteRenderer image = Instantiate(afterImage, transform.position, transform.rotation);
        image.sprite = theSR.sprite;
        image.transform.localScale = transform.localScale;
        image.color = afterImageColor;

        Destroy(image.gameObject, afterImageLifetime);

        afterImageCounter = timeBetweenAfterImages;
    }

    public void hurtPlayer()
    {
        anim.SetTrigger("isHurt");
        hurtCounter = hurtTime;
        theRB.velocity = Vector2.zero;
    }

    public void killPlayer()
    {
        anim.SetTrigger("isDead");
        isDead = true;
        theRB.velocity = Vector2.zero;
    }

    public void revivePlayer()
    {
        isDead = false;
    }
}
