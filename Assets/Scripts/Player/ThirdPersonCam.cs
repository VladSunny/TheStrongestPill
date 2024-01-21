using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Transform playerBody;
    public GameObject crosshair;

    private CameraSwitcher _cameraSwitcher;
    
    [Header("Keybinds")]
    public KeyCode changeCameraStyleKey = KeyCode.Tab;

    public float rotationSpeed;
    
    public Transform combatLookAt;
    
    private float _horisontalInput;
    private float _verticalInput;

    private void Awake()
    {
        _cameraSwitcher = GetComponent<CameraSwitcher>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        MyInput();
    }

    private void LateUpdate()
    {
        CameraSwitcher.CameraType currentStyle = _cameraSwitcher.GetCurrentActiveCamera();
        
        if (currentStyle == CameraSwitcher.CameraType.BaseCamera)
        {
            crosshair.SetActive(false);
            
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;
            
            Vector3 inputDir = orientation.forward * _verticalInput + orientation.right * _horisontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward =
                    Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
        
        else if (currentStyle == CameraSwitcher.CameraType.CombatCamera)
        { 
            crosshair.SetActive(true);
            
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            Quaternion rotation = Quaternion.LookRotation(dirToCombatLookAt.normalized);

            orientation.rotation = rotation;
            playerObj.rotation = rotation;

        }
        
        if (currentStyle == CameraSwitcher.CameraType.DeathCamera)
        {
            crosshair.SetActive(false);
        }
    }
    private void MyInput()
    {
        _horisontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        
        if (Input.GetKeyDown(changeCameraStyleKey))
        {
            CameraSwitcher.CameraType currentStyle = _cameraSwitcher.GetCurrentActiveCamera();

            if (currentStyle == CameraSwitcher.CameraType.BaseCamera)
            {
                _cameraSwitcher.SwitchCamera(CameraSwitcher.CameraType.CombatCamera);
            }
            else
            {
                _cameraSwitcher.SwitchCamera(CameraSwitcher.CameraType.BaseCamera);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha0))
            _cameraSwitcher.SwitchCamera(CameraSwitcher.CameraType.DeathCamera);
        
    }

}
