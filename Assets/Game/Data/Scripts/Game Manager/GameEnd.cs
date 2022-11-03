using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class GameEnd : MonoBehaviour
{
    [SerializeField] private MeshRenderer GreenMesh;

    private void Start()
    {
        //Mesh Rendered Disable
        
        GreenMesh.enabled = false;
    }

    private void OnTriggerEnter(Collider hitInfo)
    {
        //Check for player collision enter

        Player player = hitInfo.GetComponent<Player>();
        if (player != null)
        {
            print("Player entered");
            SceneManager.LoadScene("EndGame");

        }
    }
}
