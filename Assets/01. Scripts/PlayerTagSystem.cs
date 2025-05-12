using UnityEngine;

public class PlayerTagSystem : MonoBehaviour
{
    [Header("Player Prefabs")]
    public GameObject[] playerPrefabs;  // 사용할 플레이어 프리팹들

    [Header("Camera Prefab")]
    public GameObject cameraPrefab;     // 카메라 프리팹

    private GameObject currentPlayer;    // 현재 활성화된 플레이어
    private GameObject currentCamera;    // 현재 활성화된 카메라

    void Start()
    {
        if (playerPrefabs.Length > 0)
        {
            SwitchPlayer(0, new Vector3(0, 2.5f, 0));
        }
        else
        {
            Debug.LogWarning("Player Prefabs가 비어있습니다.");
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

    /// <summary>
    /// 플레이어를 교체한다.
    /// </summary>
    /// <param name="index">교체할 플레이어 인덱스</param>
    /// <param name="spawnPosition">새로운 플레이어의 위치</param>
    void SwitchPlayer(int index, Vector3 spawnPosition)
    {
        if (index < 0 || index >= playerPrefabs.Length) return;

        // 기존 플레이어와 카메라 삭제
        if (currentPlayer != null) Destroy(currentPlayer);
        if (currentCamera != null) Destroy(currentCamera);

        // 새로운 플레이어 생성
        currentPlayer = Instantiate(playerPrefabs[index], spawnPosition, Quaternion.identity);

        // 플레이어를 Player 오브젝트의 자식으로 설정
        currentPlayer.transform.SetParent(this.transform);

        // 카메라 생성 및 플레이어 오브젝트에 넣기
        if (cameraPrefab != null)
        {
            currentCamera = Instantiate(cameraPrefab);
            currentCamera.transform.SetParent(currentPlayer.transform);
            currentCamera.transform.localPosition = new Vector3(0, 2, -10);
            currentCamera.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogWarning("Camera Prefab이 설정되지 않았습니다.");
        }
    }

    /// <summary>
    /// 현재 플레이어의 Transform 반환
    /// </summary>
    /// <returns></returns>
    public Transform GetCurrentPlayerTransform()
    {
        return currentPlayer?.transform;
    }
}
