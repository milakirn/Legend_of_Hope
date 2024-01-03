using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour : StateMachineBehaviour
{
    private float timer;
    private float chaseRange = 10;
    private List<Transform> points = new List<Transform>();
    private NavMeshAgent agent;
    private Transform player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        Transform pointsObject = GameObject.FindGameObjectWithTag("Points").transform;
        foreach (Transform t in pointsObject)
            points.Add(t);

        agent = animator.GetComponent<NavMeshAgent>();
        agent.SetDestination(points[0].position);

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Debug.Log("Set new point destination");
            agent.SetDestination(points[Random.Range(0, points.Count)].position);
        }

        timer += Time.deltaTime;
        if (timer > 10)
            animator.SetBool("isPatrolling", false);

        if (player != null)
        {
            float distance = Vector3.Distance(animator.transform.position, player.position);
            if (distance < chaseRange)
                animator.SetBool("isChasing", true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }
}