using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    GameObject _player;

    [SerializeField] CinemachineVirtualCamera _planetCam;
    [SerializeField] CinemachineVirtualCamera _playerCam;
    bool _planetCamActive = true;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        //FIX
        Quaternion _playerRotation = Quaternion.Euler(_player.transform.rotation.eulerAngles);
        Quaternion.Lerp(_planetCam.transform.rotation, _playerRotation, 0.5f);
    }

    public void SwitchPriority()
    {
        if (_planetCamActive)
        {
            _planetCam.Priority = 0;
            _playerCam.Priority = 1;
        }
        else
        {
            _planetCam.Priority = 1;
            _playerCam.Priority = 0;
        }
        _planetCamActive = !_planetCamActive;
    }
}
