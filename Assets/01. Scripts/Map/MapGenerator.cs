using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform player;

    [Header("Prefabs")]
    public GameObject startPlatformPrefab;
    public Vector3 startPlatformPosition = new Vector3(-4f, -4f, 1f);
    public GameObject[] platformPrefabs;

    [Header("Platform Settings")]
    public float removeDistanceBehindPlayer = 20f;

    private float nextSpawnX;
    private List<GameObject> activePlatforms = new List<GameObject>();

    void Start()
    {
        if (PlayerManager.Instance?.CurrentCharacter != null)
        {
            player = PlayerManager.Instance.CurrentCharacter.transform;
        }

        nextSpawnX = startPlatformPosition.x;
    }

    void Update()
    {
        if (player == null && PlayerManager.Instance?.CurrentCharacter != null)
        {
            player = PlayerManager.Instance.CurrentCharacter.transform;
        }

        if (player == null) return;

        while (player.position.x + 30f > nextSpawnX)
        {
            SpawnPlatform();
        }

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
        GameObject prefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
        GameObject platform = Instantiate(prefab, new Vector3(nextSpawnX, startPlatformPosition.y, startPlatformPosition.z), Quaternion.Euler(-90f, 180f, 0f));

        Map map = platform.GetComponent<Map>();
        if (map != null && map.endPoint != null)
        {
            nextSpawnX = map.endPoint.position.x;
        }
        else
        {
            nextSpawnX += 60f; // fallback
        }

        activePlatforms.Add(platform);
    }
}
