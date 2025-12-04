using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseButton : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
