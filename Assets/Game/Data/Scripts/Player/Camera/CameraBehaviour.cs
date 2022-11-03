using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform referenceTransform;
    public float collisionOffset = 0.3f; //To prevent Camera from clipping through Objects
    public float cameraSpeed = 15f; //How fast the Camera should snap into position if there are no obstacles

    private Vector3 defaultPos;
    private Vector3 directionNormalized;
    private Transform parentTransform;
    private float defaultDistance;

    // Start is called before the first frame update
    private void Start()
    {
        defaultPos = transform.localPosition;
        directionNormalized = defaultPos.normalized;
        parentTransform = transform.parent;
        defaultDistance = Vector3.Distance(defaultPos, Vector3.zero);

        //Lock cursor
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // LateUpdate is called after Update
    private void LateUpdate()
    {
        Vector3 currentPos = defaultPos;

        RaycastHit hit;

        Vector3 dirTmp = parentTransform.TransformPoint(defaultPos) - referenceTransform.position;
        if (Physics.SphereCast(referenceTransform.position, collisionOffset, dirTmp, out hit, defaultDistance))
        {
            currentPos = (directionNormalized * (hit.distance - collisionOffset));

            transform.localPosition = currentPos;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, currentPos, Time.deltaTime * cameraSpeed);
        }
    }
}