using UnityEngine;

public class CommonIdleState : EnemyState<CommonStateEnum>
{
    public CommonIdleState(Enemy enemyBase, EnemyStateMachine<CommonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Collider target = _enemyBase.IsPlayerDetected();
        if(target==null) return;
        
        Vector3 direction = target.transform.position - _enemyBase.transform.position;
        direction.y = 0;

        if (_enemyBase.IsObstacleInLine(direction.magnitude, direction.normalized) == false)
        {
            _enemyBase.targetTrm = target.transform;
            _stateMachine.ChangeState(CommonStateEnum.Battle);
        }
    }
}