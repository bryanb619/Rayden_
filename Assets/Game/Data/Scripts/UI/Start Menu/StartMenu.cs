using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitButton()
    {
        print("Exit App");
        Application.Quit();
    }
}