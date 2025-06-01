using UnityEngine;

public class CommonAttackState : EnemyState<CommonStateEnum>
{
    public CommonAttackState(Enemy enemyBase, EnemyStateMachine<CommonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();
        // 공격이 모두 종료되었다면
        // 다시 배틀
        if (_endTriggerCalled)
            _stateMachine.ChangeState(CommonStateEnum.Battle);
    }

    public override void Enter()
    {
        base.Enter();
        //정지하고
        _enemyBase.MovementCompo.StopImmediately();
        _enemyBase.AnimatorCompo.speed = _enemyBase.attackSpeed;
        
        //플레이어 위치를 바라보도록 회전
        (_enemyBase.MovementCompo as EnemyMovement).LookToTarget(_enemyBase.targetTrm.position);
    }

    public override void Exit()
    {
        _enemyBase.lastAttackTime = Time.time;
        _enemyBase.AnimatorCompo.speed = 1f;

        base.Exit();
    }
    
    
}