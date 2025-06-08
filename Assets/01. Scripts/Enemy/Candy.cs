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
            // ���� �ִϸ��̼� ��� (�ѹ��� ���, loop �ƴ�)
            skeletonAnimation.state.SetAnimation(0, "Attacke", false);

            // attack �ִϸ��̼� ������ �ٷ� ������ �߻�
            yield return new WaitForSeconds(laserDuration);

            // ������ ����
            if (laserPrefab != null && laserSpawnPoint != null)
            {
                GameObject laser = Instantiate(laserPrefab, laserSpawnPoint.position, laserSpawnPoint.rotation);
                Destroy(laser, laserDuration); // ������ ���� �ð� �� ����
            }

            // ���� �ִϸ��̼� ���� �� �ٽ� Idle �ִϸ��̼� ������ ����
            skeletonAnimation.state.SetAnimation(0, "Idle", true);

            // ��� �ð� (������ ��� ���� �ð�)
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
