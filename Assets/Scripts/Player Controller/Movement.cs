using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;

    [SerializeField] private CharacterController characterControllerComponent;

    [SerializeField] private PlayerSound playerSoundScript;
    [SerializeField] private Stamina staminaScript;

    [SerializeField] private LayerMask playerLayerMask;

    [SerializeField] public float normalSpeed;
    [SerializeField] public float crouchSpeed;
    [SerializeField] public float sprintSpeed;
    [SerializeField] private float groundAcceleration;
    [SerializeField] private float airAceleration;
    [SerializeField] private float groundDrag;
    [SerializeField] private float airDrag;
    [SerializeField] private float jumpSpeed;

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool wasGrounded;
    [HideInInspector] public bool hasGrounded;

    [HideInInspector] public bool isTouchingCeiling;
    [HideInInspector] public bool wasTouchingCeiling;
    [HideInInspector] public bool hasTouchedCeiling;

    private Vector2 moveDirectionInput;
    private Vector3 moveDirection;

    [HideInInspector] public bool isMoving;

    private float acceleration;

    private float horizontalSpeed;
    [HideInInspector] public Vector3 horizontalvelocity;

    [HideInInspector] public Vector3 verticalVelocity;

    private Vector3 velocity;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction crouchAction;
    private InputAction sprintAction;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        sprintAction = InputSystem.actions.FindAction("Sprint");
    }

    private void Update()
    {
        CheckGround();
        CheckCeiling();

        CalculateMoveDirection();

        CalculateHorizontalVelocity();
        CalculateVerticalVelocity();
        
        ComposeVelocity();

        Move();
    }

    private void CalculateMoveDirection()
    {
        moveDirectionInput = moveAction.ReadValue<Vector2>();

        isMoving = moveDirectionInput.sqrMagnitude != 0.0f;

        if (isMoving)
        {
            if (crouchAction.IsPressed())
            {
                horizontalSpeed = crouchSpeed;
            }
            else if (sprintAction.IsPressed() && staminaScript.stamina >= 0.0f && !staminaScript.isTired)
            {
                horizontalSpeed = sprintSpeed;
            }
            else
            {
                horizontalSpeed = normalSpeed;
            }
        }
        else
        {
            horizontalSpeed = 0.0f;
        }

        if (isMoving)
        {
            moveDirection = (playerObject.transform.right * moveDirectionInput.x + playerObject.transform.forward * moveDirectionInput.y) * horizontalSpeed;
        }
        else if (horizontalSpeed == 0.0f)
        {
            moveDirection = Vector3.zero;
        }
    }

    private void CalculateHorizontalVelocity()
    {
        if (isGrounded)
        {
            if (isMoving)
            {
                acceleration = groundAcceleration;
            }
            else
            {
                acceleration = groundDrag;
            }
        }
        else
        {
            if (isMoving)
            {
                acceleration = airAceleration;
            }
            else
            {
                acceleration = airDrag;
            }
        }

        horizontalvelocity = Vector3.Lerp(horizontalvelocity, moveDirection, acceleration * Time.deltaTime);
    }

    private void CalculateVerticalVelocity()
    {
        if (jumpAction.WasPressedThisFrame() && isGrounded && staminaScript.Jump())
        {
            verticalVelocity = Vector3.up * jumpSpeed;

            playerSoundScript.PlayJumpSound();
        }
        else if (hasTouchedCeiling)
        {
            verticalVelocity = Vector3.zero;
        }
        else if (isGrounded && verticalVelocity.y < 0.0f)
        {
            verticalVelocity = Vector3.down * 0.1f;
        }
        else
        {
            verticalVelocity += Vector3.down * 9.81f * Time.deltaTime;
        }
    }

    private void ComposeVelocity()
    {
        velocity = horizontalvelocity + verticalVelocity;
    }

    private void Move()
    {
        characterControllerComponent.Move(velocity * Time.deltaTime);
    }

    private void CheckGround()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(playerObject.transform.position + Vector3.up * characterControllerComponent.radius * 0.475f, characterControllerComponent.radius * 0.95f, ~playerLayerMask);
        hasGrounded = !wasGrounded && isGrounded;

        if (hasGrounded && -verticalVelocity.y > jumpSpeed * 0.95f)
        {
            playerSoundScript.PlayLandingSound();
        }
    }

    private void CheckCeiling()
    {
        wasTouchingCeiling = isTouchingCeiling;
        isTouchingCeiling = Physics.CheckSphere(playerObject.transform.position + Vector3.up * (characterControllerComponent.height -(characterControllerComponent.radius * 0.475f)), characterControllerComponent.radius * 0.95f, ~playerLayerMask);
        hasTouchedCeiling = !wasTouchingCeiling && isTouchingCeiling;
    }

    public float GetSpeedRatio()
    {
        return horizontalSpeed / normalSpeed;
    }
}
