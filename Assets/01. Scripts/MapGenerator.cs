using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlatformData
{
    public GameObject prefab;
    public float length = 5f; // 이 프리팹의 길이 (X축 크기)
}

public class MapGenerator : MonoBehaviour
{
    public Transform player;

    [Header("프리팹 설정")]
    public GameObject startPlatformPrefab;
    public Vector3 startPlatformPosition = new Vector3(-4f, -6f, 1f);
    public float startPlatformLength = 5f;

    public PlatformData[] platformDatas;

    [Header("설정 값")]
    public float platformY = -6f;
    public int initialPlatformCount = 5;
    public float removeDistanceBehindPlayer = 20f;
    public float gapBetweenPlatforms = 2f;

    private float nextSpawnX;
    private List<GameObject> activePlatforms = new List<GameObject>();

    public void Start()
    {
        if (PlayerManager.Instance?.CurrentCharacter != null)
        {
            player = PlayerManager.Instance.CurrentCharacter.transform;
        }

        InitialSpawn();
    }

    public void InitialSpawn()
    {
        // 시작 플랫폼 생성
        GameObject startPlatform = Instantiate(startPlatformPrefab, startPlatformPosition, Quaternion.identity);
        startPlatform.transform.localEulerAngles = new Vector3(-90, 180, 0);
        activePlatforms.Add(startPlatform);

        nextSpawnX = startPlatformPosition.x + startPlatformLength;

        // 나머지 초기 플랫폼 생성
        for (int i = 1; i < initialPlatformCount; i++)
        {
            SpawnPlatform();
        }
    }

    void Update()
    {
        if (PlayerManager.Instance?.CurrentCharacter != null)
        {
            player = PlayerManager.Instance.CurrentCharacter.transform;
        }

        if (player == null) return;

        // 플레이어가 다음 스폰 위치에 가까워졌는지 확인
        while (player.position.x + 30f > nextSpawnX)
        {
            SpawnPlatform();
        }

        // 뒤에 있는 플랫폼 제거
        for (int i = activePlatforms.Count - 1; i >= 0; i--)
        {
            if (player.position.x - activePlatforms[i].transform.position.x > removeDistanceBehindPlayer)
            {
                Destroy(activePlatforms[i]);
                activePlatforms.RemoveAt(i);
            }
        }
    }

    void SpawnPlatform()
    {
        PlatformData data = platformDatas[Random.Range(0, platformDatas.Length)];

        float spawnX = nextSpawnX + data.length / 2f;
        Vector3 spawnPos = new Vector3(spawnX, platformY, 1f);

        GameObject platform = Instantiate(data.prefab, spawnPos, Quaternion.identity);
        platform.transform.localEulerAngles = new Vector3(-90, 180, 0);
        activePlatforms.Add(platform);

        // 다음 위치 갱신: 현재 길이 + 간격
        nextSpawnX += data.length + gapBetweenPlatforms;
    }
}
