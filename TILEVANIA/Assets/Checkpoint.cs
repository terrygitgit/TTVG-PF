using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    [SerializeField] GameObject spawn;
    [SerializeField] bool onLayerOne;

    public bool activated = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated)
        {
            if (collision.tag == "Player")
            {
                if (collision.GetComponent<Player>().onLayerOne && onLayerOne) { 
                    spawn.GetComponent<Transform>().position = transform.position;
                    activated = true;
                }
                else if (!collision.GetComponent<Player>().onLayerOne && !onLayerOne)
                {
                    spawn.GetComponent<Transform>().position = transform.position;
                    activated = true;
                }
            }
            
            
        }
    }
}
