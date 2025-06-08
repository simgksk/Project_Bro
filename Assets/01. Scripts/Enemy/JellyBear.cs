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

        player = FindPlayerInLayer();
        if (player == null)
        {
            Debug.LogWarning("플레이어를 찾을 수 없습니다. 레이어 설정을 확인하세요.");
        }

        skeletonAnimation.state.SetAnimation(0, "animation", true);
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

    void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            Vector3 pushDir = (transform.position - collision.transform.position).normalized;
            rb.AddForce(pushDir * pushForce, ForceMode.Impulse);
        }
    }

    GameObject FindPlayerInLayer()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (((1 << obj.layer) & playerLayer) != 0)
            {
                return obj;
            }
        }
        return null;
    }
}
