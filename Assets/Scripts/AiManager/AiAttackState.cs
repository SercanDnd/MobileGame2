using System;
using UnityEngine;

public class AiAttackState : IAiManager
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
        Debug.Log("Attack State");
        
        float distance = Vector3.Distance(agent.transform.position, agent._target.transform.position);
        if (distance > agent._attackDistance)
        {
            agent._isInAttackRange = false;
        }
        else
        {
            Attack();
        }
    }

    private void Attack()
    {
        switch (agent.attackType)
        {
            case AiEntity.AttackType.melee:
                agent.transform.GetComponent<Animator>().SetBool("Attack", true);
                break;
            case AiEntity.AttackType.wizzard:
                Debug.Log("Wizzard Attack");
                break;
            case AiEntity.AttackType.shooter:
                Debug.Log("Shooter Attack");
                break;

            default:
                Debug.Log("No one attack");
                break;
        }
    }
}
