using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    Rigidbody2D rigidbody;
    bool isFired = false;
    [SerializeField] float arrowSpeed = 10f;
    PlayerMovement player;
    float xSpeed;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * arrowSpeed;
        transform.localScale = new Vector2(Mathf.Sign(player.transform.localScale.x), 1f);
    }

    void Update()
    {
        
        if (!isFired)
        {
            rigidbody.velocity = new Vector2(xSpeed, 0f);
            isFired = true;
        }

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger. destroying");
        if (other.tag == "Rarraa")
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }



    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("collision. destroying");
        StartCoroutine(waitForDestroy());
        
    }

    IEnumerator waitForDestroy()
    {
        yield return new WaitForSecondsRealtime(2f);
        Destroy(gameObject);
    }

}
