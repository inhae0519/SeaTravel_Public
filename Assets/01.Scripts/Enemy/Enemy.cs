using UnityEngine;

public abstract class Enemy : Entity
{
    [Header("Common Settings")] 
    public float moveSpeed;
    public float battleTime;
    protected float isActive;

    protected float _defaultMoveSpeed;
    
    [SerializeField] protected LayerMask _whatIsPlayer;
    [SerializeField] protected LayerMask _whatIsObstacle;

    [Header("Attack Settings")]
    public float attackSpeed;
    public float runAwayDistance;
    public float attackDistance;
    public float attackCooldown;
    [SerializeField] protected int _maxCheckEnemy = 1;
    [HideInInspector] public float lastAttackTime;
    [HideInInspector] public Transform targetTrm;
    protected Collider[] _enemyCheckColliders;

    public EnemyMovement MovementCompo { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _defaultMoveSpeed = moveSpeed;
        _enemyCheckColliders = new Collider[_maxCheckEnemy];
        
        MovementCompo = GetComponent<EnemyMovement>();
        MovementCompo.Initialize(this);
    }

    public virtual Collider IsPlayerDetected()
    {
        int cnt = Physics.OverlapSphereNonAlloc(transform.position, runAwayDistance,
            _enemyCheckColliders, _whatIsPlayer);

        return cnt >= 1 ? _enemyCheckColliders[0] : null;
    }

    public virtual bool IsObstacleInLine(float distance, Vector3 direction)
    {
        return Physics.Raycast(transform.position, direction, distance, _whatIsObstacle);
    }

    public override void Attack()
    {
        DamageCasterCompo.CastDamage();
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, runAwayDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        Gizmos.color = Color.white;
    }

    public abstract void AnimationEndTrigger();
}