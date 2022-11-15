using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using LibGameAI.FSMs;


// The script that controls an agent using an FSM
public class AIBehaviour : MonoBehaviour
{
    #region Variables
   
    //private SavePoints save;

    // Reference to the state machine (FSM)
    private StateMachine _stateMachine;



    [SerializeField] private LayerMask targetMask, obstructionMask;

    [SerializeField] private GameObject playerRef;
    public GameObject PlayerRef => playerRef;


    public bool PlayerInSight;


    private bool ContinuePatrol;


    private int health = 100;
    public int Health => health;

    private float TimeElapsed = 0;
    // random numbers


    // navMesh
    private NavMeshAgent _novas;

    [Header("Waypoint system")]

    // Array of waypoints
    [SerializeField]
    private Transform[] PatrolPoints;

    // Current waypoint index
    private int destPoint = 0;

    [SerializeField]
    private Transform _guardTarget, _guardTarget2, _guardTarget3, _guardTarget4;

    [SerializeField]
    private Transform _findCoverPos, _findCoverPos2, _findCoverPos3;


    [Header("Other variables")]

    // player               // used for look at
    [SerializeField]
    private Transform _player, _enemyNova;

    //[SerializeField] Vector3 PlayerPos;

    // Nova enemies
    [SerializeField]
    private GameObject _Enemies;

    // patroling


 
    [Header("Shoot & bullet settings")]
    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Transform _shootPos;

    private float timeToShoot = 1.7f;

    private float fireRate = 2f;
    private float nextFire = 0f;

    private float orginalTime;

    

    //public GameObject deathEffect;
 


    #endregion Variables


    //[SerializeField] private GameObject Terminal;
    // Create the FSM
    private void Start()
    {
        PlayerInSight = false;

        playerRef = GameObject.FindGameObjectWithTag("Player");

        _novas = GetComponent<NavMeshAgent>();

        orginalTime = timeToShoot;

      

        // States

        #region States

        // Guard State
        State IddleState = new State("On Guard",
            () => Debug.Log("Enter On Guard state"),
             IddleUpdate,
            () => Debug.Log("Leave On Guard state"));

        // Player Chase State
        State PlayerChaseState = new State("chasing player",
            () => Debug.Log("Enter Player chase state"),
            ChasePlayerUpdate,
            () => Debug.Log("Leaving Player chase state"));
        // Find cover state
        State OnCoverState = new State("Taking cover",
            () => Debug.Log("Entered Cover State"),
            CoverUpdate,
            () => Debug.Log("Left Cover State"));

        // Patrol state
        State PatrolState = new State(" Patroling",
            () => Debug.Log("Entered Patrol State"),
            PatrolChecker,
            () => Debug.Log("Left Patrol State"));


        #endregion States

        #region FSM Transitions


        // Idle <-> Patrol transitions

        IddleState.AddTransition(
            new Transition(
                () => ContinuePatrol == true,
                () => Debug.Log("Patrol State"),
                PatrolState)); ;

        PatrolState.AddTransition(
            new Transition(
                () => ContinuePatrol == false,
                () => Debug.Log("Patrol State"),
                IddleState));


        // Idle -> Chase Player

        IddleState.AddTransition(
            new Transition(
                () => PlayerInSight == true  || health <= 99,
                () => Debug.Log("player chase"),
                PlayerChaseState));


        // Patrol > Chase player

        PatrolState.AddTransition(
            new Transition(
                () => PlayerInSight == true || health <= 99,
                () => Debug.Log("player chase"),
                PlayerChaseState));

  
        OnCoverState.AddTransition(
            new Transition(
                () => health <= 20,
                () => Debug.Log("attacking!"),
                PlayerChaseState));
        

        #endregion FSM Transitions

        // Create the state machine
        _stateMachine = new StateMachine(IddleState);
    }

    

    // Request actions to the FSM and perform them
    private void Update()
    {
        Action actions = _stateMachine.Update();
        actions?.Invoke();

        //PIS();
        //MinimalRange();
  
        

       // print(PlayerInSight);


    }

    private void IddleUpdate()
    {
        //detected = false;

        TimeElapsed += Time.deltaTime;

        _novas.SetDestination(_guardTarget.position);

        if (TimeElapsed >= 20)
        {
            ContinuePatrol = true;
        }

       
    }




   
    public void FOVChecker(bool canSee)
    {
        if(canSee)
        {
            PlayerInSight = true;
        }
        else if(canSee == false)
        {
            PlayerInSight = false;
        }

    }
    
    private void PatrolChecker()
    {

        if (ContinuePatrol == true)
        {
            TimeElapsed = 0;

            if (!_novas.pathPending && _novas.remainingDistance < 0.5f)
                PatrolUpdate();
        }
    }

    private void PatrolUpdate()
    {
        _novas.autoBraking = false;

        _novas.speed = 1.5f;
        // Returns if no points have been set up
        if (PatrolPoints.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        _novas.destination = PatrolPoints[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % PatrolPoints.Length;
    }

    private void ChasePlayerUpdate()
    {
        _novas.speed = 3f;
        _novas.SetDestination(_player.position);

        if (_novas.remainingDistance <= 4f)
        {
            //_novas.speed = 0f;
            _novas.SetDestination(transform.position);
            //Look();
        }


         Look();
        //Attack();
        //

    }

    private void CoverUpdate()
    {
        //detected = true;

        _novas.speed = 4f;

        _novas.SetDestination(_findCoverPos.position);

        if (_novas.remainingDistance < 1)
        {
            Look();
        }

    }


    private void Look()
    {
        if (PlayerInSight)
        {
            _enemyNova.LookAt(_player.transform);

            Shoot();
        }
    }

    private void Shoot()
    {
        if (Time.time > nextFire)
        {

            nextFire = Time.time + fireRate;

            bullet = Instantiate(bullet, _shootPos.position, _shootPos.rotation);

        }

    }

    public void TakeDamage(int _damage)
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