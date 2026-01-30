using UnityEngine;

public class FOVDistortion : MonoBehaviour
{
    [SerializeField] private Camera cameraObject;

    [SerializeField] private Movement movementScript;

    [SerializeField] private float factor;
    [SerializeField] private float speed;

    private float defaultFOV;
    private float slowFOV;
    private float fastFOV;

    private void Start()
    {
        defaultFOV = cameraObject.fieldOfView;
        slowFOV = defaultFOV * (1.0f - factor);
        fastFOV = defaultFOV * (1.0f + factor);
    }

    private void Update()
    {
        float movementSpeedRatio = movementScript.GetSpeedRatio();

        if (movementSpeedRatio < 0.95f)
        {
            cameraObject.fieldOfView = Mathf.Lerp(cameraObject.fieldOfView, slowFOV, speed * Time.deltaTime);
        }
        else if (movementSpeedRatio > 1.05f)
        {
            cameraObject.fieldOfView = Mathf.Lerp(cameraObject.fieldOfView, fastFOV, speed * Time.deltaTime);
        }
        else if (movementSpeedRatio == 0)
        {
            cameraObject.fieldOfView = Mathf.Lerp(cameraObject.fieldOfView, defaultFOV, speed * Time.deltaTime);
            if (Mathf.Abs(cameraObject.fieldOfView - defaultFOV) < 0.05f)
            {
                cameraObject.fieldOfView = defaultFOV;
            }
        }
    }
}
