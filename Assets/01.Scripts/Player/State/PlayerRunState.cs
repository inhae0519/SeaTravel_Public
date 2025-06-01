using UnityEngine;

public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerCam.RunCameraZoom(true);
        SoundManager.Instance.RoopEffect(_player.run);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _player.SetDir(_player.PlayerInput.PlayerMoveDir, _player.runSpeed);
        _player.Move();

        Vector2 input = _player.PlayerInput.PlayerMoveDir;
        _player.PlayerCam.StartHeadBob();

        if (!_player.PlayerInput.IsRunPressed || input.magnitude < 0.05f || _player.PlayerStat.ReturnCurrentEnergy() <= 0)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _player.PlayerCam.RunCameraZoom(false);
        SoundManager.Instance.RoopEffect(null);
    }
}
