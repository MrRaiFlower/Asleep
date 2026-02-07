using JetBrains.Annotations;
using Unity.Cinemachine;
using UnityEngine;

public class FOVDistortion : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cameraObject;

    [SerializeField] private Movement movementScript;

    [SerializeField] private float factor;
    [SerializeField] private float speed;

    private float defaultFOV;
    private float slowFOV;
    private float fastFOV;

    private void Start()
    {
        defaultFOV = cameraObject.Lens.FieldOfView;
        slowFOV = defaultFOV * (1.0f - factor);
        fastFOV = defaultFOV * (1.0f + factor);
    }

    private void Update()
    {
        float movementSpeedRatio = movementScript.GetSpeedRatio();

        if (movementSpeedRatio < 0.95f)
        {
            cameraObject.Lens.FieldOfView = Mathf.Lerp(cameraObject.Lens.FieldOfView, slowFOV, speed * Time.deltaTime);
        }
        else if (movementSpeedRatio > 1.05f)
        {
            cameraObject.Lens.FieldOfView = Mathf.Lerp(cameraObject.Lens.FieldOfView, fastFOV, speed * Time.deltaTime);
        }
        else if (movementSpeedRatio == 0)
        {
            cameraObject.Lens.FieldOfView = Mathf.Lerp(cameraObject.Lens.FieldOfView, defaultFOV, speed * Time.deltaTime);
            if (Mathf.Abs(cameraObject.Lens.FieldOfView - defaultFOV) < 0.05f)
            {
                cameraObject.Lens.FieldOfView = defaultFOV;
            }
        }
    }
}
