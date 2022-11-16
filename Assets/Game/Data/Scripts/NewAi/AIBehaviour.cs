using System;
using UnityEngine;
using System.Collections;
//using URandom = UnityEngine.Random;
using LibGameAI.FSMs;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

// The script that controls an agent using an FSM
[RequireComponent(typeof(NavMeshAgent))]
public class AIBehaviour : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    private float damage;
    public float _damage => damage;

    private float health;

    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;
    public Transform Player;


    public LayerMask targetMask;
    public LayerMask obstructionMask;

    private bool canSeePlayer;
    public bool CanSee => canSeePlayer;


    [Header("Shoot & bullet settings")]
    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Transform _shootPos;

    private float fireRate = 2f;
    private float nextFire = 0f;

    // Reference to the state machine
    private StateMachine stateMachine;

    // Get references to enemies
   
    #region Start & States
    // Create the FSM
    private void Start()
    {
        
        health = 100f;
        canSeePlayer = false;
        //playerRef = GameObject.FindGameObjectWithTag("Player");

        agent = GetComponent<NavMeshAgent>();   
        
        StartCoroutine(FOVRoutine());

        // Create the states
        State onGuardState = new State("On Guard",
            () => Debug.Log("Enter On Guard state"),
            null,
            () => Debug.Log("Leave On Guard state"));

        State fightState = new State("Fight",
            () => Debug.Log("Enter Fight state"),
            ChasePlayer,
            () => Debug.Log("Leave Fight state"));

        // Idle - Fight

        onGuardState.AddTransition(
            new Transition(
            () => canSeePlayer == true,
            () => Debug.Log("PLAYER FOUND"), 
            fightState));
        
        // Create the state machine
        stateMachine = new StateMachine(onGuardState);
    }
    #endregion

    // Request actions to the FSM and perform them
    private void Update()
    {
        Action actions = stateMachine.Update();
        actions?.Invoke();
        
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
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    // Chase the small enemy
    private void ChasePlayer()
    {
        agent.speed = 4f;

        if(canSeePlayer == true)
        {
            agent.SetDestination(Player.position);
            Attack();

            if (agent.remainingDistance <= 5f)
            {
                agent.speed = 0f;
                //agent.SetDestination(transform.position);
            }
        }

    }

    private void Attack()
    {
        transform.LookAt(Player.position);

        //print("attack in progress");

        if(Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(bullet, _shootPos.position, _shootPos.rotation);
        }




    }

    public void TakeDamage(float _damage)
    {
        health -= _damage;
        Debug.Log("enemy shot");
        if (health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        //Instantiate(transform.position, Quaternion.identity);
        Destroy(gameObject);

        // call for AI event
        //DieEvent.Invoke();

        Debug.Log("Enemy died");
    }
}
