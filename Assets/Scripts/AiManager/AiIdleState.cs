using UnityEngine;
using UnityEngine.AI;

public class AiIdleState : IAiManager
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
        
        Debug.Log("Idle State");
        agent.GetComponent<NavMeshAgent>().isStopped = true;
       
    }

    
}
