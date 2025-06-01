using System.Collections;
using UnityEngine;

public class CommonDeadState : EnemyState<CommonStateEnum>
{
    public CommonDeadState(Enemy enemyBase, EnemyStateMachine<CommonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
    }
    private int _deadBodyLayer = LayerMask.NameToLayer("DeadBody");
    private bool _isDissolve = false;

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Dead");
        _enemyBase.gameObject.layer = _deadBodyLayer;
    }

    //아직 안한게 넉백중일때 처리
    public override void UpdateState()
    {
        base.UpdateState();
        if (_endTriggerCalled)
        {
            _enemyBase.StartCoroutine(StartDissolveCoroutine());
        }
    }

    private IEnumerator StartDissolveCoroutine()
    {
        yield return new WaitForSeconds(2f);
        GameObject.Destroy(_enemyBase.gameObject);
    }
}
