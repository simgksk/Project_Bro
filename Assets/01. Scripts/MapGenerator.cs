using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform player;

    [Header("Prefabs")]
    public GameObject startPlatformPrefab;
    public Vector3 startPlatformPosition = new Vector3(-4f, -4f, 1f);
    public GameObject[] platformPrefabs;

    [Header("Setting")]
    public int initialPlatformCount = 5;
    public float removeDistanceBehindPlayer = 20f;
    public float gapBetweenPlatforms = 2f;

    private float nextSpawnX;
    private List<GameObject> activePlatforms = new List<GameObject>();

    void Start()
    {
        if (PlayerManager.Instance?.CurrentCharacter != null)
        {
            player = PlayerManager.Instance.CurrentCharacter.transform;
        }

        InitialSpawn();
    }

    void Update()
    {
        // player�� null�� ���� ����
        if (player == null && PlayerManager.Instance?.CurrentCharacter != null)
        {
            player = PlayerManager.Instance.CurrentCharacter.transform;
        }

        if (player == null) return;

        // �÷��̾ nextSpawnX ������ ���� �÷��� ����
        while (player.position.x + 30f > nextSpawnX)
        {
            SpawnPlatform();
        }

        // �÷��̾� ���ʿ� �־��� �÷��� ����
        for (int i = activePlatforms.Count - 1; i >= 0; i--)
        {
            if (player.position.x - activePlatforms[i].transform.position.x > removeDistanceBehindPlayer)
            {
                Destroy(activePlatforms[i]);
                activePlatforms.RemoveAt(i);
            }
        }
    }

    public void InitialSpawn()
    {
        GameObject startPlatform = Instantiate(startPlatformPrefab, startPlatformPosition, Quaternion.Euler(-90, 180, 0));
        activePlatforms.Add(startPlatform);

        float startLength = GetPlatformWorldLength(startPlatform);
        // ���� �÷��� �� ��ġ�� �ʱ� nextSpawnX ����
        nextSpawnX = startPlatformPosition.x + startLength + gapBetweenPlatforms;

        for (int i = 1; i < initialPlatformCount; i++)
        {
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        GameObject prefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];

        // �ӽ÷� �����ؼ� ���� ����
        GameObject temp = Instantiate(prefab);
        float length = GetPlatformWorldLength(temp);
        Destroy(temp);

        // �� �÷��� ��ġ�� nextSpawnX + (���� ����) ��ŭ �̵�
        float spawnX = nextSpawnX + length / 2f;

        // Y ��ġ ���� ���� (�ʿ信 ���� ����)
        float platformY = Random.Range(-5f, 0f);
        Vector3 spawnPos = new Vector3(spawnX, platformY, 1f);

        GameObject platform = Instantiate(prefab, spawnPos, Quaternion.Euler(-90, 180, 0));
        activePlatforms.Add(platform);

        // ���� �÷��� ���� ���� ��ġ ����: ���� �÷��� �� ���� + gap
        nextSpawnX = spawnX + length / 2f + gapBetweenPlatforms;
    }

    float GetPlatformWorldLength(GameObject platform)
    {
        Renderer[] renderers = platform.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return 5f;

        Bounds combinedBounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
        {
            combinedBounds.Encapsulate(r.bounds);
        }

        Vector3 size = combinedBounds.size;

        // transform�� �������� ����� ���� (x�� ����)
        Vector3 lossyScale = platform.transform.lossyScale;
        float length = size.x * lossyScale.x;

        return Mathf.Abs(length);
    }
}
