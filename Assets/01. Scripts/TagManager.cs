using UnityEngine;

public class TagManager : MonoBehaviour
{
    [Header("Player Prefabs")]
    public GameObject[] playerPrefabs; 

    [Header("Camera Prefab")]
    public GameObject cameraPrefab;    

    private GameObject currentPlayer;    
    private GameObject currentCamera;    

    void Start()
    {
        if (playerPrefabs.Length > 0)
        {
            SwitchPlayer(0, new Vector3(0, 2.5f, 0));
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

        if (currentPlayer != null) Destroy(currentPlayer);
        if (currentCamera != null) Destroy(currentCamera);

        currentPlayer = Instantiate(playerPrefabs[index], spawnPosition, Quaternion.identity);

        currentPlayer.transform.SetParent(this.transform);

        if (cameraPrefab != null)
        {
            currentCamera = Instantiate(cameraPrefab);
            currentCamera.transform.SetParent(currentPlayer.transform);
            currentCamera.transform.localPosition = new Vector3(0, 2, -10);
            currentCamera.transform.localRotation = Quaternion.identity;
        }
    }

    public Transform GetCurrentPlayerTransform()
    {
        return currentPlayer?.transform;
    }
}