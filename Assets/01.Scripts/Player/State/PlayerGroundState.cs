using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.JumpEvent += HandleJumpEvent;
        _player.PlayerInput.AttackEvent += HandleAttackEvent;
        _player.PlayerInput.WeaponSwapEvent += HandleWeaponSwap;
    }

    public override void Exit()
    {
        base.Exit();
        _player.PlayerInput.JumpEvent -= HandleJumpEvent;
        _player.PlayerInput.AttackEvent -= HandleAttackEvent;
        _player.PlayerInput.WeaponSwapEvent -= HandleWeaponSwap;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Vector2 input = _player.PlayerInput.PlayerMoveDir;

        if(!_player.IsGround)
            _stateMachine.ChangeState(PlayerStateEnum.Fall);
        if (_player.IsWater)
            _stateMachine.ChangeState(PlayerStateEnum.Swim);
        if (_player.PlayerInput.IsRunPressed && input.magnitude > 0.05f && _player.PlayerStat.EnergyDown())
            _stateMachine.ChangeState(PlayerStateEnum.Run);
    }

    private void HandleJumpEvent()
    {
        if (_player.IsGround)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Jump);
        }
    }

    private void HandleAttackEvent()
    {
        if(_player.currentWeapon != null)
            _stateMachine.ChangeState((PlayerStateEnum)_player.currentWeapon.currentWeaponEnum);
    }
    
    private void HandleWeaponSwap(int index)
    {
        if(_player.currentWeapon != null)
            _player.currentWeapon.gameObject.SetActive(false);
        
        _player.currentWeapon = _player.weapons.GetChild(index - 1).GetComponent<Weapon>();
        _player.currentWeapon.gameObject.SetActive(true);
        _player.currentWeapon.IKInit(_player.leftIK, _player.rightIK, index);
        _player.rigBuilder.Build();
    }
}
