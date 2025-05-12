using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    public float mapSpacing = 5f;       // ���� (X, Y �����ϰ� ����)
    public Transform player;            // �÷��̾� ����
    public int initialMaps = 3;         // ó�� �̸� ������ ���� �� ����
    public int maxActiveMaps = 10;      // Ȱ��ȭ�Ǵ� �� ����

    [Header("Map Prefabs")]
    public GameObject startingMapPrefab; // ���� ��ġ�� ���� �� ������
    public Vector3 startingMapPosition = new Vector3(0f, -4f, 0f); // ���� ���� ��ġ
    public GameObject[] mapPrefabs;      // �������� ������ �� �����յ�

    private Queue<GameObject> activeMaps = new Queue<GameObject>();
    private float nextMapX = 0f;

    void Start()
    {
        SpawnStartingMap();  // ���� �� ����

        // ���� �� 3���� �� �̸� ����
        for (int i = 0; i < initialMaps; i++)
        {
            SpawnRandomMap();
        }
    }

    void Update()
    {
        if (player.position.x >= nextMapX - (mapSpacing * 2))
        {
            SpawnRandomMap();
        }

        if (activeMaps.Count > maxActiveMaps)
        {
            GameObject oldMap = activeMaps.Dequeue();
            Destroy(oldMap);
        }
    }

    void SpawnStartingMap()
    {
        if (startingMapPrefab != null)
        {
            GameObject startMap = Instantiate(startingMapPrefab, startingMapPosition, Quaternion.Euler(-90f, 0f, 0f));
            activeMaps.Enqueue(startMap);

            float mapWidth = GetMapWidth(startMap);
            nextMapX = startingMapPosition.x + mapWidth + mapSpacing;
        }
    }

    void SpawnRandomMap()
    {
        if (mapPrefabs.Length == 0) return;

        GameObject randomMapPrefab = mapPrefabs[Random.Range(0, mapPrefabs.Length)];
        float mapScaleX = randomMapPrefab.transform.localScale.x;

        float randomY = Random.Range(-mapSpacing, mapSpacing);
        Vector3 spawnPos = new Vector3(nextMapX, startingMapPosition.y + randomY, 0);
        GameObject newMap = Instantiate(randomMapPrefab, spawnPos, Quaternion.Euler(-90f, 0f, 0f));

        activeMaps.Enqueue(newMap);
        nextMapX += mapScaleX + mapSpacing;
    }

    float GetMapWidth(GameObject map)
    {
        Renderer[] renderers = map.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return 0f;

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds.size.x;
    }
}
