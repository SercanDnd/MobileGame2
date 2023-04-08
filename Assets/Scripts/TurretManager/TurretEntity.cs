using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEntity : MonoBehaviour
{
    public enum States
    {
        passive,
        active,
        attacking,
        notAttacking
    }


    TurretStateMachine _stateMachine;
    [Header("Turret Settings")]
    public GameObject _activedTurret;
    public int _healt;
    public States TurretState { get; set; }
    public string _stateDebug;
    public bool _isActive;
    private void Awake()
    {
        _stateMachine = new TurretStateMachine(this);
    }

    private void Start()
    {
        ITurretManager passiveState = new TurretPassiveState();
        ITurretManager activeState = new TurretActiveState();

        _stateMachine.SetNormalStates(passiveState, activeState, () => _isActive == true);
        _stateMachine.SetNormalStates(activeState, passiveState, () => _isActive == false);
        // _stateMachine.SetAnyStates(passiveState, () => true);

        _stateMachine.SetState(passiveState);
    }

    public void Update()
    {
        _stateMachine.Tick();
        StateDebug();
        CheckActivity();
    }

    public void StateDebug()
    {
        _stateDebug = TurretState.ToString();
    }

    public void CheckActivity()
    {
        if (_activedTurret != null)
        {
            _isActive = true;
        }
        else
        {
            _isActive = false;
        }
    }
}

public class TurretPassiveState : ITurretManager
{
    public TurretEntity turretEntity { get; set; }
    
   

    public void Enter()
    {
      
    }

    public void Exit()
    {
       
    }

    public void Tick()
    {
        Debug.Log($"Turret Passive : {turretEntity.TurretState}");
        turretEntity.TurretState = TurretEntity.States.passive;
    }
}

public class TurretActiveState : ITurretManager
{
    public TurretEntity turretEntity { get; set; }

    public void Enter()
    {
        
    }

    public void Exit()
    {
        
    }

    public void Tick()
    {
        turretEntity.TurretState = TurretEntity.States.active;
    }
}
