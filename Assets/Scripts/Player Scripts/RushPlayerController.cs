using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushPlayerController : MonoBehaviour, PlayerControllerInterface
{

    public Rigidbody2D theRB;

    public float moveSpeed;
    public float jumpForce;

    public Transform groundPoint;
    private bool isOnGround;
    private bool canDoubleJump;

    public float jumpButtonGracePeriod;

    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;
    public LayerMask whatIsGround;

    public Animator anim;
    public SpriteRenderer theSR;

    private PlayerAbilityTracker abilities;

    //Player hurt settings

    public float hurtTime;

    private float hurtCounter;

    private bool isDead;

    public float pushBackX;

    public float pushBackY;

    // Shooting

    public BulletController shotToFire;
    public Transform ShotPoint;

    public float shootTime;
    private float shootCounter;

    // ducking/sliding
    public float slideDeadZone;
    public float slideSlowDown;

    public bool canMove;

    private bool isDucking;

    public Animator duckingAnim;

    public Animator slidingAnim;

    public GameObject standing, sliding, ducking;

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
        if (!canMove || Time.timeScale == 0 || isDead)
        {
            theRB.velocity = Vector2.zero;
            return;
        }

        // dash();

        // If the player isn't dashing, attacking, or busy being hurt, the character can be moved.
        // if (!updateDash() && !updateAttacking() && !updatePlayerHurt())
        if (!updatePlayerHurt() && !updateShooting() && !updateSlide())
        {
            moveCharacter(Input.GetAxisRaw("Horizontal"));
        }

        // Am I on the Ground?
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, .2f, whatIsGround);

        if (isOnGround)
        {
            lastGroundedTime = Time.time;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }

        // If character is hurt, do not jump or attack
        if (!isPlayerHurt() && !isPlayerShooting())
        {
            jump();
            shoot();
            duckAndSlide();
            // attack();
        }


        // Set Animation params
        if (standing.activeSelf)
        {
            anim.SetBool("isOnGround", isOnGround);
            anim.SetFloat("speedHoriz", Mathf.Abs(theRB.velocity.x));
            anim.SetFloat("speedVert", theRB.velocity.y);
            anim.SetBool("isDucking", isDucking);
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

    private void duckAndSlide()
    {
        if (Input.GetAxisRaw("Vertical") < -.9f && isOnGround) // Mathf.Abs(theRB.velocity.y) < 0.4)
        {
            isDucking = true;
        }
        else
        {
            isDucking = false;
        }
    }

    private bool updateSlide() {
        if (!isDucking) {
            ducking.SetActive(false);
            standing.SetActive(true);
            sliding.SetActive(false);
            return false;
        }
        if (Mathf.Abs(theRB.velocity.x) > slideDeadZone) {
            theRB.velocity = new Vector2(theRB.velocity.x*(1-slideSlowDown), theRB.velocity.y*2);
            ducking.SetActive(false);
            standing.SetActive(false);
            sliding.SetActive(true);
        } else {
            theRB.velocity = new Vector2(0, 0);
            if (!ducking.activeSelf) {
                ducking.SetActive(true);
                standing.SetActive(false);
                sliding.SetActive(false);
            }
        }
        return true;
    }


    private void jump()
    {
        if (sliding.activeSelf || ducking.activeSelf) {
            return;
        }
        // Jumping 
        // was the "jump" button pressed, the character was on the ground, or can double jump?
        if (hasJumpedRecently() && (hasTouchedGroundRecently() || (canDoubleJump && abilities.canDoubleJump))) // && attackCounter == 0)
        {
            if (hasTouchedGroundRecently())
            {
                canDoubleJump = true;
                AudioManager.instance.PlaySFXAdjusted(12); // TODO: constants!

            }
            else
            {
                canDoubleJump = false;
                anim.SetTrigger("doubleJump");
                AudioManager.instance.PlaySFXAdjusted(9); // TODO: constants!
            }

            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
            jumpButtonPressedTime = null;
            lastGroundedTime = null;
        }
    }

    private bool hasJumpedRecently()
    {
        return Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod;
    }

    private bool hasTouchedGroundRecently()
    {
        return Time.time - lastGroundedTime <= jumpButtonGracePeriod;
    }


    private bool updatePlayerHurt()
    {

        if (hurtCounter <= 0)
        {
            return false;
        }
        hurtCounter -= Time.deltaTime;
        if (hurtCounter <= 0)
        {
            hurtCounter = 0;
            Debug.Log("Player No Longer Hurt, setting isUp.");
            anim.SetTrigger("isUp");
        }
        return true;

    }

    public void hurtPlayer()
    {
        anim.SetTrigger("isHurt");
        hurtCounter = hurtTime;
        // theRB.velocity = Vector2.zero;
        theRB.velocity = new Vector2(-(transform.localScale.x * pushBackX),  pushBackY);
    }

    bool isPlayerHurt()
    {
        return hurtCounter > 0;
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

    public void setAnimationTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    private void shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (standing.activeSelf && Mathf.Abs(theRB.velocity.y) < 0.1)
            {
                Instantiate(shotToFire, ShotPoint.position, ShotPoint.rotation).moveDir = new Vector2(transform.localScale.x, 0f);
                anim.SetTrigger("shotFired");
                shootCounter = shootTime;

                AudioManager.instance.PlaySFXAdjusted(14);
                theRB.velocity = new Vector2(0, 0);
            }
        }
    }

    private bool updateShooting()
    {
        if (!isPlayerShooting())
        {
            return false;
        }
        shootCounter = shootCounter - Time.deltaTime;
        if (!isPlayerShooting())
        {
            shootCounter = 0;
        }
        return true;
    }

    bool isPlayerShooting()
    {
        return shootCounter > 0;
    }
}
