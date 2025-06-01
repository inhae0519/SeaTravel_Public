using UnityEngine;

public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SoundManager.Instance.RoopEffect(_player.walk);

    }

    public override void Exit()
    {
        SoundManager.Instance.RoopEffect(null);
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _player.PlayerStat.EnergyUp(true);
        _player.SetDir(_player.PlayerInput.PlayerMoveDir, _player.defaultSpeed);
        _player.Move();
        
        Vector2 input = _player.PlayerInput.PlayerMoveDir;
        _player.PlayerCam.StartHeadBob();
        
        if (input.magnitude < 0.05f)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
