using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface ITurretManager 
{
   public TurretEntity turretEntity { get; set; }
  
     void Enter();
     void Exit();
     void Tick();
}

public class TurretStateMachine
{
    ITurretManager _currentState;
    readonly List<TurretStateTransition> _stateTransitions;
    readonly List<TurretStateTransition> _anyTransitions;
    TurretEntity _turret;
    

    public TurretStateMachine(TurretEntity turretEntity)
    {
        _stateTransitions = new List<TurretStateTransition>();
        _anyTransitions = new List<TurretStateTransition>();
        _turret = turretEntity;
        
    }

    public void SetState(ITurretManager state)
    {
        _currentState?.Exit();
        _currentState = state;
        _currentState.Enter();
    }

    public void Tick()
    {
        ITurretManager state = CheckState();
        if (state != null)
        {
            SetState(state);
        }

        _currentState.turretEntity = _turret;
        _currentState.Tick();
    }

    private ITurretManager CheckState()
    {
        foreach (var stateTransition in _anyTransitions)
        {
            if (stateTransition.Condition.Invoke())
            {
                return stateTransition.To;
            }
        }

        foreach (var stateTransition in _stateTransitions)
        {
            if (stateTransition.Condition.Invoke() && stateTransition.From.Equals(_currentState))
            {
                return stateTransition.To;
            }
        }
        return null;
    }
    public void SetNormalStates(ITurretManager from,ITurretManager to,System.Func<bool> condition)
    {
        _stateTransitions.Add(new TurretStateTransition(from, to, condition));
    }
    public void SetAnyStates(ITurretManager to,System.Func<bool> condition)
    {
        _anyTransitions.Add(new TurretStateTransition(null, to, condition));
    }
}

public class TurretStateTransition
{
    public ITurretManager From { get; set; }
    public ITurretManager To { get; set; }
    public System.Func<bool> Condition { get; }
    public TurretStateTransition(ITurretManager from,ITurretManager to,System.Func<bool> condition)
    {
        From = from;
        To = to;
        Condition = condition;
    }
}
