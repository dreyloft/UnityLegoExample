using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour {

    public float speed;
    public Color color;
    public float maxRotation;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        // fade color
        this.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, Mathf.Abs(Mathf.Sin(Time.time * speed)));

        // rotation back / forth
        transform.rotation = Quaternion.Euler(-90.0f, maxRotation * Mathf.Sin(Time.time * speed), 0.0f);

        // blink every 2nd second
        if ((int)Time.time % 2 == 0)
        {
            this.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            this.GetComponent<Renderer>().enabled = false;
        }
    }
}
