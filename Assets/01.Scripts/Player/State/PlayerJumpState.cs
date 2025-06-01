using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter(); 
        _player.RigidbodyCompo.AddForce(Vector3.up * _player.jumpPower, ForceMode.VelocityChange);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _player.SetDir(_player.PlayerInput.PlayerMoveDir, _player.defaultSpeed);
        _player.Move();
        
        if (_player.RigidbodyCompo.velocity.y < 0)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Fall);
        }
    }
}
