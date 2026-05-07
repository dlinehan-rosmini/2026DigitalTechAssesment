using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int menuSceneIndex;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1;
    }

    public void RestartMission()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void QuitToMenu()
    {
        SceneManager.LoadScene(menuSceneIndex);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
