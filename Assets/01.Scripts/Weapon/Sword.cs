using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Sword : Weapon
{
    [SerializeField] private DamageCaster _damageCaster;
    [SerializeField] private AudioClip AudioClip;
    private Transform rightGrip;
    private readonly int _comboCounterHash = Animator.StringToHash("ComboCount");
    private readonly int _attackHash = Animator.StringToHash("Attack");

    protected override void Awake()
    {
        base.Awake();
        rightGrip = transform.Find("RightGrip");
    }

    public override void IKInit(TwoBoneIKConstraint leftIK, TwoBoneIKConstraint rightIK, int index)
    {
        base.IKInit(leftIK, rightIK, index);
        rightIK.data.target = rightGrip;
    }
    
    public void Attack(int comboCounter)
    {
        SoundManager.Instance.PlayEffect(AudioClip);
        Animator.SetInteger(_comboCounterHash, comboCounter);
    }

    public void CheckDamage()
    {
        _damageCaster.CastDamage();
    }
}
