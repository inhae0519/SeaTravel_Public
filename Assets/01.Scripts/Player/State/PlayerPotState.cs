using UnityEngine;

public class PlayerPotState : PlayerState
{
    public PlayerPotState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    
    private float _comboWindow = 1f; // 키를 누른 이후 다시 키를 누르기까지 대기시간
    private Pot _pot;

    public override void Enter()
    {
        base.Enter();
        _pot = (Pot)_player.currentWeapon;
        if (_pot.isReady)
        {
            _pot.Init();
        }
        else
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
    }

    public override void Exit()
    {
        _pot.AttackAnimEnable(false);
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _player.SetDir(_player.PlayerInput.PlayerMoveDir, _player.defaultSpeed);
        _player.Move();
        if (_pot._endTriggerCalled)
        {
            _pot.Attack();
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
