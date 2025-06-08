using Spine.Unity;
using System.Collections;
using UnityEngine;

public class Candy : MonoBehaviour
{
    [Header("Spine & Animation Setting")]
    public SkeletonAnimation skeletonAnimation;

    [Header("Player Settings")]
    public LayerMask playerLayer;
    public float detectDistance = 5f;
    private GameObject player;

    [Header("Laser Settings")]
    public GameObject laserPrefab;
    public Transform laserSpawnPoint;
    public float laserDuration = 1f;
    public float laserCooldown = 2f;

    private bool isFiring = false;

    private Rigidbody rb;

    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.state.SetAnimation(0, "Idle", true);

        rb = GetComponent<Rigidbody>();
        player = FindPlayerInLayer();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= detectDistance && !isFiring)
        {
            StartCoroutine(FireLaserLoop());
        }

        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator FireLaserLoop()
    {
        isFiring = true;

        while (Vector3.Distance(transform.position, player.transform.position) <= detectDistance)
        {
            // 공격 애니메이션 재생 (한번만 재생, loop 아님)
            skeletonAnimation.state.SetAnimation(0, "Attacke", false);

            // attack 애니메이션 끝나고 바로 레이저 발사
            yield return new WaitForSeconds(laserDuration);

            // 레이저 생성
            if (laserPrefab != null && laserSpawnPoint != null)
            {
                GameObject laser = Instantiate(laserPrefab, laserSpawnPoint.position, laserSpawnPoint.rotation);
                Destroy(laser, laserDuration); // 레이저 지속 시간 후 삭제
            }

            // 공격 애니메이션 끝난 후 다시 Idle 애니메이션 루프로 복귀
            skeletonAnimation.state.SetAnimation(0, "Idle", true);

            // 대기 시간 (레이저 쏘고 쉬는 시간)
            yield return new WaitForSeconds(laserCooldown);
        }

        isFiring = false;
    }

    GameObject FindPlayerInLayer()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (((1 << obj.layer) & playerLayer) != 0)
                return obj;
        }
        return null;
    }
}
