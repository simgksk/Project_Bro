using Spine.Unity;
using UnityEngine;

public class JellyBear : MonoBehaviour
{
    [Header("Spine & Animation Setting")]
    public SkeletonAnimation skeletonAnimation;

    [Header("Player Detection Settings")]
    public LayerMask playerLayer;
    public float attackDistance = 5f;
    private GameObject player;
    private bool isAttacking = false;

    [Header("Push Settings")]
    public float pushForce = 2f;

    private Rigidbody rb;

    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        rb = GetComponent<Rigidbody>();

        skeletonAnimation.state.SetAnimation(0, "animation", true);

        // �÷��̾� ã��
        player = GameObject.FindGameObjectWithTag("Player");

        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= attackDistance && !isAttacking)
        {
            isAttacking = true;
            skeletonAnimation.state.SetAnimation(0, "attack", true);
        }
        else if (distance > attackDistance && isAttacking)
        {
            isAttacking = false;
            skeletonAnimation.state.SetAnimation(0, "animation", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // �˹� ���� ��� (�÷��̾� �� �ݴ� ����)
                Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
                pushDirection.y = 0.5f; // �������� ��¦ �б�

                playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }
    }
}
