using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Character : MonoBehaviour
{
    [Header("Move Setting")]
    protected float moveDistance = 3f;
    protected float moveDuration = 0.15f;
    protected bool isMoving = false;
    private Coroutine moveRoutine;

    [Header("Jump Setting")]
    protected float jumpForce = 7f;
    public float jumpPower = 1f;
    private Rigidbody rb;

    [SerializeField] private int maxJumpCount = 2; // 이중 점프까지 허용
    private int jumpCount = 0;

    [Header("Spine & Animation Setting")]
    public SkeletonAnimation skeletonAnimation;
    private Animator animator;
    private string currentAnimation = "";

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
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
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!isMoving)
            {
                moveRoutine = StartCoroutine(MoveCourutine());
            }
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
        Vector3 start = rb.position;
        Vector3 target = rb.position + Vector3.right * moveDistance;

        while (t < moveDuration)
        {
            yield return new WaitForFixedUpdate();
            t += Time.fixedDeltaTime;
            Vector3 newPos = Vector3.Lerp(start, target, t / moveDuration);
            rb.MovePosition(newPos);
        }

        isMoving = false;
        rb.linearVelocity = Vector3.zero;
        moveRoutine = null;
    }

    public virtual void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && jumpCount < maxJumpCount)
        {
            jumpCount++;
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
                    animator.SetTrigger("Jump");
            }
        }
    }

    protected virtual void SetSpineAnimation(string animationName, bool loop = true)
    {
        if (currentAnimation == animationName && loop) return;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
            if (animator != null)
            {
                animator.SetBool("isGround", true);
            }
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            // 이동 중이면 중단
            if (moveRoutine != null)
            {
                StopCoroutine(moveRoutine);
                moveRoutine = null;
                isMoving = false;
            }

            Vector3 pushBack = -transform.right * 3f + Vector3.up * 1f;
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(pushBack, ForceMode.Impulse);
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

    // 외부에서 착지 상태 확인용
    public bool IsGrounded()
    {
        return jumpCount == 0;
    }
}
