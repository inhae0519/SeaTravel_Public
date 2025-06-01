using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Gun : Weapon
{
    private Transform rightGrip;

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

}
