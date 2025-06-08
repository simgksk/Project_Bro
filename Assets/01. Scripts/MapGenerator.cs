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
        // player가 null일 때만 갱신
        if (player == null && PlayerManager.Instance?.CurrentCharacter != null)
        {
            player = PlayerManager.Instance.CurrentCharacter.transform;
        }

        if (player == null) return;

        // 플레이어가 nextSpawnX 가까이 오면 플랫폼 생성
        while (player.position.x + 30f > nextSpawnX)
        {
            SpawnPlatform();
        }

        // 플레이어 뒤쪽에 멀어진 플랫폼 제거
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
        // 시작 플랫폼 끝 위치로 초기 nextSpawnX 설정
        nextSpawnX = startPlatformPosition.x + startLength + gapBetweenPlatforms;

        for (int i = 1; i < initialPlatformCount; i++)
        {
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        GameObject prefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];

        // 임시로 생성해서 길이 측정
        GameObject temp = Instantiate(prefab);
        float length = GetPlatformWorldLength(temp);
        Destroy(temp);

        // 새 플랫폼 위치는 nextSpawnX + (길이 절반) 만큼 이동
        float spawnX = nextSpawnX + length / 2f;

        // Y 위치 랜덤 설정 (필요에 따라 조정)
        float platformY = Random.Range(-5f, 0f);
        Vector3 spawnPos = new Vector3(spawnX, platformY, 1f);

        GameObject platform = Instantiate(prefab, spawnPos, Quaternion.Euler(-90, 180, 0));
        activePlatforms.Add(platform);

        // 다음 플랫폼 생성 기준 위치 갱신: 현재 플랫폼 끝 지점 + gap
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

        // transform의 스케일을 고려한 길이 (x축 방향)
        Vector3 lossyScale = platform.transform.lossyScale;
        float length = size.x * lossyScale.x;

        return Mathf.Abs(length);
    }
}
