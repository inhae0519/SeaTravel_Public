using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _player.SetDir(_player.PlayerInput.PlayerMoveDir, _player.defaultSpeed);
        _player.Move();

        if(_player.IsGround)
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        else if(_player.IsWater)
            _stateMachine.ChangeState(PlayerStateEnum.Swim);
    }
}
