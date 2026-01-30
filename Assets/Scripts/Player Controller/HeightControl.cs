using UnityEngine;
using UnityEngine.InputSystem;

public class HeightControl : MonoBehaviour
{
    [SerializeField] private GameObject cameraHolderObject;

    [SerializeField] private CharacterController characterControllerComponent;

    [SerializeField] private Movement movementScript;

    [SerializeField] private float crouchHeightMultiplier;
    [SerializeField] private float crouchHeightChangeSpeed;

    private float normalHeight;
    private float crouchHeight;

    [HideInInspector] public Vector3 cameraHolderOffset;

    private InputAction crouchAction;

    private void Start()
    {
        crouchAction = InputSystem.actions.FindAction("Crouch");

        normalHeight = characterControllerComponent.height;
        crouchHeight = normalHeight * crouchHeightMultiplier;
    }

    private void Update()
    {
        if (crouchAction.IsPressed() && movementScript.isGrounded)
        {
            characterControllerComponent.height = Mathf.Lerp(characterControllerComponent.height, crouchHeight, crouchHeightChangeSpeed * Time.deltaTime);
        }
        else if (!movementScript.isTouchingCeiling)
        {
            characterControllerComponent.height = Mathf.Lerp(characterControllerComponent.height, normalHeight, crouchHeightChangeSpeed * Time.deltaTime);
        }

        characterControllerComponent.center = Vector3.up * characterControllerComponent.height / 2.0f;
        
        cameraHolderObject.transform.localPosition = Vector3.up * characterControllerComponent.height * 0.9375f + cameraHolderOffset;
    }
}
