using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{

    #region Variables
    public GameObject Power;

    public Transform shootPos;

    private NavMeshAgent agent;

    [SerializeField] private Transform _player;

    public LayerMask Ground, Player;



    // attack
    public float TimeBetweenAttacks = 0.5f;
    bool alreadyattacked;

    public bool PlayerInSight, PlayerInAttackRange;

    // 

    public float sightRange, AttackRange;

    #endregion

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInSight = Physics.CheckSphere(transform.position, sightRange, Player);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, Player);

        // guard
        if(!PlayerInSight && !PlayerInAttackRange)
        {
            Guard();
        }

        // chase
        if (PlayerInSight  && !PlayerInAttackRange)
        {
            ChasePlayer();
        }

        // attack
        if (PlayerInSight && PlayerInAttackRange)
        {
            AttackPlayer(); 
        }
    }


    private void Guard()
    {

    }

    private void ChasePlayer()
    {
        agent.SetDestination(_player.position);
    }
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(_player);
        if(!alreadyattacked)
        {
             Instantiate(Power, shootPos.position, shootPos.rotation);

            alreadyattacked = true;
            Invoke(nameof(ResetAttack), TimeBetweenAttacks);
        }

    }

    private void ResetAttack()
    {
        alreadyattacked = false;
    }
}
