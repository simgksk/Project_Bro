using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bro : Character
{
    Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void MoveAnimation()
    {
        animator.SetBool("isRunning", true);
    }
    private void CheckAnimation()
    {
        animator.SetBool("isRunning", false);
    }
    protected override void JumpAnimation()
    {
        if (animator == null) return;

    }
    public override void Attack()
    {
        throw new System.NotImplementedException();
    }
}
