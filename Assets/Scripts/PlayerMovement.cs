using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{


    Vector2 moveInput;
    float jumpInput = 0;
    Rigidbody2D rigidbody;
    [SerializeField] float velocity = 5;
    [SerializeField] float jumpSpeed = 10;
    [SerializeField] float climbSpeed = 5;
    [SerializeField] Collider2D groundCollider;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowPoint;


    Animator animator;
    bool playerHasHorizontalSpeed;
    CapsuleCollider2D capsuleCollider2D;
    BoxCollider2D feetCollider;
    CircleCollider2D headCollider;
    float gravity;
    bool isJumping = false;
    float tiktok = 0;
    bool isAlive = true;
    

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        gravity = rigidbody.gravityScale;
        feetCollider = GetComponent<BoxCollider2D>();
        headCollider = GetComponent<CircleCollider2D>();
    }


    void Update()
    {
        if (!isAlive)
        {
            return;
        }
        Run();
        FlipSprite();
        Jump();
        ClimbLadder();
        CheckJumpOver_1();
        Die();
    }

    void FlipSprite()
    {
        playerHasHorizontalSpeed = Mathf.Abs(rigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidbody.velocity.x), 1f);
        }   
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * velocity, rigidbody.velocity.y);
        rigidbody.velocity = playerVelocity;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);//nice code!
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {

        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Ladder", "Water")))
        {
            return;
        }



        if (value.isPressed && !isJumping)
        {
            jumpInput = value.Get<float>();
            Debug.Log("now jumping!");
            isJumping = true;
        }
    }
        
    

    void Jump()
    {
        if (jumpInput != 0)
        {
            rigidbody.gravityScale = gravity;
            rigidbody.velocity += new Vector2(0f, jumpSpeed);
            isJumping = true;
            jumpInput = 0;
        }
    }

    bool triggerForTwo = false;
    void CheckJumpOver_1()
    {
        if (isJumping && !feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            triggerForTwo = true;
        }
        if (triggerForTwo) { CheckJumpOver_2(); }
    }

    void CheckJumpOver_2()
    {
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            Debug.Log("now jumping is over!");
            isJumping = false;
            triggerForTwo = false;
        }
    }



    void ClimbLadder()
    {
        if (!capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            //capsuleCollider2D.isTrigger = false;//see below comments.
            animator.SetBool("isClimbing", false);
            rigidbody.gravityScale = gravity;
            return;
        }

        
        if (isJumping)
        {
            rigidbody.gravityScale = gravity;
            animator.SetBool("isClimbing", false);
            return;
        }
        else if (!isJumping)
        {
            rigidbody.gravityScale = 0;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, moveInput.y * climbSpeed);



            if (moveInput.y != 0)
            {
                animator.SetBool("isClimbing", true);
            }
            else
            {
                animator.SetBool("isClimbing", false);
            }
        }

    }

    //an attempt but failed.
    void CheckThroughLatter()
    {
        if (headCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) &&
            !feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && isJumping == false)
        {
            capsuleCollider2D.isTrigger = true;
        }
        else if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) &&
            !headCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            capsuleCollider2D.isTrigger = false;
            
        }
    }

    Animation fire;
    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        Instantiate(arrow, arrowPoint.position, transform.rotation);
        animator.Play("Shooting");
       
    }




    void Die()
    {
        if (capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Rarraa","Hazard")))
        {
            isAlive = false;
            rigidbody.velocity = new Vector2(-5f, 15f);
            rigidbody.gravityScale = gravity/2;
            capsuleCollider2D.isTrigger = true;
            animator.SetTrigger("Dying");
            StartCoroutine(waitForReload());
            
        }
    }


    IEnumerator waitForReload()
    {
        yield return new WaitForSecondsRealtime(2f);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
}



//if (boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
//{
//    capsuleCollider2D.isTrigger = true;
//}
// THESE LINES ARE USED to enable the passthrough of normal grounds
// when C-Dippy is climbing ladder. However I failed to make it.
// Now one thought is to add two more colliders on feet and head to
// detect ground colliding. But I DO NOT KNOW HOW TO SEPARATE TWO
// box colliders!!!!!!! what the fk!
