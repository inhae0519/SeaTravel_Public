using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class ShipCam : MonoBehaviour
{
    private Ship _ship;
    private float _yRotate;
    private float _xRotate;
    
    [Header("Rotate Value")]
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float rotateLimit;

    private void Awake()
    {
        _ship = transform.root.GetComponent<Ship>();
    }

    private void OnEnable()
    {
        _ship.ShipInput.mouseDeltaEvent += HandleMouseMove;
    }
    
    private void OnDisable()
    {
        _ship.ShipInput.mouseDeltaEvent -= HandleMouseMove;
    }

    private void HandleMouseMove(Vector2 moveValue)
    {
        _xRotate += moveValue.x * rotateSpeed;
        _yRotate += moveValue.y * rotateSpeed;
        _yRotate = Mathf.Clamp(_yRotate, -rotateLimit, rotateLimit);
        transform.localRotation = Quaternion.Euler(-_yRotate,_xRotate,0); 
    }

    public void ShakeShipCam(float value)
    {
        transform.DOShakePosition(0.3f,  value * 0.5f);
    }
}
