using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level0sleep2 : MonoBehaviour
{
    private Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("sleep", 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetInteger("sleep") != 2)
        {
            animator.SetInteger("sleep", 2);
        }
    }
}
