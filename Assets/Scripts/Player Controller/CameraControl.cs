using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject cameraHolderObject;

    [SerializeField] private float sensitivity;
    [SerializeField] private float acceleration;
    [SerializeField] private float drag;

    private Vector2 mouseVelocity;
    private Vector2 lookVelocity;

    private Vector3 cameraRotation;

    private InputAction lookAction;

    private void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        cameraRotation = Vector3.up * playerObject.transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        mouseVelocity = lookAction.ReadValue<Vector2>() / Time.unscaledDeltaTime;

        if (mouseVelocity.sqrMagnitude != 0f)
        {
            lookVelocity = Vector2.Lerp(lookVelocity, ProcessMouseVelocity(mouseVelocity), acceleration * Time.deltaTime);
        }
        else
        {
            lookVelocity = Vector2.Lerp(lookVelocity, Vector2.zero, drag * Time.deltaTime);
        }

        cameraRotation.y += lookVelocity.x;

        cameraRotation.x -= lookVelocity.y;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -90.0f, 90.0f);

        playerObject.transform.rotation = Quaternion.Euler(0.0f, cameraRotation.y, 0.0f);
        cameraHolderObject.transform.localRotation = Quaternion.Euler(cameraRotation.x, 0.0f, 0.0f);
    }

    private Vector2 ProcessMouseVelocity(Vector2 velocity)
    {
        return Vector2.right * (velocity.x * sensitivity / 2.0f * Time.deltaTime) + Vector2.up * (velocity.y * sensitivity * Time.deltaTime);
    }
}
