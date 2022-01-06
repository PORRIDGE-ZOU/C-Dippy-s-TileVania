using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{

    [SerializeField] AudioClip pickupSound;
    [SerializeField] int pointForOneCoin = 100;
    bool wasCollected = false; //from Rick; to avoid second-pickup bug

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !wasCollected)
        {
            Debug.Log("coin picked!");
            AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position); 
            Destroy(gameObject);
            FindObjectOfType<GameSession>().PickUpOneCoin(pointForOneCoin);
            wasCollected = true;
        }
    }

}
