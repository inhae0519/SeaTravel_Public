using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera _playerCam;
    [SerializeField] private Transform _shipCam;
    
    public void ChangeCamera()
    {
        _playerCam.gameObject.SetActive(false);
        _shipCam.gameObject.SetActive(true);
    }
    
    public void SetCameraPlayer()
    {
        _shipCam.gameObject.SetActive(false);
        _playerCam.gameObject.SetActive(true);
    }

    public Ray ReturnForwardRay()
    {
        return new Ray(_playerCam.transform.position, _playerCam.transform.forward);
    }
}
