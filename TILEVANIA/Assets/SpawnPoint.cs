using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

    public Transform myTransform;

    private void Start()
    {
        myTransform = GetComponent<Transform>();

    }
}
