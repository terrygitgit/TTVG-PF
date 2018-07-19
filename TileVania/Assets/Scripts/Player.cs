using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    //Config
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float jumpLength = 5f;
    [SerializeField] float vertJumpSpeed = 2f;
    [SerializeField] float maxJumpLength = 5;
    [SerializeField] float grav = 2.5f;


    [SerializeField] float doubleJumpSpeed = 7f;
    [SerializeField] float tooFastMultiplier = .5f;
    [SerializeField] Vector2 deathKick = new Vector2(5f, 5f);
    [SerializeField] Vector2 hitKick = new Vector2(5f, 5f);
    [SerializeField] bool doubleJumpAllowed = false;
    [SerializeField] float teleTime = 2f;
    [SerializeField] float LayerOne = -3;
    [SerializeField] float LayerTwo = -5;
    [SerializeField] int hp = 3;
    [SerializeField] float stunTime = .5f;
    [SerializeField] float breather = .1f;
    [SerializeField] float dieTime = .5f;



    [SerializeField] float climbJumpSpeedH = 2f;
    [SerializeField] float jumpSpeedH = 2f;

    float tooFast;
    float gravityScale;
    Vector2 startVelocity;
    float totalStuntime;

    //States
    bool isAlive = true;
    bool canMove = true;
    bool canDoubleJump = false;
    bool invincibility = false;
    bool weighton = false;

    bool airbourne = false;
    bool stunned = false;
    bool stunnedAndDying = false;

    bool climbing = false;
    public bool onLayerOne = true;
    bool ladder1 = false;
    bool ladder2 = false;

    bool ground1 = false;
    bool ground2 = false;

    //
    Rigidbody2D myRigidBody;
    CapsuleCollider2D myBodyCollider; //BODY
    BoxCollider2D myFeetCollider; //FEET
    Animator myAnimator;
    SpriteRenderer mySpriteRenderer;
    PolygonCollider2D myTeleportCollider;

    Vector3 spawnPoint;
    


    [SerializeField] GameObject[] Layer1s;
    [SerializeField] GameObject[] Layer2s;


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Ladder1")
        {
            ladder1 = true;
        } else if (collider.tag == "Ladder2")
        {
            ladder2 = true;
        }


        //Also
        DetectDeath(collider);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Ladder1")
        {
            ladder1 = false;
        }
        else if (collider.tag == "Ladder2")
        {
            ladder2 = false;
        }



    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.tag == "Ground" && stunned)
        {
            StartCoroutine(StopStun(totalStuntime));
        }
        if (collision.collider.tag == "Ground" && stunnedAndDying)
        {
            StartDeath();
        }


    }

    

    private void StartDeath()
    {
        myRigidBody.isKinematic = true;
        startVelocity = myRigidBody.velocity;
        myRigidBody.velocity = new Vector2(0f, 0f);
        canMove = false;
        StopClimbing();
        myBodyCollider.enabled = false;
        myFeetCollider.enabled = false;
        myAnimator.ResetTrigger("Stunned");
        myAnimator.SetTrigger("Dying");

    }



    // Messages and then Methods
    void Start()
    {

        

        myRigidBody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponent<Animator>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myTeleportCollider = GetComponent<PolygonCollider2D>();
        tooFast = tooFastMultiplier * jumpSpeed;

        deathKick = new Vector2(Mathf.Abs(deathKick.x), Mathf.Abs(deathKick.y));
        hitKick = new Vector2(Mathf.Abs(hitKick.x), Mathf.Abs(hitKick.y));

        gravityScale = myRigidBody.gravityScale;

        

        spawnPoint = FindObjectOfType<SpawnPoint>().myTransform.position;

        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            spawnPoint = new Vector3(-74.05852f, -5.759879f, -3f);
        }

        Spawn();

    }

    public void Spawn()
    {
        transform.position = spawnPoint;
        print(spawnPoint);
        if (transform.position.z == LayerOne)
        {
            onLayerOne = true;
            Farther();
        }
        else if (transform.position.z == LayerTwo)
        {
            onLayerOne = false;
            Closer();
        }
    }


    // Update is called once per frame
    void Update()
    {

        
        if (isAlive && canMove)
        {
            if (stunned)
            {
                Stunned();
            }

            if (weighton)
            {
                WeightOn();
            }

            CheckClimb();
            UpdateAirbourneStatus();

            if (!stunned)
            {
                if (!airbourne)
                {
                    if (!climbing)
                    {
                        Run();
                    }
                    Jump();
                    
                }
                else {
                    if (!climbing)
                    {
                        //Steer();
                    }
                }
                FinishJump();


                if (doubleJumpAllowed)
                {
                    CheckDoubleJump();
                    DoubleJump();
                }

                
                Climb();
            }

            Warp();

        } else if (!isAlive)
        {
            ReAlive();
        }
    }

    private void WeightOn()
    {
        if (Mathf.Sign( myRigidBody.velocity.y) == 1)
        {
            myRigidBody.gravityScale += grav;
        } else
        {
            myRigidBody.gravityScale = gravityScale;
            weighton = false;
        }
    }

    void UpdateAirbourneStatus ()
    {
        if (climbing) { airbourne = false; return; }

        if (onLayerOne)
        {
            if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground1")))
            {

                myRigidBody.gravityScale = gravityScale;
                airbourne = false;
                return;
            }
        }
        else
        {
            if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground2")))
            {
                myRigidBody.gravityScale = gravityScale;
                airbourne = false;
                return;
            }
        }
        airbourne = true;
    }


    void Warp()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            myAnimator.SetTrigger("Teleport");

            myRigidBody.isKinematic = true;
            startVelocity = myRigidBody.velocity;
            myRigidBody.velocity = new Vector2(0f, 0f);
            canMove = false;
            
            StopClimbing();
        }
    }

    public void Teleport()
    {
        mySpriteRenderer.enabled = false;
        StartCoroutine(WaitAndFinishTeleport(teleTime));

    }

    IEnumerator WaitAndFinishTeleport(float teleTime)
    {
        yield return new WaitForSeconds(teleTime);
        FinishTeleport();
    }

    private void FinishTeleport()
    {
        GameObject[] Layer1Objects = GameObject.FindGameObjectsWithTag("Ground1");
        GameObject[] Layer2Objects = GameObject.FindGameObjectsWithTag("Ground2");

        
        if (onLayerOne)
        {
            
            

            Closer();
            onLayerOne = false;
        }
        else
        {
            

            Farther();
            onLayerOne = true;
        }

        myBodyCollider.enabled = false;
        myFeetCollider.enabled = false;
        myAnimator.SetTrigger("Teleport 2");
    }

    public void Closer()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, LayerTwo);
        mySpriteRenderer.sortingLayerName = "Ground2";

        foreach (GameObject thing in Layer1s)
        {
            thing.GetComponent<CompositeCollider2D>().isTrigger = true;
        }

        foreach (GameObject thing in Layer2s)
        {
            thing.GetComponent<CompositeCollider2D>().isTrigger = false;
        }

    }
    
    public void Farther()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, LayerOne);
        mySpriteRenderer.sortingLayerName = "Ground1";

        foreach (GameObject thing in Layer1s)
        {
            thing.GetComponent<CompositeCollider2D>().isTrigger = false;
        }

        foreach (GameObject thing in Layer2s)
        {
            thing.GetComponent<CompositeCollider2D>().isTrigger = true;
        }
    }

    private void TurnSpriteRendererBackOn()
    {
        mySpriteRenderer.enabled = true;
    }

    private void TurnMovementBackOn()
    {
        if (onLayerOne)
        {


            if (myTeleportCollider.IsTouchingLayers(LayerMask.GetMask("Ground1")))
            {
                StartDeath();
                return;
            }
            
        }
        else
        {
            if (myTeleportCollider.IsTouchingLayers(LayerMask.GetMask("Ground2")))
            {
                StartDeath();
                return;
            }

            
        }

        


        airbourne = true;

        myBodyCollider.enabled = true;
        myFeetCollider.enabled = true;
        myRigidBody.isKinematic = false;
        myRigidBody.velocity += startVelocity;
        canMove = true;
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // -1 to +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        FlipSpriteIfNeeded();

    }

    private void Steer()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // -1 to +1
        Vector2 playerVelocity = new Vector2(Mathf.Clamp( myRigidBody.velocity.x + (controlThrow * runSpeed * .1f) ,-runSpeed, runSpeed), myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        FlipSpriteIfNeeded();
    }

    private void Stunned()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon; //mathf.epsilon = 0?

        if (playerHasHorizontalSpeed)
        {
            print("stunned");
            transform.localScale = new Vector2(Mathf.Sign(-myRigidBody.velocity.x), transform.localScale.y);
        }
    }

    private void FlipSpriteIfNeeded()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon; //mathf.epsilon = 0?
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);


        if (playerHasHorizontalSpeed && !stunned)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), transform.localScale.y);
        }
    }

    private void Jump()
    {

        if (climbing)
        {
            if (onLayerOne)
            {
                if (!ladder1)
                {
                    return;
                }
            }
            else
            {
                if (!ladder2)
                {
                    return;
                }
            }
        }

        
        
        if (CrossPlatformInputManager.GetButton("Jump"))
        {

            if (climbing)
            {
                if ( CrossPlatformInputManager.GetAxis("Horizontal") == 0)
                {
                    StopClimbing();
                    Vector2 jumpVelocityToAdd = new Vector2(0, jumpSpeed)*.5f;
                    myRigidBody.velocity += jumpVelocityToAdd;
                    canDoubleJump = false;
                }
                else if (Mathf.Sign(CrossPlatformInputManager.GetAxis("Horizontal")) == 1)
                {
                    StopClimbing();
                    Vector2 jumpVelocityToAdd = new Vector2(climbJumpSpeedH, jumpSpeed*.5f);
                    myRigidBody.velocity += jumpVelocityToAdd;
                    canDoubleJump = false;
                }
                else if (Mathf.Sign(CrossPlatformInputManager.GetAxis("Horizontal")) == -1)
                {
                    StopClimbing();
                    Vector2 jumpVelocityToAdd = new Vector2(-climbJumpSpeedH, jumpSpeed*.5f);
                    myRigidBody.velocity += jumpVelocityToAdd;
                    canDoubleJump = false;
                }
                
            }
            else
            {
                ActuallyJump();
            }
        }


    }

    private void ActuallyJump()
    {
        if (CrossPlatformInputManager.GetAxis("Horizontal") == 0)
        {
            Vector2 jumpVelocityToAdd = new Vector2(0, 12);
            myRigidBody.velocity = jumpVelocityToAdd;

        }

        else if (Mathf.Sign(CrossPlatformInputManager.GetAxis("Horizontal")) == 1)
        {
            Vector2 jumpVelocityToAdd = new Vector2(jumpLength, jumpSpeed);
            myRigidBody.velocity = jumpVelocityToAdd;

        }
        else if (Mathf.Sign(CrossPlatformInputManager.GetAxis("Horizontal")) == -1)
        {
            Vector2 jumpVelocityToAdd = new Vector2(-jumpLength, jumpSpeed);
            myRigidBody.velocity = jumpVelocityToAdd;

        }
    }

    private void FinishJump()
    {
        
        if (CrossPlatformInputManager.GetButtonUp("Jump"))
        {
            weighton = true;
            
        }
    }


    private void CheckDoubleJump()
    {
        if (climbing) { canDoubleJump = false; }
        if (canDoubleJump) { return; }
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground1")) ^ 
            myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground2"))) { canDoubleJump = true; }


    }

    private void DoubleJump()
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground1")) 
            ^ myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground2"))) { return; }

        if (canDoubleJump)
        {
            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                if (myRigidBody.velocity.y < tooFast)
                {
                    print("double");
                    Vector2 jumpVelocityToAdd = new Vector2(myRigidBody.velocity.x, doubleJumpSpeed);
                    myRigidBody.velocity = jumpVelocityToAdd;

                    canDoubleJump = false;
                }
            }
        }
    }

    private void CheckClimb()
    {

        if (climbing)
        {
            //TURN OFF CLIMBING IF NEEDED
            if (onLayerOne)
            {
                if (!ladder1)
                {
                    StopClimbing();
                    return;

                }
                else if (!onLayerOne)
                {
                    StopClimbing();
                    return;
                }
            }
            else {
                if (!ladder2)
                {
                    StopClimbing();
                    return;

                }
                else if (onLayerOne)
                {
                    StopClimbing();
                    return;
                }
            }

        } else if (!climbing)
        {   //TURN ON CLIMBING IF NEEDED
            if (CrossPlatformInputManager.GetButton("Vertical"))
            {
                if (ladder1 && onLayerOne)
                {
                    myRigidBody.velocity = new Vector2(0, 0);
                    climbing = true;
                    print(climbing);
                } else 
                if (ladder2 && !onLayerOne)
                {
                    myRigidBody.velocity = new Vector2(0, 0);
                    climbing = true;
                    print(climbing);
                }

            }
        }
    }

    private void StopClimbing()
    {
        myAnimator.SetBool("Climbing", false);
        myRigidBody.gravityScale = gravityScale;
        climbing = false;
        canDoubleJump = false;
        return;
    }

    private void Climb()
    {

        if (!climbing) //fail safe
        {
            return;
        }
        
        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0f;

        bool playerHasVerticalSped = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;

        myAnimator.SetBool("Warping", false);
        myAnimator.SetBool("Running", false);
        myAnimator.SetBool("Climbing", true);

    }
    
    
    
    private void DetectDeath(Collider2D collision)
    {
        if (!isAlive) { return; }
        if (invincibility) { return; }

        if (collision.tag == "Hazard1")
        {
            if (onLayerOne)
            {
                StartDeath();
            }
        }
        if (collision.tag == "Hazard2")
        {
            if (!onLayerOne)
            {
                StartDeath();
            }
        }

        if (collision.GetComponent<Collider2D>().gameObject.layer == 10) //MAGIC
        {
            FallAndDie();
        }

    }

    private void FallAndDie()
    {
        print("ded");
        myAnimator.SetTrigger("Falling");
        isAlive = false;
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    private void Die()
    {
        myAnimator.SetTrigger("Dying");
        FindObjectOfType<GameSession>().ProcessPlayerDeath();

        isAlive = false;
    }

    private void ReAlive()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            transform.position = spawnPoint;
            myAnimator.SetTrigger("Waking");
            isAlive = true;
        }
    }

    public void TakeDamage(string type, Collider2D enemyCollider, float stunMultiplier)
    {
        if (invincibility) { return; }

        if (type == "Enemy")
        {
            hp--;
            if (hp == 0)
            {
                
                Knockback(enemyCollider, stunMultiplier, true);
            } else
            {
                invincibility = true;
                StartCoroutine(TurnOffInvincibility(breather));
                Knockback(enemyCollider, stunMultiplier, false);
            }
        }
    }

    private void Knockback(Collider2D collision, float stunMultiplier, bool dieAfter)
    {
        float idk = collision.GetComponent<Transform>().position.x;
        float idkk = idk - transform.position.x;

        hitKick = new Vector2(Mathf.Abs(hitKick.x), Mathf.Abs(hitKick.y));

        stunned = true;
        totalStuntime = stunTime * stunMultiplier;
        if (Mathf.Sign(idkk) == 1)
        {
            myRigidBody.transform.localScale = 
                new Vector3(myRigidBody.transform.localScale.x, myRigidBody.transform.localScale.y, 1);
            hitKick = new Vector2(-hitKick.x, hitKick.y);
            myRigidBody.velocity = hitKick;
        }
        else
        {
            myRigidBody.transform.localScale = 
                new Vector3(myRigidBody.transform.localScale.x, myRigidBody.transform.localScale.y, 1); 
            myRigidBody.velocity = hitKick;
        }
        if (dieAfter)
        {
            myAnimator.SetTrigger("Stunned");
            stunnedAndDying = true;
        } 
        else
        {
            myAnimator.SetTrigger("Stunned");
        }
        
    }

    

    IEnumerator DieAfterDelay(float dietime)
    {
        yield return new WaitForSeconds(dietime);
        Die();
    }

    IEnumerator TurnOffInvincibility(float breather)
    {
        yield return new WaitForSeconds(breather);
        invincibility = false;
    }

    IEnumerator StopStun(float totalStunTime)
    {
        yield return new WaitForSeconds(totalStunTime);
        myAnimator.SetTrigger("StunRecovery");
        stunned = false;
    }

}

