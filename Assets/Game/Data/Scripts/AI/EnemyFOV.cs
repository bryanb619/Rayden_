using System.Collections;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    [Header("FOV Settings")]
    [SerializeField][Range(0, 25)] private float radius;
    public float Radius => radius;

    [SerializeField][Range(0, 360)] private float angle;
    public float Angle => angle;

    private bool canSeePlayer;
    public bool CanSeePlayer => canSeePlayer;

    public Transform PlayerRef;

    [SerializeField] private LayerMask targetMask, obstructionMask;
    [SerializeField] private AIBehaviour AI;


    private void Start()
    {
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;

                    AI.FOVChecker(CanSeePlayer);
                }
                    
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
}
