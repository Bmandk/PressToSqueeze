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
    [FormerlySerializedAs("squeezeForce")] [Range(0, 100f)]
    public float squeezeHoldForce = 0.01f;
    [Range(0, 1f)]
    public float squeezePressForce = 0.1f;
    [Range(0, 0.1f)]
    public float squeezeDecayForce = 0.01f;
    public SqueezeType squeezeType = SqueezeType.Hold;
    public float smallJumpForce = 0.1f;
    public float floorRaycastDistance;
    
    private Rigidbody2D _rigidbody2D;
    private SpongeScript _spongeScript;
    private Collider2D col;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spongeScript = GetComponent<SpongeScript>();
        col = GetComponent<Collider2D>();
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

        if (Mathf.Abs(_rigidbody2D.velocity.x) > 0.1f)
        {
            CheckSmallJump();
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
    
    private float smallJumpVelocity;
    private bool isSmallJumping;
    public Transform graphics;
    public float smallGravity;
    public GameObject waterShadowPrefab;
    public float waterShadowOffset;

    private void CheckSmallJump()
    {
        RaycastHit2D[] results = new RaycastHit2D[9];
        if (!isSmallJumping && col.Raycast(Vector2.down, results, floorRaycastDistance) > 0)
        {
            StartCoroutine(SmallJump());
        }
    }

    private IEnumerator SmallJump()
    {
        if (isSmallJumping)
            yield break;
        isSmallJumping = true;
        smallJumpVelocity = smallJumpForce;
        while (graphics.localPosition.y >= 0)
        {
            yield return null;
            smallJumpVelocity += smallGravity * Time.deltaTime;
            graphics.position += Vector3.up * smallJumpVelocity;
        }
        isSmallJumping = false;
        var graphicsLocalPosition = graphics.localPosition;
        graphicsLocalPosition.y = 0;
        graphics.localPosition = graphicsLocalPosition;

        RaycastHit2D[] results = new RaycastHit2D[9];
        if (_spongeScript.currentWaterAmount > 0.05f && col.Raycast(Vector2.down, results, floorRaycastDistance) > 0)
        {
            var shadow = Instantiate(waterShadowPrefab, transform.position + Vector3.down * waterShadowOffset, Quaternion.identity);
            var sr = shadow.GetComponent<SpriteRenderer>();
            var color = sr.color;
            color.a = _spongeScript.currentWaterAmount * color.a;
            sr.color = color;
        }
    }

    private void KeyboardSqueezeHold()
    {
        int dir = Keyboard.current.eKey.isPressed ? 1 : -1;
        _spongeScript.Squeeze(squeezeHoldForce * dir * Time.deltaTime);
    }
    
    private void SqueezeHold(Gamepad gamepad)
    {
        int dir = gamepad.xButton.isPressed ? 1 : -1;
        _spongeScript.Squeeze(squeezeHoldForce * dir * Time.deltaTime);
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
            _spongeScript.Squeeze(squeezePressForce * Time.deltaTime);
        }
        else if (!gamepad.xButton.isPressed)
        {
            _spongeScript.Squeeze(-squeezeDecayForce * Time.deltaTime);
        }
    }
}
