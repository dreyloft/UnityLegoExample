using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cutscene;


public class openDoor : MonoBehaviour {
    public bool openLeft;
    public bool locked;
    public bool open;
    public float speed;
    public float maxAngle;
    public GameObject turnAxis;
    public GameObject levelDummie;

    private bool externalOpen;
    private float minAngle;
    private level0 level0;

    void Awake()
    {
        level0 = levelDummie.GetComponent<level0>();
    }

    // Use this for initialization
    void Start () {
        minAngle = transform.eulerAngles.y;
        maxAngle = transform.eulerAngles.y + maxAngle;

        if (!openLeft)
        {
            speed = -speed;
        }
	}
	
	// Update is called once per frame
	void Update () {
        open = level0.SafeOpen;
        
        if (!locked)
        {
            if (transform.eulerAngles.y < maxAngle && open)
            {
                transform.RotateAround(turnAxis.transform.position, transform.up, speed * Time.deltaTime);
            }
            else if (transform.eulerAngles.y > minAngle && !open)
            {
                transform.RotateAround(turnAxis.transform.position, transform.up, -speed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        bool block = false;

        if ((Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 1")) && !open && !block)
        {
            block = true;
            open = true;
        }

        if ((Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 1")) && open && !block)
        {
            block = true;
            open = false;
        }
    }
}
