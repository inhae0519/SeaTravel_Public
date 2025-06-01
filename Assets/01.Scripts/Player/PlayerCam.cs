using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private Transform rootTrm;
    private Player _player;
    private float _yRotate;
    
    [Header("Rotate Value")]
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float rotateLimit;

    [Header("Camera Head")]
    public Transform weapon;
    public float amount;
    public float frequency;
    public float smooth;
    public float returnTime;
    
    [Header("Run Lens Size")]
    [SerializeField] private float runLensSize;
    [SerializeField] private float runZoomInTime;
    
    private float _defaultLensSize;
    [HideInInspector] public Vector3 startPos;

    private CinemachineVirtualCamera _virtualCamera;
    private bool _stopRotate;
    
    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        startPos = weapon.localPosition;
        _defaultLensSize = _virtualCamera.m_Lens.FieldOfView;
    }

    private void OnEnable()
    {
        _player.PlayerInput.MouseMoveEvent += HandleMouseMove;
    }
    
    private void OnDisable()
    {
        _player.PlayerInput.MouseMoveEvent -= HandleMouseMove;
    }

    private void HandleMouseMove(Vector2 moveValue)
    {
        if(_stopRotate)
            return;
        
        _player.transform.Rotate(Vector3.up, moveValue.x * rotateSpeed);
        _yRotate += moveValue.y * rotateSpeed;
        _yRotate = Mathf.Clamp(_yRotate, -rotateLimit, rotateLimit);
        transform.localRotation = Quaternion.Euler(-_yRotate,0,0); 
    }

    public void StartHeadBob()
    {
        Vector3 pos = Vector3.zero;
        pos.y = Mathf.Lerp(pos.y, Mathf.Sin(Time.time * frequency) * amount * 1.4f, Time.deltaTime * smooth);
        //pos.x = Mathf.Lerp(pos.x, Mathf.Cos(Time.time * frequency / 2) * amount * 1.6f, Time.deltaTime * smooth);
        weapon.localPosition += pos;
    }

    public void StopHeadBob()
    {
        if(weapon.localPosition == startPos)
            return;
        
        weapon.localPosition = Vector3.Lerp(weapon.localPosition, startPos, returnTime * Time.deltaTime);    
    }

    public void RunCameraZoom(bool isRun)
    {
        if (isRun)
            DOTween.To(() => _virtualCamera.m_Lens.FieldOfView, f => _virtualCamera.m_Lens.FieldOfView = f
                ,runLensSize, runZoomInTime);
        else
            DOTween.To(() => _virtualCamera.m_Lens.FieldOfView, f => _virtualCamera.m_Lens.FieldOfView = f
                ,_defaultLensSize, runZoomInTime);
    }

    public void RotateStop(bool isRotate)
    {
        _stopRotate = isRotate;
    }
}