using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour {

    [SerializeField] AudioClip sfx;
    [SerializeField] int pointsForCoinPickup = 1000;

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision is CapsuleCollider2D)
        {
            if (collision.GetComponent<Collider2D>().tag == "Player")
            {
                if (collision.GetComponent<Player>().onLayerOne && gameObject.tag == "Item1")
                {

                    FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickup);
                    

                    Destroy(gameObject);
                }

                if (!collision.GetComponent<Player>().onLayerOne && gameObject.tag == "Item2")
                {

                    FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickup);
                    

                    Destroy(gameObject);
                }
            }


           
        }
    }
    
}
