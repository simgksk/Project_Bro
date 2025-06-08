using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] mapPrefabs;    // 사용할 맵 프리팹 배열
    public float mapSpacing = 45f;     // 맵 간격
    public int viewDistance = 5;       // 플레이어 기준으로 왼쪽/오른쪽 유지할 맵 개수

    [Header("Enemy Spawn Settings")]
    public GameObject[] enemyPrefabs;  // 소환할 에너미 프리팹 배열
    public int enemiesPerMap = 3;      // 각 맵당 소환할 적 수

    private Dictionary<int, GameObject> spawnedMaps = new Dictionary<int, GameObject>();
    private bool isFirstGenerate = true;

    void Update()
    {
        UpdateMapChunks();
    }

    void UpdateMapChunks()
    {
        if (PlayerManager.Instance == null || PlayerManager.Instance.CurrentCharacter == null)
            return;

        float playerX = PlayerManager.Instance.CurrentCharacter.transform.position.x;
        int currentChunk;

        if (isFirstGenerate)
        {
            currentChunk = 0;
            isFirstGenerate = false;
        }
        else
        {
            currentChunk = Mathf.FloorToInt(playerX / mapSpacing);
        }

        int minChunk = currentChunk - viewDistance;
        int maxChunk = currentChunk + viewDistance;

        for (int i = minChunk; i <= maxChunk; i++)
        {
            if (!spawnedMaps.ContainsKey(i))
            {
                Vector3 spawnPos = new Vector3(i * mapSpacing, -4f, 2f);  // Y 좌표 -4로 고정
                GameObject prefab = mapPrefabs[Random.Range(0, mapPrefabs.Length)];
                GameObject instance = Instantiate(prefab, spawnPos, Quaternion.identity);
                spawnedMaps.Add(i, instance);

                // 에너미 소환
                SpawnEnemiesOnMap(instance);
            }
        }

        List<int> chunksToRemove = new List<int>();
        foreach (var kvp in spawnedMaps)
        {
            int chunkIndex = kvp.Key;
            if (chunkIndex < minChunk || chunkIndex > maxChunk)
            {
                Destroy(kvp.Value);
                chunksToRemove.Add(chunkIndex);
            }
        }

        foreach (int i in chunksToRemove)
        {
            spawnedMaps.Remove(i);
        }
    }

    void SpawnEnemiesOnMap(GameObject mapInstance)
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        // 맵의 가로 길이를 대략 mapSpacing으로 가정하고, 그 범위 내에서 랜덤 위치에 소환
        float mapStartX = mapInstance.transform.position.x - mapSpacing / 2f;
        float mapEndX = mapInstance.transform.position.x + mapSpacing / 2f;
        float groundY = 5f; // 맵 Y 좌표 기준, 적이 땅 위에 뜨도록 약간 조정

        for (int i = 0; i < enemiesPerMap; i++)
        {
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            float spawnX = Random.Range(mapStartX, mapEndX);
            Vector3 spawnPos = new Vector3(spawnX, groundY, 0f);

            Instantiate(enemyPrefab, spawnPos, Quaternion.identity, mapInstance.transform);
        }
    }
}
