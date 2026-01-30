using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private Image crosshair;

    [SerializeField] private CharacterController characterControllerComponent;

    [SerializeField] private LayerMask playerLayerMask;

    [SerializeField] private float range;

    private Color crosshairColor;
    private Color transperentColor;

    private bool isFacingObject;

    private InputAction interactAction;
    
    private void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");

        crosshairColor = crosshair.color;
        transperentColor = new Color(0f, 0f, 0f, 0f);

        crosshair.color = transperentColor;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraObject.transform.position, cameraObject.transform.forward, out hit, range, ~playerLayerMask))
        {
            if (hit.collider.gameObject.TryGetComponent(out InteractableObject interactableObject))
            {
                if (!isFacingObject)
                {
                    crosshair.DOColor(crosshairColor, 0.2f);
                }
                isFacingObject = true;

                if (interactAction.WasPressedThisFrame())
                {
                    interactableObject.Interact();
                }
            }
            else
            {
                if (isFacingObject)
                {
                    crosshair.DOColor(transperentColor, 0.2f);
                }
                isFacingObject = false;
            }
        }
        else
        {
            if (isFacingObject)
            {
                crosshair.DOColor(transperentColor, 0.2f);
            }
            isFacingObject = false;
        }
    }
}
