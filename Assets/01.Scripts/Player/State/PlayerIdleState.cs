using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _player.PlayerStat.EnergyUp(false);
        Vector2 input = _player.PlayerInput.PlayerMoveDir;
        if (input.magnitude > 0.05f)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Walk);
        }
    }
}
