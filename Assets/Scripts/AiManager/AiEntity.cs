using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using System;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

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
    public GameObject WizzardBulletPrefab;
    public GameObject WizzardUPrefab;
    public GameObject WizzardShootPointRef;
   [SerializeField] int _wizzardShootCounter;
    [SerializeField] float Ydistance;
    private void Awake() =>_stateMachine = new AiStateMachine(this,_isDeath);
    
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


    private void SetEnemy()
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


    private void Death()
    {
        if (_isDeath != false) return;
        GetComponent<NavMeshAgent>().isStopped = true;
        anim.SetTrigger("Death");
        Invoke("DeathTimer", 3f);
        _isDeath = true;
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

    public void TakeDamage(int damage)
    {
        _health -= damage;
    }
    
    public void WizzardShoot()
    {
        
       

        if (_wizzardShootCounter >= 4)
        {
            _wizzardShootCounter=0;
            Vector3 target = new Vector3(_target.position.x, Ydistance, _target.position.z);
            GameObject BulletU = Instantiate(WizzardUPrefab, target,Quaternion.Euler(new Vector3(90,0,0)));
            BulletU.GetComponent<Rigidbody>().AddForce(Vector3.down * 1000);
            



        }
        else
        {
            GameObject bulletGO = Instantiate(WizzardBulletPrefab, WizzardShootPointRef.transform.position,WizzardShootPointRef.transform.rotation); // Mermi objesini olu�turur
            bulletGO.GetComponent<Rigidbody>().AddForce(WizzardShootPointRef.transform.forward * bulletGO.GetComponent<WizzardBullet>().speed);
            WizzardBullet bullet = bulletGO.GetComponent<WizzardBullet>(); // Mermi bile�enine eri�ir
            _wizzardShootCounter++;

            if (bullet != null) // E�er mermi bile�eni varsa
            {
                bullet.Seek(_target);
            }
        }
        
    }
}
