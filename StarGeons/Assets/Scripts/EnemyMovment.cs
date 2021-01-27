using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovment : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float speed = 2f;
    [SerializeField] float strength = 5f;
    [SerializeField] float angularSpeed = 120f;
    [SerializeField] float acceleration = 120f;
    [SerializeField] float stoppingDistance = 1f;

    [Header("AI")]
    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float escapeRange = 15f;

    NavMeshAgent navMeshAgent;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        
    }

    void Update()
    {
        setUpAgent();

        handleMovement();
    }

    private void setUpAgent()
    {
        navMeshAgent.speed = speed;
        navMeshAgent.angularSpeed = angularSpeed;
        navMeshAgent.acceleration = acceleration;
        navMeshAgent.stoppingDistance = stoppingDistance;
    }

    private void handleMovement()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
            //navMeshAgent.SetDestination(target.position);
        }

        if (distanceToTarget >= escapeRange)
        {
            isProvoked = false;
        }
    }

    private void EngageTarget()
    {
        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }

        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }
    }

    private void AttackTarget()
    {
        print("attacked target");
        //attack player
    }

    private void ChaseTarget()
    {
        navMeshAgent.SetDestination(target.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, escapeRange);
    }
}
// TODO Provoke enemy