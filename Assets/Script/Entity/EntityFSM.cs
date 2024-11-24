public class EntityFSM
{
    public EntityState currentState { get; private set; }
    public EntityState nextState { get; private set; }

    public void InitState(EntityState _startState)
    {
        nextState = _startState;
        currentState = _startState;
        currentState.OnEnter();
    }

    public void SetNextState(EntityState _nextState)
    {
        nextState = _nextState;
    }

    public void ChangeNextState()
    {
        if (currentState == nextState) { return; }
        currentState.OnExit();
        currentState = nextState;
        currentState.OnEnter();
    }
}
