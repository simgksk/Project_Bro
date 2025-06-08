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
            // �ִϸ��̼�: ���ݰ� ���ÿ� ������
            skeletonAnimation.state.SetAnimation(0, "Attacke", false);

            if (laserPrefab != null && laserSpawnPoint != null)
            {
                Vector3 direction = (player.transform.position - laserSpawnPoint.position).normalized;
                float distance = Vector3.Distance(player.transform.position, laserSpawnPoint.position);

                // ������ ����
                GameObject laser = Instantiate(
                    laserPrefab,
                    laserSpawnPoint.position,
                    Quaternion.LookRotation(direction)
                );

                // ������ ũ�� ���� (Z������ ���)
                laser.transform.localScale = new Vector3(0.1f, 0.1f, distance);

                // �߽� �ǹ� ����: ������ �ݸ�ŭ �̵�
                laser.transform.position += laser.transform.forward * (distance / 2f);

                // ���� �ð� �� ����
                Destroy(laser, laserDuration);
            }

            // laserDuration �� Idle�� ��ȯ
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
