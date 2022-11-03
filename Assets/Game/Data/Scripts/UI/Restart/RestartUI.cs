using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartUI : MonoBehaviour
{
    
    [SerializeField] private SaveManager _saveManager;

    [SerializeField] private GameObject Player, Abilities, Enemies, MenuCamera, PauseMenu, UI, walksSteps;
    // Start is called before the first frame update
    void Start()
    {
        

        UI.SetActive(false);
    }

    private void Update()
    {
        if(UI.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            MenuCamera.SetActive(true);

            Player.SetActive(false);
            Enemies.SetActive(false);
            Abilities.SetActive(false);
            PauseMenu.SetActive(false);
            walksSteps.SetActive(false);
        }
    

    }

    public void LoadGameButton()
    {
        Player.SetActive(true);
        Enemies.SetActive(true);
        Abilities.SetActive(true);
        PauseMenu.SetActive(true);
        walksSteps.SetActive(true);

        MenuCamera.SetActive(false);
        UI.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;


        _saveManager.LoadGame();

     
        Debug.Log("Loaded last saved Game");
    }

    public void Quit()
    {
       
        SceneManager.LoadScene("Main_Menu");
    }
}
