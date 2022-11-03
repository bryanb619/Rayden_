using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using NaughtyAttributes;

public class PauseMenu : MonoBehaviour
{
    // game paused bool
    [HideInInspector]
    public bool Paused = false;

    // Pause Menu
    [SerializeField]
    private GameObject _pauseMenu;


    [SerializeField]
    private GameObject _SaveLoadMenu;

    // archives Menu
    [SerializeField]
    private GameObject _archiveMenu;

    // options Menu
    [SerializeField]
    private GameObject _optionsMenu;

    [SerializeField] private GameObject AbilityUI, Player, Enemy, MenuCamara, footsteps;

    [SerializeField] private AudioSource Zener1, Zener2;

    private SaveManager _save;

    // Update is called once per frame
    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (Paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if(Time.timeScale == 1f)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

    }

    // method Resume
    private void Resume()
    {
        _pauseMenu.SetActive(false);
        AbilityUI.SetActive(true);
        footsteps.SetActive(true);
        Enemy.SetActive(true);
        //Zener1.UnPause();
        //Zener2.UnPause();

        Time.timeScale = 1f;

        Paused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        Player.SetActive(true);
        MenuCamara.SetActive(false);
    }

    // method pause
    private void Pause()
    {
        AbilityUI.SetActive(false);
        _pauseMenu.SetActive(true);
        Player.SetActive(false);
        MenuCamara.SetActive(true);
        Enemy.SetActive(false);

        //Zener1.Pause();
        //Zener2.Pause();
        footsteps.SetActive(false);

        Time.timeScale = 0f;

        Paused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // buttons
    //[Button]
    public void ResumeButton()
    {
        Resume();
    }

    public void SaveLoadButton()
    {
        _SaveLoadMenu.SetActive(true);
        _pauseMenu.SetActive(false);
    }


    public void DisableSPMenu()
    {
        _pauseMenu.SetActive(false);
        _SaveLoadMenu.SetActive(false);

    }


    public void ArchiveButton()
    {
        _pauseMenu.SetActive(false);
        _archiveMenu.SetActive(true);
    }

    public void OptionsButton()
    {
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(true);
    }

    public void QuitButton()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("Main_Menu");
        Debug.Log("Scene Loaded");
    }

    // archive UI


    // BACK ALL BUTTON
    public void BackButton()
    {
        _archiveMenu.SetActive(false);
        _SaveLoadMenu.SetActive(false);
        _optionsMenu.SetActive(false);

        _pauseMenu.SetActive(true);
      
    }

}