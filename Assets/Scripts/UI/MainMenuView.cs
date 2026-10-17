using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuView : MonoBehaviour
{
    public void OnStartPressed()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
