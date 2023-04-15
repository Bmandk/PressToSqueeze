using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInput : MonoBehaviour
{
    public enum SqueezeType { Hold, Press }

    public float speed = 5f;
    public float jumpForce = 5f;
    [FormerlySerializedAs("squeezeForce")] [Range(0, 0.1f)]
    public float squeezeHoldForce = 0.01f;
    [Range(0, 1f)]
    public float squeezePressForce = 0.1f;
    [Range(0, 0.1f)]
    public float squeezeDecayForce = 0.01f;
    public SqueezeType squeezeType = SqueezeType.Hold;
    
    private Rigidbody2D _rigidbody2D;
    private SpongeScript _spongeScript;
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spongeScript = GetComponent<SpongeScript>();
    }

    private void Update()
    {
        var gamepad = Gamepad.current;

        if (gamepad == null)
        {
            KeyboardMovement();
            switch (squeezeType)
            {
                case SqueezeType.Hold:
                    KeyboardSqueezeHold();
                    break;
                case SqueezeType.Press:
                    KeyboardSqueezePress();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        else
        {
            Movement(gamepad);
            switch (squeezeType)
            {
                case SqueezeType.Hold:
                    SqueezeHold(gamepad);
                    break;
                case SqueezeType.Press:
                    SqueezePress(gamepad);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }


    private void KeyboardMovement()
    {
        Vector2 velocity = new Vector2();
        if (Keyboard.current.aKey.isPressed)
        {
            velocity.x = -speed;
        }
        else if (Keyboard.current.dKey.isPressed)
        {
            velocity.x = speed;
        }
        velocity.y = _rigidbody2D.velocity.y;
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            velocity.y = jumpForce;
        }
        _rigidbody2D.velocity = velocity;
    }


    private void Movement(Gamepad gamepad)
    {
        Vector2 velocity = new Vector2();

        var leftStick = gamepad.leftStick.ReadValue();
        velocity.x = leftStick.x * speed;

        velocity.y = _rigidbody2D.velocity.y;
        if (gamepad.aButton.wasPressedThisFrame)
        {
            velocity.y = jumpForce;
        }

        _rigidbody2D.velocity = velocity;
    }

    private void KeyboardSqueezeHold()
    {
        int dir = Keyboard.current.eKey.isPressed ? 1 : -1;
        _spongeScript.Squeeze(squeezeHoldForce * dir);
    }
    
    private void SqueezeHold(Gamepad gamepad)
    {
        int dir = gamepad.xButton.isPressed ? 1 : -1;
        _spongeScript.Squeeze(squeezeHoldForce * dir);
    }

    private void KeyboardSqueezePress()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            _spongeScript.Squeeze(squeezePressForce);
        }
        else if (!Keyboard.current.eKey.isPressed)
        {
            _spongeScript.Squeeze(-squeezeDecayForce);
        }
    }
    
    private void SqueezePress(Gamepad gamepad)
    {
        if (gamepad.xButton.wasPressedThisFrame)
        {
            _spongeScript.Squeeze(squeezePressForce);
        }
        else if (!gamepad.xButton.isPressed)
        {
            _spongeScript.Squeeze(-squeezeDecayForce);
        }
    }
}
