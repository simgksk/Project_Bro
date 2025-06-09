using UnityEngine;

public class Map : MonoBehaviour
{
    [Tooltip("�� ���� ������ ����(������ ��) Transform")]
    public Transform endPoint;  // ������ �� Ʈ������

    [Tooltip("���� ���� ����(���� ��) Transform")]
    public Transform startPoint;  // ���� �� ��ġ �ʿ� (������ �̰� �߰�����)

    /// <summary>
    /// ���ʹ̸� ���� �� �ް�, �� �� ������ ���� ��ġ�� �������� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="enemyPrefabs">��ȯ�� ���ʹ� ������ �迭</param>
    /// <param name="count">��ȯ�� ���ʹ� ����</param>
    public void SpawnEnemies(GameObject[] enemyPrefabs, int count)
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0 || startPoint == null || endPoint == null)
        {
            Debug.LogWarning("Enemy prefabs or start/end points are not set.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            // �������� ������ ����
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // ���� ���۰� �� X ��ǥ ���̿��� ���� X ��ġ ����
            float randomX = Random.Range(startPoint.position.x, endPoint.position.x);

            // �� ����� �����ϰ� Y ��ġ�� startPoint�� endPoint�� Y�� ���� (�ʿ��ϸ� �� ���� ����)
            float groundY = startPoint.position.y;

            Vector3 spawnPos = new Vector3(randomX, groundY, 0f);

            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        }
    }
}
