using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public abstract class Character : MonoBehaviour
{
    [Header ("Move Setting")]
    //public float moveDistance = 0.3f;
    protected float moveSpeed = 5f;
    protected float jumpForce = 7f;

    [Header("Jump Setting")]
    public float jumpPower = 1f;
    private Rigidbody rb;

    [Header ("Spain & Animaion Setting")]
    public SkeletonAnimation skeletonAnimation;
    private Animator animator;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        AnimationTypeCheck();
    }
    public virtual void Update()
    {
        Move();
        Jump();
        ClickAttack();
    }
    private void AnimationTypeCheck()
    {
        if (skeletonAnimation != null)
        {
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            skeletonAnimation.state.SetAnimation(0, "Idle", true);
        }
        else
        {
            animator = GetComponent<Animator>();
        }
    }
    public virtual void Move()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;

            if(skeletonAnimation != null)
            {
                MoveSpainAnimation();
            }
            else if(animator != null)
            {
                MoveAnimation();
            }
        }
    }
    protected virtual void MoveAnimation()
    {
        animator.SetBool("isRunning", true);
    }
    protected virtual void MoveSpainAnimation()
    {
        if (skeletonAnimation.AnimationName == "Run") return;

        skeletonAnimation.state.ClearTrack(0);
        TrackEntry entry = skeletonAnimation.state.SetAnimation(0, "Run", false);
        entry.MixDuration = 0f;
        entry.Delay = 0f;

        entry.Complete += (entry) =>
        {
            skeletonAnimation.state.SetAnimation(0, "Idle", true).MixDuration = 0f;
        };
    }
    public virtual void Jump() 
    { 
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            rb.velocity = Vector3.zero;

            Vector3 jumpDirection = Vector3.up * jumpForce;
            rb.AddForce(jumpDirection, ForceMode.Impulse);

            if (skeletonAnimation != null)
            {
                JumpSpainAnimation();
            }
            else if (animator != null)
            {
                JumpAnimation();
            }
        }
    }
    protected virtual void JumpAnimation()
    {
        Debug.Log("Jump");
    }
    protected virtual void JumpSpainAnimation()
    {
        if (skeletonAnimation.AnimationName == "Jump") return;

        skeletonAnimation.state.ClearTrack(0);
        TrackEntry entry = skeletonAnimation.state.SetAnimation(0, "Jump", false);
        entry.MixDuration = 0f;
        entry.Delay = 0f;

        entry.Complete += (entry) =>
        {
            skeletonAnimation.state.SetAnimation(0, "Idle", true).MixDuration = 0f;
        };
    }
    public virtual void ClickAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }
    public abstract void Attack();
}
