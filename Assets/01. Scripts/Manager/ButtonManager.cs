using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    private void Update()
    {
        Exit();
    }
    public void Start_Button()
    {
        SceneManager.LoadScene(1);
    }
    public void Tutorial_Button()
    {
        SceneManager.LoadScene(2);
    }
    public void Exit_Button()
    {
        Application.Quit();
    }
    void Exit()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void Home_Button()
    {
        PlayerPrefs.SetInt("PlayTitleAnimation", 0);
        SceneManager.LoadScene(0);
    }
    public void Retry_Button()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameManager.Instance.ResetScore();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void Pause_Button()
    {
        GameManager.Instance.TogglePause();
    }
}
