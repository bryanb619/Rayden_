using UnityEngine;

public class SavePoints : MonoBehaviour
{

    [SerializeField] private GameObject TerminalUI, TerminalParent, ErrorMessage, PauseMenu;

    //private AIBehaviour enemy;
    
    private bool CanSave; 

    // Start is called before the first frame update
    void Start()
    {
        
        TerminalUI.SetActive(false);

        ErrorMessage.SetActive(false);

        //enemy = FindObjectOfType<AIBehaviour>();
        
    }


    public void SetSaveTrue()
    {

        //print("true");

        CanSave = true; 

    }

    public void SetSaveFalse()
    {
        //print("false");
        CanSave = false;
        TerminalParent.SetActive(false);
    }
    

    public void StartTerminal()
    {
        //RunTerminalUI();

    }

    public void RunTerminalUI()
    {
        if (CanSave == false)
        {
            
            Debug.Log("Cant Save");
            //ErrorMessage.SetActive(true);

        }

        else if (CanSave)
        {
            TerminalUI.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            Time.timeScale = 0f;
            PauseMenu.SetActive(false);

            //ErrorMessage.SetActive(false);
        }

        
        
    }

    
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Player" && CanSave == false)
        {
            ErrorMessage.SetActive(true);
        }
        else
        {
            ErrorMessage.SetActive(false);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player" && CanSave == false)
        {
            ErrorMessage.SetActive(false);
        }
        

    }
}
