using UnityEngine;

public class CyberFragments : MonoBehaviour
{
    [SerializeField] GameObject ArchiveUI;

    public void ActivateInUI()
    {
        ArchiveUI.SetActive(true);

        Destroy(gameObject);

        print("i was found");
    }

    


}
