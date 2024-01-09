using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BPSInput : BasePunchingSystem
{
    protected override void Awake()
    {
        base.Awake();
        
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.BaseAttacks.Enable();
        playerInputActions.BaseAttacks.Punch.performed += base.Punch;
    }
}
