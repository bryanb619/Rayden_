using UnityEngine;

public class TerminalManager : MonoBehaviour
{   [SerializeField] private SaveManager _saveManager;

    [SerializeField] private GameObject TerminalMenu, PauseMenu;

    [SerializeField] private GameObject Player, Enemies, MenuCamera, abilities;

   


    private void Start()
    {
        TerminalMenu.SetActive(false);
    }
    private void Update()
    {
        MenuState();
    }

    private void MenuState()
    {
        if(TerminalMenu.activeSelf)
        {
            Player.SetActive(false);
            Enemies.SetActive(false);
            abilities.SetActive(false);

            MenuCamera.SetActive(true);
        }
    }

    public void SaveButton()
    {
        _saveManager = FindObjectOfType<SaveManager>();
        _saveManager.SaveGame();
    }

    public void LoadButton()
    {
        _saveManager = FindObjectOfType<SaveManager>();

        _saveManager.LoadGame();
        TerminalMenu.SetActive(false);
        PauseMenu.SetActive(true);
        ResetCursor();


        Player.SetActive(true);
        Enemies.SetActive(true);
        abilities.SetActive(true);
    }

    public void BackButton()
    {
        TerminalMenu.SetActive(false);
        PauseMenu.SetActive(true);
        ResetCursor();

        MenuCamera.SetActive(false);
        Player.SetActive(true);
        Enemies.SetActive(true);
        abilities.SetActive(true);
    }

    private void ResetCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

}
