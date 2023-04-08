using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class aiWalkState : IAiManager
{
    public AiEntity agent { get; set; }

    public void Enter()
    {
        
    }

    public void Exit()
    {
        
    }

    public void Tick()
    {
        
        Debug.Log("Walk State");
        float distance = Vector3.Distance(agent.transform.position, agent._target.transform.position);
        agent.GetComponent<NavMeshAgent>().stoppingDistance = agent._attackDistance;
        if (distance > agent._attackDistance)
        {
            agent.GetComponent<NavMeshAgent>().SetDestination(agent._target.position);
            Debug.Log("Set Destination");
            agent.GetComponent<NavMeshAgent>().isStopped = false;
        }
        else
        {
            agent._isInAttackRange = true;
            agent.GetComponent<NavMeshAgent>().isStopped = true;
            
            Debug.Log("isStop");
        }
        agent.transform.GetComponent<Animator>().SetBool("Attack", false);
    }
}
