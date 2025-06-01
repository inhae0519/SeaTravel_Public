using UnityEngine;

public class PlayerSwordState : PlayerState
{
    public PlayerSwordState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    private int _comboCounter = 0;
    private float _lastAttackTime;
    private float _comboWindow = 1f; // 키를 누른 이후 다시 키를 누르기까지 대기시간
    private Sword _sword;
    
    public override void Enter()
    {
        base.Enter();
        _sword = (Sword)_player.currentWeapon;
        _sword.Init();
        
        bool comboCounterOver = _comboCounter > 1;
        bool comboWindowExhaust = Time.time >= _lastAttackTime + _comboWindow;
        if (comboCounterOver || comboWindowExhaust)
        {
            _comboCounter = 0;
        }
        
        _sword.Attack(_comboCounter);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _player.SetDir(_player.PlayerInput.PlayerMoveDir, _player.defaultSpeed);
        _player.Move();
        if(_sword._endTriggerCalled || _player.currentWeapon != _sword)
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
    }

    public override void Exit()
    {
        ++_comboCounter;
        _lastAttackTime = Time.time;
        _sword.AttackAnimEnable(false);
        base.Exit();
    }
}
