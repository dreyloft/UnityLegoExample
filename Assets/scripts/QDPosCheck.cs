using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QDPosCheck : MonoBehaviour {
    void Update () {
	    if (this.transform.position.y >= 1.1f)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
        }	
	}
}
