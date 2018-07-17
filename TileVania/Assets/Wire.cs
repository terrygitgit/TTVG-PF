using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour {

    public bool tripped = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        tripped = true;
    }
}
