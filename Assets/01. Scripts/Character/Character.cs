using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        skeletonAnimation.state.SetAnimation(0, "Idle", true);
    }

    public virtual void Move()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += Vector3.right * moveSpeed * moveDistance;
            MoveAnimation();
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
    public abstract void Attack();
}
