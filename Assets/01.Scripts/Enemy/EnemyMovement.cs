using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Vector3 Velocity => _navAgent.velocity;
    public bool IsGround { get; }

    [SerializeField] private float _knockbackThreshold;
    [SerializeField] private float _maxKnockbackTime;

    public NavMeshAgent NavAgent => _navAgent;

    private Enemy _enemy;
    private NavMeshAgent _navAgent;
    private Rigidbody _rbCompo;

    private float _knockbackStartTime;
    private bool _isKnockback;
    public void Initialize(Entity agent)
    {
        _enemy = agent as Enemy;
        _navAgent = GetComponent<NavMeshAgent>();
        _navAgent.speed = _enemy.moveSpeed;
        _rbCompo = GetComponent<Rigidbody>();
    }
    
    public void StopImmediately()
    {
        if(_navAgent.enabled == false) return;
            _navAgent.isStopped = true;
    }

    public void SetDestination(Vector3 destination)
    {
        if(_navAgent.enabled == false) return;
        _navAgent.isStopped = false;
        _navAgent.SetDestination(destination);
    }

    public void LookToTarget(Vector3 targetPos)
    {
        Vector3 toward = targetPos - transform.position;
        toward.y = 0;
        transform.rotation=Quaternion.LookRotation(toward);
    }

    public void GetKnockback(Vector3 force)
    {
        StartCoroutine(ApplyKnockbackCoroutine(force));
    }

    private IEnumerator ApplyKnockbackCoroutine(Vector3 force)
    {
        _navAgent.enabled = false;
        _rbCompo.useGravity = true;
        _rbCompo.isKinematic = false;
        _rbCompo.AddForce(force, ForceMode.Impulse);
        _knockbackStartTime = Time.time;
        
        //이미 넉백이 진행중이라면 그냥 시간이랑 힘만 리셋 시키고 종료한다.
        if(_isKnockback)
            yield break;

        _isKnockback = true;
        //yield return new WaitForSeconds(0.1f); //원래는 물리프레임 2프레임 기다려야해
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        yield return new WaitUntil(() => Time.time > _knockbackStartTime + _maxKnockbackTime 
                                         || _rbCompo.velocity.magnitude < _knockbackThreshold);

        DisableRigidbody();

        _navAgent.Warp(transform.position);
        _navAgent.enabled = true;
        _isKnockback = false;
    }

    private void DisableAgent()
    {
        _navAgent.enabled = false;
    }

    private void DisableRigidbody()
    {
        _rbCompo.velocity = Vector3.zero;
        _rbCompo.angularVelocity = Vector3.zero;
        _rbCompo.useGravity = false;
        _rbCompo.isKinematic = true;
    }
}
