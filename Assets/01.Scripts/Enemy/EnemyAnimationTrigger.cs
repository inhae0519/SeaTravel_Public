using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimationTrigger : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    public UnityEvent OnAnimationEvent;

    private void AnimationEnd()
    {
        _enemy.AnimationEndTrigger();
    }

    private void AnimationEvent()
    {
        OnAnimationEvent?.Invoke();
    }
    
    public void DamageCast()
    {
        _enemy.Attack();
    }
}
