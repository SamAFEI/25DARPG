public class EntityFSM
{
    public EntityState currentState { get; private set; }

    public void InitState(EntityState _startState)
    {
        currentState = _startState;
        currentState.OnEnter();
    }

    public void ChangeState(EntityState _newState)
    {
        currentState.OnExit();
        currentState = _newState;
        currentState.OnEnter();
    }
}
