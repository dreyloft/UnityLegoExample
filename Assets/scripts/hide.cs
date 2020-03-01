using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hide : MonoBehaviour {
    void Start () {
        GetComponent<Renderer>().enabled = false;
    }
}
