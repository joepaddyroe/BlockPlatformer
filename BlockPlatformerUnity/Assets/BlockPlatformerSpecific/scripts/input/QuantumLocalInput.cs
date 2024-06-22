using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using Quantum;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuantumLocalInput : MonoBehaviour
{

    [SerializeField] private InputActionAsset _inputActions;

    private InputAction _moveInputAction;
    private InputAction _jumpInputAction;
    
    void Start()
    {
        // grab and assign all of the input actions from the input action asset
        _moveInputAction = _inputActions.FindAction("Move");
        _jumpInputAction = _inputActions.FindAction("Jump");
        
        // this will need to be handled in some kind of UI/Gameplay state switch
        _inputActions.FindActionMap("Gameplay").Enable();
        
        // Subscribe to the input poll callback
        QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
    }

    public void PollInput(CallbackPollInput callback)
    {
        Quantum.Input input = new Quantum.Input();

        // Note: Use GetButton not GetButtonDown/Up Quantum calculates up/down itself.
        input.Jump = _jumpInputAction.IsPressed();
        Vector2 moveValue = _moveInputAction.ReadValue<Vector2>();

        // Input that is passed into the simulation needs to be deterministic that's why it's converted to FPVector2.
        input.Direction = new Vector2(moveValue.x, moveValue.y).ToFPVector2();

        callback.SetInput(input, DeterministicInputFlags.Repeatable);
    }
}
