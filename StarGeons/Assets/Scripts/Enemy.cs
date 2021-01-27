using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float speed = 2f;
    [SerializeField] int strength = 5;
    [SerializeField] float acceleration = 8f;
    [SerializeField] float angularSpeed = 120f;
    [SerializeField] float stoppingDistance = 1f;

    [Header("AI")]
    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float escapeRange = 15f;
    [SerializeField] float hitDelay = 0.5f;


    NavMeshAgent navMeshAgent;
    Health enemyHealth;
    PlayerHandler playerHandler;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<Health>();
        playerHandler = FindObjectOfType<PlayerHandler>();
    }

    void Update()
    {
        setUpAgent();

        handleMovement();

        lifeCheck();
    }

    private void handleMovement()
    {
        if (!target) { return; }
        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }

        if (distanceToTarget >= escapeRange)
        {
            isProvoked = false;
        }
    }

    private void setUpAgent()
    {
        navMeshAgent.speed = speed;
        navMeshAgent.angularSpeed = angularSpeed;
        navMeshAgent.acceleration = acceleration;
        navMeshAgent.stoppingDistance = stoppingDistance;
    }

    private void lifeCheck()
    {
        if (enemyHealth.GetHealPoints() <= 0)
        {
            // TODO Death effects (Vfx, SfX, etc...)
            Destroy(gameObject);
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
        print("attaking player");
        //playerHandler.playerTakeHit(strength);
        //TODO play animation, VFX, SFX
    }

    private void ChaseTarget()
    {
        //TODO Play Animation, SFX
        if (!target) { return; }
        navMeshAgent.SetDestination(target.position);
    }

    public void takeHit(int damageTaken)
    {
        isProvoked = true;
        enemyHealth.takeHeal(damageTaken);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, escapeRange);
    }

}