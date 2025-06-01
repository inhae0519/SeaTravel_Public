using UnityEngine;

public class PlayerSwimState : PlayerState
{
    public PlayerSwimState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.RigidbodyCompo.useGravity = false;
        UIManager.Instance.EnableOxygenBar(true);
        SoundManager.Instance.RoopEffect(_player.swim);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _player.SetDir(_player.PlayerInput.PlayerMoveDir, _player.defaultSpeed);
        if (_player.PlayerInput.IsSwimPressed)
            _player.Swim();
        else
            _player.Move(true);
        
        if (!_player.IsWater)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Fall);
        }
    }

    public override void Exit()
    {
        SoundManager.Instance.RoopEffect(null);
        _player.RigidbodyCompo.useGravity = true;
        base.Exit();
    }
}
