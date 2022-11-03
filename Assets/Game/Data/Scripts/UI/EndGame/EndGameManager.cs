using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] private AudioSource EndGameSound;

    [SerializeField] private GameObject Screen1, Screen2;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        EndGameSound.Play();

        Screen1.SetActive(true);
        Screen2.SetActive(false);

    }
    public void Resume()
    {
        Screen1.SetActive(false);
        Screen2.SetActive(true);
    }



    public void QuitButton()
    {
        EndGameSound.Stop();

        SceneManager.LoadScene("Main_Menu");

    }

}
