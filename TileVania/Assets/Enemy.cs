using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float stunMultiplier = 1f;
    [SerializeField] GameObject tripwire;

    Rigidbody2D myRigidbody2D;
    Collider2D oldCollider;
    public bool canMove = true;
    public bool prepare = false;
	// Use this for initialization
	void Start () {
        myRigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
    {
        if (!canMove)
        {
            CheckWire();
        }
        if (canMove)
        {
            Move();
        }
    }

    private void CheckWire()
    {
        if (tripwire.GetComponent<Wire>().tripped)
        {
            canMove = true;
        }
    }

    private void Move()
    {
        if (IsFacingRight())
        {
            myRigidbody2D.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            myRigidbody2D.velocity = new Vector2(-moveSpeed, 0f);
        }
    }

    bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetType() == typeof(CompositeCollider2D))
        {
            if (collision.tag == "Ground" || collision.tag == "Hazard1" || collision.tag == "Hazard2" || collision.tag == "Blocker1" || collision.tag == "Blocker2")
            {
                Flip();
            }
        }
        
    }

    public void Flip()
    {
        transform.localScale = new Vector2(-Mathf.Sign(myRigidbody2D.velocity.x), 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision is CapsuleCollider2D)
        {
            if (collision.GetComponent<Collider2D>().tag == "Player")
            {
                if (collision.GetComponent<Player>().onLayerOne && gameObject.tag == "Ground1")
                {
                    FindObjectOfType<Player>().TakeDamage("Enemy", gameObject.GetComponent<Collider2D>(), stunMultiplier);
                }

                if (!collision.GetComponent<Player>().onLayerOne && gameObject.tag == "Ground2")
                {
                    FindObjectOfType<Player>().TakeDamage("Enemy", gameObject.GetComponent<Collider2D>(), stunMultiplier);
                }
            }
        }
    }

}
