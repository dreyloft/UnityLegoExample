using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level0sleep1 : MonoBehaviour {
    private Animator animator;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        animator.SetInteger("sleep", 1);
    }
	
	// Update is called once per frame
	void Update () {
        if (animator.GetInteger("sleep") != 1)
        {
            animator.SetInteger("sleep", 1);
        }
    }
}
