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
            // 애니메이션: 공격과 동시에 레이저
            skeletonAnimation.state.SetAnimation(0, "Attacke", false);

            if (laserPrefab != null && laserSpawnPoint != null)
            {
                Vector3 direction = (player.transform.position - laserSpawnPoint.position).normalized;
                float distance = Vector3.Distance(player.transform.position, laserSpawnPoint.position);

                // 레이저 생성
                GameObject laser = Instantiate(
                    laserPrefab,
                    laserSpawnPoint.position,
                    Quaternion.LookRotation(direction)
                );

                // 레이저 크기 조정 (Z축으로 길게)
                laser.transform.localScale = new Vector3(0.1f, 0.1f, distance);

                // 중심 피벗 보정: 앞으로 반만큼 이동
                laser.transform.position += laser.transform.forward * (distance / 2f);

                // 일정 시간 후 삭제
                Destroy(laser, laserDuration);
            }

            // laserDuration 후 Idle로 전환
            StartCoroutine(ResetToIdleAfter(laserDuration));

            yield return new WaitForSeconds(laserDuration + laserCooldown);
        }

        isFiring = false;
    }

    IEnumerator ResetToIdleAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        skeletonAnimation.state.SetAnimation(0, "Idle", true);
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
