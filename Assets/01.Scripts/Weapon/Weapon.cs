using UnityEngine;
using UnityEngine.Animations.Rigging;

public enum WeaponEnum
{
    Sword =1,
    Pot = 2
}

public abstract class Weapon : MonoBehaviour
{
    public Animator Animator { get; private set; }
    [HideInInspector] public WeaponEnum currentWeaponEnum;
    [HideInInspector] public bool _endTriggerCalled;
    private readonly int _attackHash = Animator.StringToHash("Attack");
    
    protected Transform rightGrip;
    protected Transform leftGrip;
    
    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public virtual void IKInit(TwoBoneIKConstraint leftIK, TwoBoneIKConstraint rightIK, int index)
    {
        Animator.Play("Default",-1,0);
        leftIK.data.target = null;
        rightIK.data.target = null;
        currentWeaponEnum = (WeaponEnum)index;
    }

    public virtual void Init()
    {
        _endTriggerCalled = false;
        AttackAnimEnable(true);
    }
    
    public virtual void AttackAnimEnable(bool isActive)
    {
        Animator.SetBool(_attackHash, isActive);
    }

    
    public virtual void AnimationFinishTrigger()
    {
        _endTriggerCalled = true;
    }
    
}
