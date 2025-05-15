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
    bool isAttacking; // ���� ���� Ȯ�ο�

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GroundCheck();

        // ������ ���� ���� ���� �̵� �Ұ�
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isJumping)
        {
            Jump();
        }

        // ���� �߿��� �̵� �Ұ�
        if (!isJumping)
        {
            // �̵� ó��
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

        // ���콺 ��Ŭ�� �� ���� �ִϸ��̼� ����
        if (Input.GetMouseButtonDown(0) && !isAttacking) // ��Ŭ�� ��, ������ ������� �ʾҴٸ�
        {
            Attack();
        }

        // �ִϸ��̼� ������Ʈ
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
            // ���� �߿��� Run �ִϸ��̼��� �������� ����
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

            // ���� ���¸� ����
            animator.SetBool("isAttacking", isAttacking); // ���� ���̸� attack�� true�� ����
        }
    }

    private void Attack()
    {
        // ���� �ִϸ��̼� ����
        if (animator != null && !isAttacking)
        {
            isAttacking = true; // ���� �� ���·� ����
            animator.SetBool("isAttacking", true); // ������ ������ �� attack�� true�� ����
            StartCoroutine(ResetAttackFlag()); // ������ ���� ������ ���
        }
    }

    // ���� �ִϸ��̼��� ���� ������ ��ٸ� �� isAttacking�� �����ϴ� �ڷ�ƾ
    private IEnumerator ResetAttackFlag()
    {
        // ���� �ִϸ��̼��� ���̸� ��ٸ� (�ִϸ��̼� ���̸� ���� ��������� ��)
        yield return new WaitForSeconds(1f); // �ִϸ��̼� ���̿� �°� ���� (��: 1��)

        isAttacking = false; // ������ �������Ƿ� ���� ���� ����
        animator.SetBool("isAttacking", false); // attack�� false�� �����Ͽ� �ִϸ��̼��� ����
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
