using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bro : Character
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        Move();
        Jump();
    }
    public override void MoveAnimation()
    {
        animator.SetBool("isRunning", true);
    }
    private void CheckAnimation()
    {
        animator.SetBool("isRunning", false);
    }
    public override void Attack()
    {
        throw new System.NotImplementedException();
    }
}
