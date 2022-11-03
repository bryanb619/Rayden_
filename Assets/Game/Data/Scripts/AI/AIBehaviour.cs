using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using LibGameAI.FSMs;
using URandom = UnityEngine.Random;


// The script that controls an agent using an FSM
public class AIBehaviour : MonoBehaviour
{
    #region Variables
   
    private SavePoints save;

    // Reference to the state machine (FSM)
    private StateMachine _stateMachine;



    [SerializeField] private LayerMask targetMask, obstructionMask;

    [SerializeField] private GameObject playerRef;
    public GameObject PlayerRef => playerRef;

    private bool initiateSearch;

    private bool canSeePlayer;
    public bool CanSeePlayer => canSeePlayer;

    private bool PlayerInMinRange;


    private bool ContinuePatrol;
    private bool RestartPatrol;

    private bool LookingForPlayer;

    private int health = 100;
    public int Health => health;

    private float TimeElapsed = 0;

    private float RetreatUpdateTime;

    // random numbers
    private int RandomNumbers, RandomPatrolNumbers, RandomFleeNumbers;

    // navMesh
    private NavMeshAgent _novas;

    [Header("Waypoint system")]

    // Array of waypoints
    [SerializeField]
    private Transform[] PatrolPoints, SearchPoints;

    // Current waypoint index
    private int destPoint = 0;

    [SerializeField]
    private Transform _guardTarget, _guardTarget2, _guardTarget3, _guardTarget4;

    // retreat position
    [SerializeField]
    private Transform _retreatPos;

    [SerializeField]
    private Transform _findCoverPos, _findCoverPos2, _findCoverPos3;

    [SerializeField] private float MinDist = 16f;

    [Header("Minimal Range Config")]
    [SerializeField] private float sightRange = 4f;

    //private float dist = Vector3.Distance(.position, transform.position);



    [Header("Other variables")]

    // player               // used for look at
    [SerializeField]
    private Transform _player, _enemyNova;

    // Nova enemies
    [SerializeField]
    private GameObject _Enemies;

    // patroling
    private bool LeaveRetreat;

 
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
    [Header("Other events")]
    [SerializeField] private UnityEvent RandomEvent;

    private bool ChasePlayer;


    #endregion Variables


    private void Awake()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");

    }

    //[SerializeField] private GameObject Terminal;
    // Create the FSM
    private void Start()
    {
        ChasePlayer = false;
        LookingForPlayer = false;

        LeaveRetreat = false;
        initiateSearch = false;

        ContinuePatrol = false;
        RestartPatrol = false;

       
        _novas = GetComponent<NavMeshAgent>();

        //PatrolChecker();

        RetreatUpdateTime = 0;
        orginalTime = timeToShoot;

       
        //StartCoroutine(FOVRoutine());

        RandomNumbers = URandom.Range(0, 101);
        RandomPatrolNumbers = URandom.Range(0, 101);

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

        // Retreat State
        State EnemyRetreatState = new State("retreating",
            () => Debug.Log("Entered Retreat State"),
            RetreatUpdate,
            () => Debug.Log("Leaving retreat state"));

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

        // searching state

        State SearchState = new State("Searching",
            () => Debug.Log("Entered Search State"),
            SearchChecker,
            () => Debug.Log("Left Search State"));

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
                () => //(_player.transform.position - transform.position).magnitude <= _minDistToPlayer ||
                    canSeePlayer == true || health <= 99 || ChasePlayer == true,
                () => Debug.Log("player chase"),
                PlayerChaseState));


        // Patrol > Chase player

        PatrolState.AddTransition(
            new Transition(
                () => canSeePlayer == true || health <= 99 || ChasePlayer == true,
                () => Debug.Log("player chase"),
                PlayerChaseState));

        /*
        PlayerChaseState.AddTransition(
            new Transition(
                () => //URandom.value < 0.001f ||
                    (_player.transform.position - transform.position).magnitude
                    >= 10f,
                () => Debug.Log("player chase"),
               PatrolState));

        */

        // Chase player <-> Find cover
        IddleState.AddTransition(
            new Transition(
                () => //URandom.value < 0.001f ||
                      //(_player.transform.position - transform.position).magnitude < _minDistToPlayer,
                    canSeePlayer == true || health <= 99|| ChasePlayer == true,
                () => Debug.Log("player chase"),
                PlayerChaseState));

        /*
        onGuardState.AddTransition(
            new Transition(
                () =>
                    (_player.transform.position - transform.position).magnitude
                    == _FindCoverDist
                    || (_health) <= 60,
                () => Debug.Log("FINDING COVER!"),

        OnCoverState));*/

        // Player Chase <-> Cover State 

        PlayerChaseState.AddTransition(
            new Transition(
                () =>
                     //(_player.transform.position - transform.position).magnitude == _FindCoverDist ||
                     (health) <= 70,
                () => Debug.Log("FINDING COVER!"),
                OnCoverState));

        // Player chase > enemy retreat state

        PlayerChaseState.AddTransition(
            new Transition(
                () => //(_player.transform.position - transform.position).magnitude
                      // < _minChaseDist ||
                   (health) <= 50,
                () => Debug.Log("RETREATING!"),
                EnemyRetreatState));


        // Player Chase <-> Search
        
        PlayerChaseState.AddTransition(
            new Transition(
                () => //URandom.value < 0.001f ||
                      //(_player.transform.position - transform.position).magnitude < _minDistToPlayer,
                     canSeePlayer == false , //
                     //initiateSearch == true,
                () => Debug.Log("AI searching for player"),
                SearchState));


        SearchState.AddTransition(
            new Transition(
                () => //URandom.value < 0.001f ||
                      //(_player.transform.position - transform.position).magnitude < _minDistToPlayer,
                    canSeePlayer == true || ChasePlayer == true,
                () => Debug.Log("AI searching for player"),
                PlayerChaseState));


        // Search -> Patrol State
        SearchState.AddTransition(
            new Transition(
                () => //URandom.value < 0.001f ||
                      //(_player.transform.position - transform.position).magnitude < _minDistToPlayer,
                      //LookingForPlayer == false && canSeePlayer == false &&
                   RestartPatrol == true,
                () => Debug.Log("AI searching for player"),
                PatrolState));
        
        
        // On Retreat > Cover transition
        EnemyRetreatState.AddTransition(
            new Transition(
                () => LeaveRetreat == true,
                () => Debug.Log("Finding cover after retreat"),
                OnCoverState));

        /*
        OnCoverState.AddTransition(
            new Transition(
                () =>
                    (_player.transform.position - transform.position).magnitude
                    <= _avoidPlayerOnCover
                    || (_health) <= 20,
                () => Debug.Log("RETREATING!"),
                EnemyRetreatState));
        */

        #endregion FSM Transitions

        // Create the state machine
        //_stateMachine = new StateMachine(onGuardState);
        _stateMachine = new StateMachine(IddleState);
    }

    

    // Request actions to the FSM and perform them
    private void Update()
    {
        Action actions = _stateMachine.Update();
        actions?.Invoke();

        PIS();
        MinimalRange();



        //print(CanSeePlayer);


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

        /*
        if (RandomPatrolNumbers <= 40)
        {
            _novas.SetDestination(_guardTarget.position);
        }
        
        else if (RandomPatrolNumbers <= 70)
        {
            _novas.SetDestination(_guardTarget2.position);
        }
        else
        {
            _novas.SetDestination(_guardTarget3.position);
        }
        */
    }




    // Player in Sight
    private void PIS()
    {
        save = FindObjectOfType<SavePoints>();

        if (canSeePlayer)
        {
            save.SetSaveFalse();
            _enemyNova.LookAt(_player.transform);

            

        }
        else if(canSeePlayer == false || initiateSearch)
        {
            save.SetSaveTrue();
        }
            
    }

    private void MinimalRange()
    {
        PlayerInMinRange = Physics.CheckSphere(transform.position, sightRange, targetMask);

        if(PlayerInMinRange)
        {
            ChasePlayer = true;
            _enemyNova.LookAt(_player.transform);
           
        }
        else if (!PlayerInMinRange)
        {
            ChasePlayer = false;
            
        }

        //Debug.Log("Is Player in Minimal Ragen?: + "PlayerInMinRange);
    }

    public void FOVChecker(bool canSee)
    {
        if(canSee)
        {
            canSeePlayer = true;
        }
        else if(canSee == false)
        {
            canSeePlayer = false;
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

    private void SearchChecker()
    {
        if (!_novas.pathPending && _novas.remainingDistance < 0.5f)
        {
            SearchUpdate();
        }
    }

    private void SearchUpdate()
    {
        _novas.speed = 2f;

        

        // Returns if no points have been set up
        if (SearchPoints.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        _novas.destination = SearchPoints[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % SearchPoints.Length;

        SearchTimer();

        
    }

    private void SearchTimer()
    {
        float SearchForPlayerTime = 0;

        SearchForPlayerTime += Time.deltaTime;

        if (SearchForPlayerTime >= 40)
        {
            print("Search for player time " + SearchForPlayerTime);
            RestartPatrol = true;

            float ResetTime = 0;
            ResetTime = SearchForPlayerTime;
            print("Search Time "+ SearchForPlayerTime);
        }
        else
        {
            RestartPatrol = false;
        }
    }

    private void ChasePlayerUpdate()
    {
        _novas.speed = 3f;
        _novas.SetDestination(_player.position);
     
        if(_novas.remainingDistance <= 4f)
        {
            _novas.speed = 0f;
        }

        if(CanSeePlayer == false || _novas.remainingDistance >= 16)
        {
            initiateSearch = true;
        }
        else 
            initiateSearch = false;

        Look();
    }

    private void CoverUpdate()
    {
        //detected = true;

        _novas.speed = 4f;

        _novas.SetDestination(_findCoverPos.position);

        if (_novas.remainingDistance < 1)
            Look();
    }

    private void RetreatUpdate()
    {
        //detected = false;

        _novas.SetDestination(_retreatPos.position);

        if (_novas.remainingDistance < 1f)
            Look();

        RetreatUpdateTime += Time.deltaTime;

        if (RetreatUpdateTime >= 5f)
        {
            LeaveRetreat = true;
        }

    }

    private void Look()
    {
        if (canSeePlayer)
        {
            _enemyNova.LookAt(_player.transform);
            Shoot();
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

    private void Shoot()
    {
       

        if (Time.time > nextFire)
        {
            
            nextFire = Time.time + fireRate;

            GameObject currentBullet = Instantiate(bullet, _shootPos.position, _shootPos.rotation);

        }
        else
        {
            // stop anim may not be necessary 
            //enemy.ShootCancel();
        }
        
    }
}