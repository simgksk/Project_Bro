using UnityEngine;

public class TagManager : MonoBehaviour
{
    [Header("Player Prefabs")]
    public GameObject[] playerPrefabs;  // ����� �÷��̾� �����յ�

    [Header("Camera Prefab")]
    public GameObject cameraPrefab;     // ī�޶� ������

    private GameObject currentPlayer;    // ���� Ȱ��ȭ�� �÷��̾�
    private GameObject currentCamera;    // ���� Ȱ��ȭ�� ī�޶�

    void Start()
    {
        if (playerPrefabs.Length > 0)
        {
            SwitchPlayer(0, new Vector3(0, 2.5f, 0));
        }
        else
        {
            Debug.LogWarning("Player Prefabs�� ����ֽ��ϴ�.");
        }
    }

    void Update()
    {
        for (int i = 0; i < playerPrefabs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                Vector3 currentPosition = currentPlayer != null ? currentPlayer.transform.position : Vector3.zero;
                SwitchPlayer(i, currentPosition);
            }
        }
    }

    void SwitchPlayer(int index, Vector3 spawnPosition)
    {
        if (index < 0 || index >= playerPrefabs.Length) return;

        // ���� �÷��̾�� ī�޶� ����
        if (currentPlayer != null) Destroy(currentPlayer);
        if (currentCamera != null) Destroy(currentCamera);

        // ���ο� �÷��̾� ����
        currentPlayer = Instantiate(playerPrefabs[index], spawnPosition, Quaternion.identity);

        // �÷��̾ Player ������Ʈ�� �ڽ����� ����
        currentPlayer.transform.SetParent(this.transform);

        // ī�޶� ���� �� �÷��̾� ������Ʈ�� �ֱ�
        if (cameraPrefab != null)
        {
            currentCamera = Instantiate(cameraPrefab);
            currentCamera.transform.SetParent(currentPlayer.transform);
            currentCamera.transform.localPosition = new Vector3(0, 2, -10);
            currentCamera.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogWarning("Camera Prefab�� �������� �ʾҽ��ϴ�.");
        }
    }

    public Transform GetCurrentPlayerTransform()
    {
        return currentPlayer?.transform;
    }
}