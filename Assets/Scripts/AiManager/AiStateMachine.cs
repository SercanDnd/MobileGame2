using System.Collections.Generic;

public class AiStateMachine
{
    IAiManager _currenState;
    readonly List<AiStateTransition> _stateTransitions;
    readonly List<AiStateTransition> _anyTransitions;
    AiEntity _agent;
    bool _isDeath;
    public AiStateMachine(AiEntity agent,bool isDeath)
    {
        _stateTransitions = new List<AiStateTransition>();
        _anyTransitions = new List<AiStateTransition>();
        _agent = agent;
        _isDeath = isDeath;
    }
    
    public void SetState(IAiManager state)
    {
        _currenState?.Exit();
        _currenState = state;
        _currenState.Enter();
    }

    public void Tick()
    {
        IAiManager state = CheckState();

        if (state != null)
        {
            SetState(state);
        }

        _currenState.agent = _agent;
        _currenState.Tick();
    }

    private IAiManager CheckState()
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
            if (stateTransition.Condition.Invoke() && stateTransition.From.Equals(_currenState))
            {
                return stateTransition.To;
            }
        }

        return null;
    }

    public void SetNormalStates(IAiManager from,IAiManager to,System.Func<bool> condition)
    {
        _stateTransitions.Add(new AiStateTransition(from, to, condition));
    }

    public void SetAnyStates(IAiManager to,System.Func<bool> condition)
    {
        _anyTransitions.Add(new AiStateTransition(null,to,condition));
    }

   
}
