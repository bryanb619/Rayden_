using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class SoundHandler : MonoBehaviour
{

    [SerializeField] private MeshRenderer GreenMesh;
    [SerializeField] private AudioSource GameMusic;
    //public SaveManager _saveManager;

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

            GameMusic.Stop();

        }
    }
}
