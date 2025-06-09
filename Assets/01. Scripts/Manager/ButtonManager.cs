using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
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
        SceneManager.LoadScene(0);
    }
    public void Retry_Button()
    {
        SceneManager.LoadScene(1);
    }
}
