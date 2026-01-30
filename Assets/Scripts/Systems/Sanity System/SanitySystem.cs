using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SanitySystem : MonoBehaviour
{
    [SerializeField] public float sanityTickLenght;
    [Space(8)]
    [SerializeField] public float startDisturbanceChance;
    [SerializeField] public float endDisturbanceChance;
    [Space(8)]
    [SerializeField] public float maxSanity;
    [SerializeField] public float sanityDropRate;
    [SerializeField] public float sanityAmplifier;
    [SerializeField] public float sanityDropInDark;
    [SerializeField] public float sanityRegenInLight;
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
    [HideInInspector] public List<DangerObject> dangerObjects = new List<DangerObject>();
    // [HideInInspector] public List<DisturbanceObject> intruders = new List<DisturbanceObject>();

    [HideInInspector] public int disturbanceLevel;
    [HideInInspector] public int disturbanceMaxLevel;

    private float actualDisturbanceAmplifier;

    private float sanity;
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

    private void Start()
    {
        timeSystem = GameObject.Find("Time System").GetComponent<TimeSystem>();
        timeSystem.sanityTickLenght = sanityTickLenght;

        TimeSystem.OnSanityTick += () => Tick();

        roomSystem = GameObject.Find("Room System").GetComponent<RoomSystem>();

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
        actualSanityAmplifier =  (1 - (sanity / 100)) * sanityAmplifier;

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
            if (s < actualIntruderChance  && canIntruder)
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
    
    public void UpdateActualDisturbanceAmplifier()
    {
        actualDisturbanceAmplifier = disturbanceLevel / disturbanceMaxLevel;
    }

    private void Disturb()
    {
        List<DisturbanceObject> inactives = disturbanceObjects.Where(currentObject => !currentObject.isActive).ToList();

        if (inactives.Count() == 0)
        {
            return;
        }

        inactives[UnityEngine.Random.Range(0, inactives.Count())].Disturb();
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
        
    }

    private void GameOver()
    {
        Debug.Log("Game is over");
    }
}
