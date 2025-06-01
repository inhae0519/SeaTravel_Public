using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : Entity
{
    [SerializeField] private PlayerInputReader playerInput;
    public PlayerInputReader PlayerInput => playerInput;

    [Header("Move Value")]
    public float defaultSpeed;
    public float runSpeed;
    public float jumpPower;
    public float swimDownValue;
    public float swimSpeed;

    [Header("Collider Check")]
    [SerializeField] private Transform groundChecker;
    [SerializeField] private LayerMask isGround;
    [SerializeField] private Vector3 checkSize;

    [Header("Water Check")]
    [SerializeField] private LayerMask isWater;
    public bool IsGround => IsGroundDetected().Length > 0;
    public bool IsWater => IsWaterDetected().Length > 0;

    [Header("Weapon")] 
    public Transform weapons;
    public RigBuilder rigBuilder;
    public TwoBoneIKConstraint leftIK;
    public TwoBoneIKConstraint rightIK;
    public Weapon currentWeapon;

    [Header("Sound")] 
    public AudioClip walk;
    public AudioClip run;
    public AudioClip damaged;
    public AudioClip swim;
    
    public PlayerStat PlayerStat { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerCam PlayerCam { get; private set; }
    
    private Vector3 _velocity;
    
    private bool isInventoryOpen = true;
    private bool isMapOpen = true;
    protected override void Awake()
    {
        base.Awake();
        StateMachine = new PlayerStateMachine();

        foreach (PlayerStateEnum stateEnum in Enum.GetValues(typeof(PlayerStateEnum)))
        {
            string typeName = stateEnum.ToString();
            try
            {
                Type t = Type.GetType($"Player{typeName}State");
                PlayerState state = Activator.CreateInstance(t, this, StateMachine, typeName) as PlayerState;

                StateMachine.AddState(stateEnum, state);
            }
            catch(Exception e)
            {
                Debug.LogError($"{typeName} is loading error!");
            }
        }

        PlayerStat = GetComponent<PlayerStat>();
        PlayerCam = GetComponentInChildren<PlayerCam>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        StateMachine.Initialize(PlayerStateEnum.Idle, this);
    }

    private void OnEnable()
    {
        playerInput.InventoryEvent += InventoryOpen;
        playerInput.MapEvent += MapOpen;
        playerInput.PauseEvent += Pause;
    }

    private void OnDisable()
    {
        playerInput.InventoryEvent -= InventoryOpen;
        playerInput.MapEvent -= MapOpen;
        playerInput.PauseEvent -= Pause;
    }

    private void Update()
    {
        StateMachine.CurrentState.UpdateState();
        PlayerCam.StopHeadBob();
    }
    
    public void SetDir(Vector3 moveDir, float speed)
    {
        _velocity = transform.TransformDirection(moveDir.x, 0, moveDir.y)
            .normalized * speed;
    }

    public void Move(bool inWater = false)
    {
        _velocity.y = inWater ? swimDownValue : RigidbodyCompo.velocity.y;
        RigidbodyCompo.velocity = _velocity;
    }

    public void Swim()
    {
        _velocity.y = swimSpeed;
        RigidbodyCompo.velocity = _velocity;
    }

    public void SetPositionAndRot(Transform trm)
    {
        transform.position = trm.position;
        transform.rotation = trm.rotation;
    }
    
    public void SetDamage()
    {
        SoundManager.Instance.PlayEffect(damaged);
        VolumeManager.Instance.StartDamageVolume(HealthCompo.ReturnCurrentHealth());
        UIManager.Instance.SetPlayerStat(PlayerStatEnum.Health,HealthCompo.ReturnCurrentHealth());
    }

    public void SetDead()
    {
        PlayerManager.Instance.PlayerInputOnOff(false);
        ShipManager.Instance.ShipInputOnOff(false);
        GameManager.Instance.GameOver();
    }

    private void InventoryOpen()
    {
        UIManager.Instance.OnOffInventory(isInventoryOpen);
        isInventoryOpen = !isInventoryOpen;
    }
    
    public override void Attack()
    {
        bool success = DamageCasterCompo.CastDamage();
    }
    
    private Collider[] IsGroundDetected()
    {
        return Physics.OverlapBox(groundChecker.position, checkSize,
            groundChecker.localRotation, isGround);
    }

    private Collider[] IsWaterDetected()
    {
        return Physics.OverlapBox(groundChecker.position, checkSize,
            groundChecker.localRotation, isWater);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundChecker.transform.position, checkSize);
    }
}
