using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using System;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine.AI;

public class AiEntity : MonoBehaviour
{
  
    public enum AttackType
    {
        melee,
        wizzard,
        shooter
    }


    AiStateMachine _stateMachine;
    [Header("Agent Settings")]
    public int _health;
    public AttackType attackType;
    public int _attackPower;
    public Transform _target;
    public float _attackDistance;
    public float _checkRange;
    public LayerMask _checkEnemyLayer;

    public bool _isDeath;
    public bool _isInAttackRange;

    public Animator anim;
    private void Awake()
    {
        _stateMachine = new AiStateMachine(this,_isDeath);
       

       
    }


    private void Start()
    {
        IAiManager idleState = new AiIdleState();
        IAiManager walkState = new aiWalkState();
        IAiManager attackState = new AiAttackState();
        IAiManager DeathState = new AiDeathState();
        _stateMachine.SetNormalStates(idleState, walkState, () => _target!=null);
        _stateMachine.SetNormalStates(walkState, idleState, () => _target == null);
        _stateMachine.SetNormalStates(walkState, attackState, () => _isInAttackRange == true&&_target!=null);
        _stateMachine.SetNormalStates(attackState, walkState, () => _isInAttackRange == false&&_target!=null);
        _stateMachine.SetNormalStates(attackState, idleState, () => _isInAttackRange == false && _target == null);
        _stateMachine.SetAnyStates(DeathState, () => _health <= 0);
        _stateMachine.SetState(idleState);
        
        
    }


    private void Update()
    {
        _stateMachine.Tick();
        SetEnemy();
        if (_health <= 0)
        {
            Death();
        }
    }



    public void SetEnemy()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, _checkRange, _checkEnemyLayer);
        Collider nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        if (col.Length>0)
        {
            foreach (Collider collider in col)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = collider;
                }


            }

        }
        else if(col.Length<=0)
        {
            nearestEnemy = null;
        }

        if (nearestEnemy != null)
        {
            _target = nearestEnemy.transform;
        }
        else
        {
            _target = null;
        }
        

    }


    public void Death()
    {
        if (_isDeath == false)
        {
           
                GetComponent<NavMeshAgent>().isStopped = true;
                anim.SetTrigger("Death");
                Invoke("DeathTimer", 3f);
                _isDeath = true;
                
          
            
        }
    }
    public void DeathTimer()
    {
        Destroy(this.gameObject);
    }

    public void Damage()
    {
        if (_target != null)
        {
            _target.GetComponent<TurretEntity>()._healt -= _attackPower;
        }
    }
}
