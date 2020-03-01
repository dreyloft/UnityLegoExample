using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevatorDoors : MonoBehaviour {
    public GameObject doorLeft;
    public GameObject doorRight;
    public float speed;
    
    private bool open;
    private float startposLeft;
    private float startposRight;
    private float endposLeft;
    
	// Use this for initialization
	void Start ()
    {
        startposLeft = doorLeft.transform.position.x;
        startposRight = doorRight.transform.position.x;
        endposLeft = startposLeft - doorLeft.transform.lossyScale.x;
    }
	
	// Update is called once per frame
	void Update () {
        if (open) {
            if(doorLeft.transform.position.x > endposLeft + 0.02f)
            {
                doorLeft.transform.position -= doorLeft.transform.right * speed * Time.deltaTime;
                doorRight.transform.position += doorRight.transform.right * speed * Time.deltaTime;
            }
        }
        else
        {
            if (doorLeft.transform.position.x < startposLeft)
            {
                doorLeft.transform.position += doorLeft.transform.right * speed * Time.deltaTime;
                doorRight.transform.position -= doorRight.transform.right * speed * Time.deltaTime;
            }
            else
            {
                doorLeft.transform.position = new Vector3(startposLeft, doorLeft.transform.position.y, doorLeft.transform.position.z);
                doorRight.transform.position = new Vector3(startposRight, doorRight.transform.position.y, doorRight.transform.position.z);
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        open = true;
    }

    private void OnTriggerExit(Collider other)
    {
        open = false;
    }
}
