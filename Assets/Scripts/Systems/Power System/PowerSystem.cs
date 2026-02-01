using System;
using DG.Tweening;
using UnityEngine;

public class PowerSystem : MonoBehaviour
{
    public static PowerSystem Instance;

    public static event Action OnStateChange;

    [SerializeField] private BreakerSwitch[] switches;

    [SerializeField] private float maxPower;
    [SerializeField] private float powerFailureDecrement;
    [SerializeField] private float maxPowerFailure;

    [HideInInspector] public float power;
    [HideInInspector] public float drain;
    [HideInInspector] public bool hasPower;

    [HideInInspector] public float powerFailure;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        power = maxPower;
        hasPower = true;

        powerFailure = 1.0f;
    }

    private void Update()
    {
        if (power > 0f)
        {
            power -= drain * Time.deltaTime;

            if (power < 0f)
            {
                power = 0f;
                PowerOut();
            }
        }

        if (!hasPower)
        {
            if (switches[0].isOn && switches[1].isOn && switches[2].isOn)
            {
                hasPower = true;
                OnStateChange.Invoke();
            }
        }
        else if (!(switches[0].isOn && switches[1].isOn && switches[2].isOn))
        {
            hasPower = false;
            OnStateChange.Invoke();
        }

        DebugUI.instance.power = power;
        DebugUI.instance.maxPower = maxPower;
        DebugUI.instance.powerDrain = drain;
        DebugUI.instance.powerFailure = powerFailure;
    }

    private void PowerOut()
    {
        hasPower = false;
        OnStateChange.Invoke();

        int numberOfSwitches = UnityEngine.Random.Range(1, 4);
        bool[] mask;

        if (numberOfSwitches == 1)
        {
            mask = new bool[3];
            mask[UnityEngine.Random.Range(0, 3)] = true;
        }
        else if (numberOfSwitches == 2)
        {
            mask = new bool[3] {true, true, true};
            mask[UnityEngine.Random.Range(0, 3)] = false;
        }
        else
        {
            mask = new bool[3] {true, true, true};
        }

        for (int i = 0; i < 3; i++)
        {
            if (mask[i])
            {
                switches[i].Interact();
            }
        }

        if (powerFailure > maxPowerFailure)
        {
            powerFailure -= powerFailureDecrement;
            powerFailure = Mathf.Clamp(powerFailure, maxPowerFailure, 1f);
        }
        power = maxPower * powerFailure;
    }
}
