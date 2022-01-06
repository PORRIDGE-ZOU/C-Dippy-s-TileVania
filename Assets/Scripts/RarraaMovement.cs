using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RarraaMovement : MonoBehaviour
{

    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D rigidbody;
    BoxCollider2D wallCollider;// an attempt.
    CapsuleCollider2D bodyCollider;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        wallCollider = GetComponent<BoxCollider2D>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
    }


    void Update()
    {
        rigidbody.velocity = new Vector2(moveSpeed, 0f);
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Arrow")))
        {
            Debug.Log("monster is collided. destroying!");
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Finish")
        {
            moveSpeed *= -1;
            transform.localScale = new Vector2(-Mathf.Sign(rigidbody.velocity.x), 1f);
        }

    }
}
