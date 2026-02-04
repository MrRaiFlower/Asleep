using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightControl : MonoBehaviour
{
    public static FlashlightControl Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private GameObject cameraObject;
    [SerializeField] private Light[] lights;

    [SerializeField] private AudioSource flashlightSound;

    [SerializeField] public float maxCharge;
    [SerializeField] private float startCharge;
    [SerializeField] private float dischargeSpeed;
    [SerializeField] private float switchSpeed;

    [SerializeField] public float minRecharge;
    [SerializeField] public float maxRecharge;

    private float intensity;

    [HideInInspector] public bool isActive;

    [HideInInspector] public float charge;

    [HideInInspector] public bool isOn;
    private bool isSwitching;

    private InputAction flashlightAction;

    private void Start()
    {
        flashlightAction = InputSystem.actions.FindAction("Flashlight");

        intensity = lights[0].intensity;
        
        charge = startCharge;

        foreach (Light light in lights)
        {
           light.intensity = 0;
        }

        Flashlight.OnPickup += () => isActive = true;
        BatteryPile.OnPickUp += () => Refill(Random.Range(minRecharge, maxRecharge));
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        if (isOn && charge > 0f)
        {
            charge -= dischargeSpeed * Time.deltaTime;

            if (charge < 0f)
            {
                charge = 0f;
                TurnOff();
            }
        }

        if (flashlightAction.WasPressedThisFrame() && !isSwitching)
        {
            if (isOn)
            {
                TurnOff();
            }
            else if (charge > 0f)
            {
                TurnOn();
            }

            flashlightSound.Play();
            isOn = !isOn;
        }

        DebugOverlay.Instance.flashlightCharge = charge;
    }

    private void TurnOn()
    {
        Sequence turnOnSequence = DOTween.Sequence();
        turnOnSequence.SetEase(Ease.InOutSine);
        turnOnSequence.Pause();

        turnOnSequence.AppendCallback(() => isSwitching = true);
        foreach (Light light in lights)
        {
            turnOnSequence.Join(light.DOIntensity(intensity, switchSpeed));
        }
        turnOnSequence.AppendInterval(flashlightSound.clip.length - switchSpeed);
        turnOnSequence.AppendCallback(() => isSwitching = false);

        turnOnSequence.Play();
    }

    private void TurnOff()
    {
        Sequence turnOffSequence = DOTween.Sequence();
        turnOffSequence.SetEase(Ease.InOutSine);
        turnOffSequence.Pause();

        turnOffSequence.AppendCallback(() => isSwitching = true);
        foreach (Light light in lights)
        {
            turnOffSequence.Join(light.DOIntensity(0, switchSpeed));
        }
        turnOffSequence.AppendInterval(flashlightSound.clip.length - switchSpeed);
        turnOffSequence.AppendCallback(() => isSwitching = false);

        turnOffSequence.Play();
    }

    private void Refill(float amount)
    {
        charge += amount;
        if (charge > maxCharge)
        {
            charge = maxCharge;
        }

        if (isOn)
        {
            TurnOn();
        }
    }
}
