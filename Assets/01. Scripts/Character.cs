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
    public float moveDistance = 0.3f;
    protected float moveSpeed = 5f;
    protected float jumpForce = 7f;

    [Header("Jump Setting")]
    public float jumpPower = 1f;
    private Rigidbody rb;

    [Header ("Spain & Animaion Setting")]
    public SkeletonAnimation skeletonAnimation;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (skeletonAnimation != null)
        {
            skeletonAnimation.state.SetAnimation(0, "Idle", true);
        }
    }
    public virtual void Update()
    {
        Move();
        Jump();
        ClickAttack();
    }

    public virtual void Move()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveAnimation();
            transform.position += Vector3.right * moveSpeed * moveDistance;
        }
    }
    public virtual void MoveAnimation()
    {
        if (skeletonAnimation == null)
            return;

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

            JumpAnimation();
        }
    }
    protected virtual void JumpAnimation()
    {
        if (skeletonAnimation == null)
            return;

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
