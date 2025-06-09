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

        // 플레이어 찾기
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
                // 넉백 방향 계산 (플레이어 → 반대 방향)
                Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
                pushDirection.y = 0.5f; // 위쪽으로 살짝 밀기

                playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }
    }
}
