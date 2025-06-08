using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] mapPrefabs;    // ����� �� ������ �迭
    public float mapSpacing = 45f;     // �� ����
    public int viewDistance = 5;       // �÷��̾� �������� ����/������ ������ �� ����

    [Header("Enemy Spawn Settings")]
    public GameObject[] enemyPrefabs;  // ��ȯ�� ���ʹ� ������ �迭
    public int enemiesPerMap = 3;      // �� �ʴ� ��ȯ�� �� ��

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
                Vector3 spawnPos = new Vector3(i * mapSpacing, -4f, 2f);  // Y ��ǥ -4�� ����
                GameObject prefab = mapPrefabs[Random.Range(0, mapPrefabs.Length)];
                GameObject instance = Instantiate(prefab, spawnPos, Quaternion.identity);
                spawnedMaps.Add(i, instance);

                // ���ʹ� ��ȯ
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

        // ���� ���� ���̸� �뷫 mapSpacing���� �����ϰ�, �� ���� ������ ���� ��ġ�� ��ȯ
        float mapStartX = mapInstance.transform.position.x - mapSpacing / 2f;
        float mapEndX = mapInstance.transform.position.x + mapSpacing / 2f;
        float groundY = 5f; // �� Y ��ǥ ����, ���� �� ���� �ߵ��� �ణ ����

        for (int i = 0; i < enemiesPerMap; i++)
        {
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            float spawnX = Random.Range(mapStartX, mapEndX);
            Vector3 spawnPos = new Vector3(spawnX, groundY, 0f);

            Instantiate(enemyPrefab, spawnPos, Quaternion.identity, mapInstance.transform);
        }
    }
}
