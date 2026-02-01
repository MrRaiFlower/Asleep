using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Internal;
using UnityEngine;

public class SanitySystem : MonoBehaviour
{
    public static SanitySystem Instance;

    [SerializeField] public float sanityTickLenght;
    [Space(8)]
    [SerializeField] public float startDisturbanceChance;
    [SerializeField] public float endDisturbanceChance;
    [Space(8)]
    [SerializeField] public float lightSanityDecrement;
    [SerializeField] public float doorSanityDecrement;
    [Space(8)]
    [SerializeField] public float usualDisturbanceWeight;
    [SerializeField] public float lightDisturbanceWeight;
    [SerializeField] public float doorDisturbanceWeight;
    [Space(8)]
    [SerializeField] public float maxSanity;
    [SerializeField] public float sanityDropRate;
    [SerializeField] public float sanityAmplifier;
    [SerializeField] public float sanityDropInDark;
    [SerializeField] public float sanityRegenInLight;
    [Space(8)]
    [SerializeField] public float minPillRegen;
    [SerializeField] public float maxPillRegen;
    [Space(8)]
    [SerializeField] public float startDangerChance;
    [SerializeField] public float endDangerChance;
    [Space(8)]
    [SerializeField] public float startIntruderChance;
    [SerializeField] public float endIntruderChance;
    [Space(8)]
    [SerializeField] public float intruderHour;
    [Space(8)]
    [SerializeField] public float gameOverChance;
    [SerializeField] public float gameOverTimer;
    [Space(8)]
    [SerializeField] public float disturbCooldown;
    [SerializeField] public float dangerCooldown;
    [SerializeField] public float intruderCooldown;

    [HideInInspector] public List<DisturbanceObject> disturbanceObjects = new List<DisturbanceObject>();

    [HideInInspector] public List<LightSwitchObject> lights = new List<LightSwitchObject>();
    [HideInInspector] public List<Door> doors = new List<Door>();
    [HideInInspector] public List<DangerObject> dangerObjects = new List<DangerObject>();

    // [HideInInspector] public List<DisturbanceObject> intruders = new List<DisturbanceObject>();

    [HideInInspector] public int disturbanceLevel;
    [HideInInspector] public int disturbanceMaxLevel;

    private float disturbanceWeightsSum;

    private float actualDisturbanceAmplifier;

    [HideInInspector] public float sanity;
    private float actualSanityDropRate;
    private float actualSanityAmplifier;

    [HideInInspector] public float dangerLevel;

    private bool isIntruderActive;

    private float actualDisturbanceChance;
    private float actualDangerChance;
    private float actualIntruderChance;
    private float actualGameOverChance;

    private bool canDisturb;
    private bool canDanger;
    private bool canIntruder;

    [HideInInspector] public Action OnDangerChange;
    private bool canGameOver;

    private TimeSystem timeSystem;
    private RoomSystem roomSystem;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timeSystem = GameObject.Find("Time System").GetComponent<TimeSystem>();
        timeSystem.sanityTickLenght = sanityTickLenght;

        TimeSystem.OnSanityTick += () => Tick();

        roomSystem = GameObject.Find("Room System").GetComponent<RoomSystem>();

        PillPile.OnPickUp += () =>
        {
            sanity += UnityEngine.Random.Range(minPillRegen, maxPillRegen);
            sanity = Mathf.Clamp(sanity, 0f, maxSanity);
        };

        disturbanceWeightsSum = usualDisturbanceWeight + lightDisturbanceWeight + doorDisturbanceWeight;

        actualDisturbanceAmplifier = 0;
        sanity = maxSanity;

        DisturbTimer();
        DangerTimer();
        IntruderTimer();

        OnDangerChange = () =>
        {
            if (dangerLevel != 0)
            {
                Invoke(nameof(GameOverTimer), gameOverTimer);
            }
            else if (canGameOver)
            {
                canGameOver = false;
            }
        };
    }

    private void DisturbTimer()
    {
        canDisturb = true;
    }

    private void DangerTimer()
    {
        canDanger = true;
    }

    private void IntruderTimer()
    {
        canIntruder = true;
    }

    private void GameOverTimer()
    {
        if (dangerLevel != 0 && !canGameOver)
        {
            canGameOver = true;
        }
    }

    private void Tick()
    {
        actualSanityDropRate = (sanityDropRate * actualDisturbanceAmplifier) + (roomSystem.isInLight ? -sanityRegenInLight : sanityDropInDark);
        sanity -= actualSanityDropRate;
        sanity = Mathf.Clamp(sanity, 0f, maxSanity);
        actualSanityAmplifier = (1 - (sanity / 100)) * sanityAmplifier;

        float s = UnityEngine.Random.Range(0f, 100f);

        actualDisturbanceChance = Mathf.Lerp(startDisturbanceChance, endDisturbanceChance, timeSystem.GetProgress());
        if (s < actualDisturbanceChance && canDisturb)
        {
            Disturb();
            canDisturb = false;
            Invoke(nameof(DisturbTimer), disturbCooldown);
        }

        actualDangerChance = Mathf.Lerp(startDangerChance, endDangerChance, timeSystem.GetProgress());
        actualDangerChance += actualSanityAmplifier;
        if (s < actualDangerChance && canDanger)
        {
            Danger();
            canDanger = false;
            Invoke(nameof(DangerTimer), dangerCooldown);
        }

        if (!isIntruderActive)
        {
            isIntruderActive = timeSystem.durationHours * timeSystem.GetProgress() >= intruderHour;
        }
        else
        {
            actualIntruderChance = Mathf.Lerp(startIntruderChance, endIntruderChance, timeSystem.GetIntruderProgress(intruderHour));
            actualIntruderChance += actualSanityAmplifier;
            if (s < actualIntruderChance && canIntruder)
            {
                Intruder();
                canIntruder = false;
                Invoke(nameof(IntruderTimer), intruderCooldown);
            }
        }

        actualGameOverChance = dangerLevel * gameOverChance;
        if (s < actualGameOverChance && canGameOver)
        {
            GameOver();
        }

        UpdateDebugUI();
    }

    public void UpdateActualDisturbanceAmplifier()
    {
        actualDisturbanceAmplifier = disturbanceLevel / disturbanceMaxLevel;
    }

    private void Disturb()
    {
        float r = UnityEngine.Random.Range(0f, disturbanceWeightsSum);

        if (r < usualDisturbanceWeight)
        {
            UsualDisturb();
        }
        else if (r < usualDisturbanceWeight + lightDisturbanceWeight)
        {
            LightDisturb();
            sanity -= lightSanityDecrement;
            if (sanity < 0)
            {
                sanity = 0;
            }
        }
        else
        {
            DoorDisturb();
            sanity -= doorSanityDecrement;
            if (sanity < 0)
            {
                sanity = 0;
            }
        }
    }

    private void UsualDisturb()
    {
        List<DisturbanceObject> inactives = disturbanceObjects.Where(currentObject => !currentObject.isActive).ToList();

        if (inactives.Count() == 0)
        {
            return;
        }

        inactives[UnityEngine.Random.Range(0, inactives.Count())].Disturb();
    }

    private void LightDisturb()
    {
        List<LightSwitchObject> lightsOn = lights.Where(currentObject => currentObject.isOn).ToList();

        if (lightsOn.Count() == 0)
        {
            return;
        }

        lightsOn[UnityEngine.Random.Range(0, lightsOn.Count())].Switch();
    }

    private void DoorDisturb()
    {
        List<Door> openDoors = doors.Where(currentObject => currentObject.isOpen).ToList();

        if (openDoors.Count() == 0)
        {
            return;
        }

        openDoors[UnityEngine.Random.Range(0, openDoors.Count())].Interact();
    }

    private void Danger()
    {
        List<DangerObject> dangers = dangerObjects.Where(currentObject => currentObject.stage < 2).ToList();

        if (dangers.Count() == 0)
        {
            return;
        }

        dangers[UnityEngine.Random.Range(0, dangers.Count())].IncreaseStage();
    }

    private void Intruder()
    {
        IntruderController.Instance.Intrude();
    }

    public void GameOver()
    {
        Debug.Log("Game is over");
    }

    public void ReduceSanity(float s)
    {
        sanity -= s;
        sanity = Mathf.Clamp(sanity, 0f, maxSanity);
    }

    private void UpdateDebugUI()
    {
        DebugUI.instance.disturbanceLevel = disturbanceLevel;
        DebugUI.instance.disturbanceMaxLevel = disturbanceMaxLevel;

        DebugUI.instance.actualDisturbanceAmplifier = actualDisturbanceAmplifier;

        DebugUI.instance.sanity = sanity;
        DebugUI.instance.maxSanity = maxSanity;
        DebugUI.instance.actualSanityDropRate = actualSanityDropRate;
        DebugUI.instance.actualSanityAmplifier = actualSanityAmplifier;

        DebugUI.instance.isIntruderActive = isIntruderActive;

        DebugUI.instance.actualDisturbanceChance = actualDisturbanceChance;
        DebugUI.instance.actualDangerChance = actualDangerChance;
        DebugUI.instance.actualIntruderChance = actualIntruderChance;
        DebugUI.instance.actualGameOverChance = actualGameOverChance;

        DebugUI.instance.canDisturb = canDisturb;
        DebugUI.instance.canDanger = canDanger;
        DebugUI.instance.canIntruder = canIntruder;
        DebugUI.instance.canGameOver = canGameOver;
    }
}
