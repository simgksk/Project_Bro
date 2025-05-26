using Spine;
using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Character : MonoBehaviour
{
    [Header("Move Setting")]
    protected float moveSpeed = 5f;
    protected float jumpForce = 7f;

    [Header("Jump Setting")]
    public float jumpPower = 1f;
    private Rigidbody rb;
    bool isJumping = false;

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
        bool isMoving = Input.GetKey(KeyCode.D);

        if (isMoving)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }

        if(isJumping)
        {
            return;
        }

        if (skeletonAnimation != null)
        {
            if (currentAnimation != "Run")
                SetSpineAnimation("Run");
        }
        else if (animator != null)
        {
            animator.SetBool("isRunning", isMoving);
        }
    }
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
            };
        }
    }

    #endregion

    #region Jump
    public virtual void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            isJumping = true;

            rb.velocity = Vector3.zero;

            Vector3 jumpDirection = Vector3.up * jumpForce;
            rb.AddForce(jumpDirection, ForceMode.Impulse);

            if (skeletonAnimation != null)
            {
                SetSpineAnimation("Jump", false);
            }
            else if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
        }
    }

    #endregion

    #region Attack

    public virtual void ClickAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }
    public abstract void Attack();

    #endregion
}
