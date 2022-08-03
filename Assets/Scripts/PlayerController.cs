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

    public float dashRange;

    public LayerMask whatIsDashDestructible;


    private PlayerAbilityTracker abilities;

    // Shooting

    public BulletController shotToFire;
    public Transform ShotPoint;
    
    // Ball Mode
    public GameObject standing, ball;
    public float waitToBall;
    private float ballCounter;
    public Animator ballAnim;

    // Ball Bomb
    public Transform bombPoint;
    public GameObject bomb;

    public bool canMove;
    
    // Time invisible
    public float timeInvisible;
    private float invisibleCount;

    // Start is called before the first frame update
    void Start()
    {
        abilities = GetComponent<PlayerAbilityTracker>();
        canMove = true;
        invisibleCount = timeInvisible;
    }

    // Update is called once per frame
    void Update()
    {
        // Kill if invisible
        if (!theSR.isVisible) {
            if (invisibleCount <= 0){
                PlayerHealthController.instance.DamagePlayer(PlayerHealthController.instance.maxHealth);
            }
            invisibleCount -= Time.deltaTime;
        } else {
            invisibleCount = timeInvisible;
        }
        // Dash
        if (canMove && Time.timeScale != 0) {
    
            if (dashRechargeCounter > 0) {
                dashRechargeCounter -= Time.deltaTime;
            } else {
                if (Input.GetButtonDown("Fire2") && standing.activeSelf && abilities.canDash) {
                dashCounter = dashTime;

                ShowAfterImage();

                AudioManager.instance.PlaySFXAdjusted(7);
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

                Collider2D[] objectsToRemove = Physics2D.OverlapCircleAll(transform.position, dashRange, whatIsDashDestructible);

                if (objectsToRemove.Length > 0) {
                    foreach (Collider2D item in objectsToRemove)
                    {
                        Destroy(item.gameObject);
                    }
                }
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

                    AudioManager.instance.PlaySFXAdjusted(12);
                } else {
                    canDoubleJump = false;
                    anim.SetTrigger("doubleJump");

                    AudioManager.instance.PlaySFXAdjusted(9);
                }
                theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
            }



            if (Input.GetButtonDown("Fire1")) {
                if (standing.activeSelf)
                {
                    Instantiate(shotToFire, ShotPoint.position, ShotPoint.rotation).moveDir = new Vector2(transform.localScale.x, 0f);
                    anim.SetTrigger("shotFired");

                    AudioManager.instance.PlaySFXAdjusted(14);
                } else if(ball.activeSelf)
                {
                    Instantiate(bomb, bombPoint.position, bombPoint.rotation);

                    AudioManager.instance.PlaySFXAdjusted(13);
                }
                
            }


            // ball mode
            if (!ball.activeSelf) {
                //Are we pressing down?
                if (Input.GetAxisRaw("Vertical") < -.9f) {
                    ballCounter -= Time.deltaTime;
                    if (ballCounter <= 0) {
                        ball.SetActive(true);
                        standing.SetActive(false);

                        AudioManager.instance.PlaySFX(6);
                    }
                } else {
                    ballCounter = waitToBall;
                }
            } else {
                //Are we pressing up?
                if (Input.GetAxisRaw("Vertical") > .9f) {
                    ballCounter -= Time.deltaTime;
                    if (ballCounter <= 0) {
                        ball.SetActive(false);
                        standing.SetActive(true);

                        AudioManager.instance.PlaySFX(10);
                    }
                } else {
                    ballCounter = waitToBall;
                }
            }
        } else {
            theRB.velocity = Vector2.zero;
        }



        // Set Animation params
        if (standing.activeSelf) {
            anim.SetBool("isOnGround", isOnGround);
            anim.SetFloat("speed", Mathf.Abs( theRB.velocity.x));
        }

        if (ball.activeSelf) {
            ballAnim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
        }
        


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
