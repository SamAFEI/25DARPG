
public class PlayerState : EntityState
{
    protected Player player { get; set; }
    public PlayerState(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
        player = _entity;
    }

}
