using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Pot : Weapon
{
    [SerializeField] private Transform effectTrm;
    [SerializeField] private GameObject waterEffect;
    [SerializeField] private LayerMask isFire;
    [SerializeField] private float distance;
    [SerializeField] private AudioClip AudioClip;
    
    public bool isReady = true;
    
    protected override void Awake()
    {
        base.Awake();
        rightGrip = transform.Find("RightGrip");
        leftGrip = transform.Find("LeftGrip");
    }
    public override void IKInit(TwoBoneIKConstraint leftIK, TwoBoneIKConstraint rightIK, int index)
    {
        base.IKInit(leftIK, rightIK, index);
        isReady = true;
        rightIK.data.target = rightGrip;
        leftIK.data.target = leftGrip;
    }
    public void ReadyEnd()
    {
        isReady = true;
    }

    public override void Init()
    {
        isReady = false;
        base.Init();
    }
    
    public void Attack()
    {
        Ray ray = CameraManager.Instance.ReturnForwardRay();
        bool isHit = Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, distance,isFire);
        Instantiate(waterEffect, effectTrm.position, transform.rotation);
        SoundManager.Instance.PlayEffect(AudioClip);
        if (isHit)
        {
            Fire fire = hit.collider.GetComponent<Fire>();
            fire.Extinction();
        }
    }
}
