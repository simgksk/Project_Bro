using UnityEngine;

public class Map : MonoBehaviour
{
    [Tooltip("이 맵의 마지막 지점(오른쪽 끝) Transform")]
    public Transform endPoint;  // 마지막 땅 트랜스폼

    [Tooltip("맵의 시작 지점(왼쪽 끝) Transform")]
    public Transform startPoint;  // 왼쪽 끝 위치 필요 (없으면 이걸 추가해줘)

    /// <summary>
    /// 에너미를 여러 개 받고, 맵 땅 위에서 랜덤 위치에 랜덤으로 소환하는 함수
    /// </summary>
    /// <param name="enemyPrefabs">소환할 에너미 프리팹 배열</param>
    /// <param name="count">소환할 에너미 개수</param>
    public void SpawnEnemies(GameObject[] enemyPrefabs, int count)
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0 || startPoint == null || endPoint == null)
        {
            Debug.LogWarning("Enemy prefabs or start/end points are not set.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            // 랜덤으로 프리팹 선택
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // 맵의 시작과 끝 X 좌표 사이에서 랜덤 X 위치 생성
            float randomX = Random.Range(startPoint.position.x, endPoint.position.x);

            // 땅 위라고 가정하고 Y 위치는 startPoint나 endPoint의 Y로 설정 (필요하면 땅 높이 조정)
            float groundY = startPoint.position.y;

            Vector3 spawnPos = new Vector3(randomX, groundY, 0f);

            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        }
    }
}
