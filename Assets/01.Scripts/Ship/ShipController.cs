using System;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private AudioClip AudioClip;
    private Ship _ship;
    
    [Header("Move Value")]
    public float defaultSpeed;
    public float maxSpeed;
    public float rotSpeed;

    private Transform _sailSetPos;
    private Player _player;

    private float velocity;

    private void Awake()
    {
        _ship = GetComponent<Ship>();
        _sailSetPos = transform.Find("SetSailPos");
    }
    
    private void OnEnable()
    {
        _ship.ShipInput.exitEvent += HandleExitEvent;
    }

    private void OnDisable()
    {
        _ship.ShipInput.exitEvent -= HandleExitEvent;
    }

    private void Update()
    {
        velocity = _ship.RigidbodyCompo.velocity.magnitude;
        Move();
        ShipRotate();
        if (_ship.ShipInput.GetEnable())
        {
            PlayerManager.Instance.Player.SetPositionAndRot(_sailSetPos);
        }
    }

    private void ShipRotate()
    {
        Vector3 rotDir = Vector3.up * _ship.ShipInput.XInput;
        transform.Rotate(rotDir, rotSpeed);
    }

    private void Move()
    {
        Vector3 _moveDir = transform.forward * _ship.ShipInput.ZInput;
        _ship.RigidbodyCompo.AddForce(_moveDir * defaultSpeed, ForceMode.Acceleration);
        SpeedControl();
    }
    
    private void SpeedControl()
    {
        Vector3 currentSpeed = new Vector3(_ship.RigidbodyCompo.velocity.x, 0, _ship.RigidbodyCompo.velocity.z);
        if (currentSpeed.magnitude > maxSpeed)
        {
            currentSpeed = currentSpeed.normalized * maxSpeed;
            _ship.RigidbodyCompo.velocity = new Vector3(currentSpeed.x, _ship.RigidbodyCompo.velocity.y, currentSpeed.z);
        }
    }
    
    private void HandleExitEvent()
    {
        ShipManager.Instance.ShipInputOnOff(false);
        PlayerManager.Instance.Player.RigidbodyCompo.velocity = Vector3.zero;
        PlayerManager.Instance.Player.RigidbodyCompo.position = _sailSetPos.position;
        PlayerManager.Instance.PlayerTrm.rotation = Quaternion.Euler(Vector3.zero);
        PlayerManager.Instance.PlayerInputOnOff(true);
        CameraManager.Instance.SetCameraPlayer();
        GameManager.Instance.isShip = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Rock"))
        {
            if (velocity >= 5)
            {
                SoundManager.Instance.PlayEffect(AudioClip);
                _ship.HealthCompo.ApplyDamage(velocity);
                _ship.ShipCamCompo.ShakeShipCam(velocity);
            }
        }
    }
}