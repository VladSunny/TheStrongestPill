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
    public Rigidbody rb;
    public GameObject crosshair;
    
    [Header("Keybinds")]
    public KeyCode changeCameraStyleKey = KeyCode.Tab;

    public float rotationSpeed;

    [Header("Cameras")]
    public GameObject basicCamera;
    public GameObject combatCamera;
    public Transform combatLookAt;
    public CameraStyle currentStyle = CameraStyle.Combat;
    
    private float _horisontalInput;
    private float _verticalInput;
    
    public enum CameraStyle
    {
        Basic,
        Combat
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        combatCamera.SetActive(currentStyle == CameraStyle.Combat);
        crosshair.SetActive(currentStyle == CameraStyle.Combat);
        basicCamera.SetActive(currentStyle == CameraStyle.Basic);
    }

    private void Update()
    {
        MyInput();
    }

    private void LateUpdate()
    {
        
        if (currentStyle == CameraStyle.Basic)
        {
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;
            
            Vector3 inputDir = orientation.forward * _verticalInput + orientation.right * _horisontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward =
                    Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
        
        else if (currentStyle == CameraStyle.Combat)
        { 
            // Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            // orientation.forward = dirToCombatLookAt.normalized;
            // playerObj.forward = dirToCombatLookAt.normalized;
            
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            Quaternion rotation = Quaternion.LookRotation(dirToCombatLookAt.normalized);

            orientation.rotation = rotation;
            playerObj.rotation = rotation;

        }
    }

    private void FixedUpdate()
    {
        
    }

    private void MyInput()
    {
        _horisontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        
        if (Input.GetKeyDown(changeCameraStyleKey))
        {
            combatCamera.SetActive(!combatCamera.activeSelf);
            basicCamera.SetActive(!basicCamera.activeSelf);

            if (basicCamera.activeSelf)
            {
                currentStyle = CameraStyle.Basic;
                crosshair.SetActive(false);
            }
            else if (combatCamera.activeSelf)
            {
                currentStyle = CameraStyle.Combat;
                crosshair.SetActive(true);
            }
        }
        
    }

}
