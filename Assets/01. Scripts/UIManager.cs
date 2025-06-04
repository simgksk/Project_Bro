using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
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
}
