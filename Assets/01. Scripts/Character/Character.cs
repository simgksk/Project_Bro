using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Character : MonoBehaviour
{
    [Header("Move Setting")]
    protected float moveDistance = 3f;
    protected float moveDuration = 0.15f;
    protected bool isMoving = false;

    [Header("Jump Setting")]
    protected float jumpForce = 7f;
    public float jumpPower = 1f;
    private Rigidbody rb;
    [SerializeField]bool isJumping = false;

    [Header("Spain & Animaion Setting")]
    public SkeletonAnimation skeletonAnimation;
    private Animator animator;
    private string currentAnimation = "";

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

    #region Move
    public virtual void Move()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!isMoving)
            {
                StartCoroutine(MoveCourutine());
            }
            //transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }

        if(isJumping)
        {
            return;
        }

        if (skeletonAnimation != null)
        {
            SetSpineAnimation(isMoving ? "Run" : "Idle");
        }
        else if (animator != null)
        {
            animator.SetBool("isRunning", isMoving);
        }
    }

    IEnumerator MoveCourutine()
    {
        isMoving = true;
        float t = 0;
        
        Vector3 start = transform.position;
        Vector3 target = transform.position + Vector3.right * moveDistance;

        while (t < moveDuration)
        {
            yield return null;

            t += Time.deltaTime;

            transform.position = Vector3.Lerp(start, target, t / moveDuration);
        }
        isMoving = false;
        rb.linearVelocity = Vector3.zero;
    }

    #endregion

    #region Jump
    public virtual void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            isJumping = true;

            rb.linearVelocity = Vector3.zero;

            Vector3 jumpDirection = Vector3.up * jumpForce;
            rb.AddForce(jumpDirection, ForceMode.Impulse);

            if (skeletonAnimation != null)
            {
                SetSpineAnimation("Jump", false);
            }
            else if (animator != null)
            {
                animator.SetBool("isGround", false);
                if (HasAnimatorParameter("Jump"))
                {
                    animator.SetTrigger("Jump");
                }
                else
                {
                    return;
                }
            }
        }
    }

    #endregion

    #region Spain & Animation

    protected virtual void SetSpineAnimation(string animationName, bool loop = true)
    {
        if (currentAnimation == animationName && loop)
            return;

        var trackEntry = skeletonAnimation.state.SetAnimation(0, animationName, loop);
        currentAnimation = animationName;

        if (!loop)
        {
            trackEntry.Complete += (trackEntry) =>
            {
                SetSpineAnimation("Idle", true);
                currentAnimation = "Idle";
                isJumping = false;
            };
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isJumping = false;
            if(animator != null)
            {
                animator.SetBool("isGround", true);
            }
        }
    }

    protected bool HasAnimatorParameter(string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }

    #endregion

    #region Attack

    public virtual void ClickAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (skeletonAnimation != null)
            {
                SetSpineAnimation("Attack", false);
                Attack();
            }
            else if (animator != null)
            {
                animator.SetTrigger("Attack");
                Attack();
            }
        }
    }
    public abstract void Attack();

    #endregion
}
