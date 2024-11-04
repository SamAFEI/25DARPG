using UnityEngine;

public class PlayerStateRun : PlayerStateGrounded
{
    public PlayerStateRun(Player _entity, EntityFSM _FSM, string _animName) : base(_entity, _FSM, _animName)
    {
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (player.MoveInput == Vector3.zero || !player.CanMovement)
        {
            FSM.SetNextState(player.idleState);
            return;
        }
    }
}
