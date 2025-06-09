using UnityEngine;
using System.Collections;

public class TagManager : MonoBehaviour
{
    [Header("Player Prefabs")]
    public GameObject[] playerPrefabs;

    [Header("Camera Prefab")]
    public GameObject cameraPrefab;

    private GameObject currentPlayer;
    private GameObject currentCamera;

    private bool isSwitching = false;

    void Start()
    {
        if (playerPrefabs.Length > 0)
        {
            SwitchPlayer(0, new Vector3(0, 5f, 0));
        }
    }

    void Update()
    {
        if (isSwitching) return;

        for (int i = 0; i < playerPrefabs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                StartCoroutine(JumpEffectThenSwitch(i));
            }
        }
    }

    IEnumerator JumpEffectThenSwitch(int index)
    {
        isSwitching = true;

        Vector3 spawnPosition = currentPlayer.transform.position;

        // 점프 연출
        Vector3 originalPos = spawnPosition;
        Vector3 jumpPos = originalPos + Vector3.up * 1.5f;

        float elapsed = 0f;
        float duration = 0.2f;

        while (elapsed < duration)
        {
            currentPlayer.transform.position = Vector3.Lerp(originalPos, jumpPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        currentPlayer.transform.position = jumpPos;
        yield return new WaitForSeconds(0.01f);

        // 플레이어 교체
        SwitchPlayer(index, jumpPos);

        // 착지까지 대기
        yield return StartCoroutine(WaitForLanding());
        isSwitching = false;
    }

    IEnumerator WaitForLanding()
    {
        if (currentPlayer == null) yield break;

        Character character = currentPlayer.GetComponent<Character>();
        if (character == null) yield break;

        // IsGrounded()가 true가 될 때까지 대기
        while (!character.IsGrounded())
        {
            yield return null;
        }
    }

    void SwitchPlayer(int index, Vector3 spawnPosition)
    {
        if (index < 0 || index >= playerPrefabs.Length) return;

        if (currentPlayer != null) Destroy(currentPlayer);
        if (currentCamera != null) Destroy(currentCamera);

        currentPlayer = Instantiate(playerPrefabs[index], spawnPosition, Quaternion.identity);
        currentPlayer.transform.SetParent(this.transform);

        if (cameraPrefab != null)
        {
            currentCamera = Instantiate(cameraPrefab);
            currentCamera.transform.SetParent(currentPlayer.transform);
            currentCamera.transform.localPosition = new Vector3(0, 6.5f, -50);
            currentCamera.transform.localRotation = Quaternion.identity;
        }

        PlayerManager.Instance.CurrentCharacter = currentPlayer;
    }

    public Transform GetCurrentPlayerTransform()
    {
        return currentPlayer?.transform;
    }
}
