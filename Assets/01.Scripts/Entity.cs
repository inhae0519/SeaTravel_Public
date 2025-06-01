using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour
{
    #region component region
    public Animator AnimatorCompo { get; protected set; }
    public Rigidbody RigidbodyCompo { get; protected set; }
    public Collider ColliderCompo { get; protected set; }
    
    public Health HealthCompo { get; protected set; }
    public DamageCaster DamageCasterCompo { get; protected set; }

    #endregion

    [SerializeField] private Transform visualTrm;
    public bool CanStateChangeable { get; set; } = true;
    public bool isDead;
    public int damage;

    protected virtual void Awake()
    {
        Transform damageTrm = transform.Find("DamageCaster");
        if (damageTrm != null)
        {
            DamageCasterCompo = damageTrm.GetComponent<DamageCaster>();
            DamageCasterCompo.InitCaster(this);
        }
        
        AnimatorCompo = visualTrm.GetComponent<Animator>();
        RigidbodyCompo = GetComponent<Rigidbody>();
        ColliderCompo = GetComponent<Collider>();
        HealthCompo = GetComponent<Health>();
        HealthCompo.Initialize(this);
    }

    #region Delay callback coroutine

    public Coroutine StartDelayCallback(float delayTime, Action Callback)
    {
        return StartCoroutine(DelayCoroutine(delayTime, Callback));
    }

    protected IEnumerator DelayCoroutine(float delayTime, Action callback)
    {
        yield return new WaitForSeconds(delayTime);
        callback?.Invoke();
    }

    #endregion

    public virtual void Attack()
    {
    }
    
    protected void Pause()
    {
        Debug.Log("afaf");
        GameManager.Instance.Pause();
    }
    
    protected void MapOpen()
    {
        UIManager.Instance.EnableMap();
    }
}
