using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feather : MonoBehaviour {
    private bool isFalling;
    private float fallingSpeed = 0.25f;
    private float raisingSpeed = 1.0f;
    private float maxRotation = 10.0f;
    private float rotationSpeed = 5.0f;
    private float startHeight;
    private float maxHeight = 1.0f;

    // Use this for initialization
    void Start () {
        startHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= startHeight)
        {
            isFalling = false;
        }
        else if (transform.position.y >= startHeight + maxHeight)
        {
            isFalling = true;
        }

        if (isFalling)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(transform.position.x, startHeight, transform.position.z),
                fallingSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(maxRotation * Mathf.Sin(Time.time * rotationSpeed) - 90, 0.0f, 0.0f);
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(transform.position.x, startHeight + maxHeight, transform.position.z),
                raisingSpeed * Time.deltaTime);
        }        
    }
}
