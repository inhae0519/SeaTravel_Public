using UnityEngine;

public class CommonBattleState : EnemyState<CommonStateEnum>
{
    private readonly int _velocityHash = Animator.StringToHash("Velocity");
    private EnemyMovement _enemyMovement;
    
    public CommonBattleState(Enemy enemyBase, EnemyStateMachine<CommonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        _enemyMovement = _enemyBase.MovementCompo as EnemyMovement;
    }

    private Vector3 _targetDestination;
    
    public override void Enter()
    {
        base.Enter();
        SetDestination(_enemyBase.targetTrm.position);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        if (_enemyMovement.NavAgent.enabled)
        {
            _targetDestination = _enemyMovement.NavAgent.destination;
        }
        
        float distance = (_targetDestination - _enemyBase.targetTrm.position).magnitude;
        if (distance > 0.5f)
        {
            SetDestination(_enemyBase.targetTrm.position);
        }
        
        float targetDistance = (_enemyBase.targetTrm.position - _enemyBase.transform.position).magnitude;
        
        bool playerInRange = _enemyBase.attackDistance >= targetDistance;
        bool cooldownPass = _enemyBase.lastAttackTime + _enemyBase.attackCooldown <= Time.time;

        if (playerInRange && cooldownPass)
        {
            _stateMachine.ChangeState(CommonStateEnum.Attack);
        }
        else if (playerInRange)
        {
            _enemyBase.MovementCompo.StopImmediately();
            _enemyMovement.LookToTarget(_enemyBase.targetTrm.position);
        }

        float velocity = _enemyBase.MovementCompo.Velocity.magnitude;
        _enemyBase.AnimatorCompo.SetFloat(_velocityHash, velocity);
    }

    private void SetDestination(Vector3 position)
    {
        _targetDestination = position;
        _enemyBase.MovementCompo.SetDestination(_targetDestination);
    }
}