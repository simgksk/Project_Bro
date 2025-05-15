using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move Setting")]
    public float moveSpeed = 5f;

    [Header("Jump Setting")]
    public float jumpForce = 10f;
    public float forwardForce = 5f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    Rigidbody rb;
    Animator animator;
    bool isGrounded;
    bool isJumping;
    bool isRunning;
    bool isAttacking; // 공격 상태 확인용

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GroundCheck();

        // 점프가 진행 중일 때는 이동 불가
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isJumping)
        {
            Jump();
        }

        // 점프 중에는 이동 불가
        if (!isJumping)
        {
            // 이동 처리
            if (Input.GetKey(KeyCode.D))
            {
                MoveRight();
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }
        }

        // 마우스 좌클릭 시 공격 애니메이션 실행
        if (Input.GetMouseButtonDown(0) && !isAttacking) // 좌클릭 시, 공격이 실행되지 않았다면
        {
            Attack();
        }

        // 애니메이션 업데이트
        UpdateAnimationStates();
    }

    private void MoveRight()
    {
        float moveAmount = moveSpeed * Time.deltaTime;
        transform.position += Vector3.right * moveAmount;
    }

    private void Jump()
    {
        rb.velocity = Vector3.zero;

        Vector3 jumpDirection = transform.right * forwardForce + Vector3.up * jumpForce;
        rb.AddForce(jumpDirection, ForceMode.Impulse);

        isJumping = true;
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            isJumping = false;
        }
    }

    private void UpdateAnimationStates()
    {
        if (animator != null)
        {
            // 점프 중에는 Run 애니메이션을 실행하지 않음
            if (isJumping)
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isRunning", false);
            }
            else
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isRunning", isRunning);
            }

            // 공격 상태를 설정
            animator.SetBool("isAttacking", isAttacking); // 공격 중이면 attack을 true로 설정
        }
    }

    private void Attack()
    {
        // 공격 애니메이션 실행
        if (animator != null && !isAttacking)
        {
            isAttacking = true; // 공격 중 상태로 설정
            animator.SetBool("isAttacking", true); // 공격을 시작할 때 attack을 true로 설정
            StartCoroutine(ResetAttackFlag()); // 공격이 끝날 때까지 대기
        }
    }

    // 공격 애니메이션이 끝날 때까지 기다린 후 isAttacking을 리셋하는 코루틴
    private IEnumerator ResetAttackFlag()
    {
        // 공격 애니메이션의 길이를 기다림 (애니메이션 길이를 직접 설정해줘야 함)
        yield return new WaitForSeconds(1f); // 애니메이션 길이에 맞게 조정 (예: 1초)

        isAttacking = false; // 공격이 끝났으므로 공격 상태 리셋
        animator.SetBool("isAttacking", false); // attack을 false로 설정하여 애니메이션을 끝냄
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
