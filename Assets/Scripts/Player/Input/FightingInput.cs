using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FightingInput : MonoBehaviour
{
    private FightingSystem _fightingSystem;
    private void Awake()
    {
        _fightingSystem = GetComponent<FightingSystem>();
        
        // Inputs
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.BaseAttacks.Enable();
        playerInputActions.BaseAttacks.Punch.performed += Punch;
    }

    private void Punch(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);
        _fightingSystem.Punch();
    }
}
