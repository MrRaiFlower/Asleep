using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class SanitySystem : MonoBehaviour
{
    public static Action OnDangerChange;

    public static SanitySystem Instance;

    [SerializeField] public float sanityDropRate;
    [SerializeField] public float minPillRegen;
    [SerializeField] public float maxPillRegen;
    [Space(16)]
    [SerializeField] public float intruderHour;
    [Space(16)]
    [SerializeField] public float dangerTreshold;
    [Space(16)]
    [SerializeField] public SanityEventParameters disturbParameters;
    [SerializeField] public SanityEventParameters dangerParameters;
    [SerializeField] public SanityEventParameters intruderParameters;
    [SerializeField] public SanityEventParameters gameOverParameters;

    [HideInInspector] public List<DisturbObject> disturbObjects = new List<DisturbObject>();
    [HideInInspector] public List<DangerObject> dangerObjects = new List<DangerObject>();
    [HideInInspector] public List<Intruder> intruders = new List<Intruder>();

    [HideInInspector] public int disturbLevel;
    [HideInInspector] public int disturbMaxLevel;

    [HideInInspector] public bool isActive;

    [HideInInspector] public float sanity;
    private float sanityDelta;

    private bool isIntruderActive;

    private float disturbInterval;
    private float dangerInterval;
    private float intruderInterval;
    private float gameOverInterval;

    private float timeTillDisturb;
    private float timeTillDanger;
    private float timeTillIntruder;
    private float timeTillGameOver;

    [HideInInspector] public float dangerLevel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OnDangerChange += () =>
        {
            if (dangerLevel >= dangerTreshold)
            {
                StartCoroutine(GameOverClock());
            }
        };

        PillPile.OnPickUp += () =>
        {
            ChangeSanity(UnityEngine.Random.Range(minPillRegen, maxPillRegen));
        };

        sanity = 100f;

        StartCoroutine(Activate());
    }

    private void Update()
    {
        if (!isIntruderActive && TimeSystem.Instance.currentMinutes >= intruderHour * 60f)
        {
            isIntruderActive = true;
        }

        sanityDelta = -sanityDropRate * disturbLevel / disturbMaxLevel;
        ChangeSanity(sanityDelta);

        timeTillDisturb = timeTillDisturb - Time.deltaTime >= 0f ? timeTillDisturb - Time.deltaTime : 0f;
        timeTillDanger = timeTillDanger - Time.deltaTime >= 0f ? timeTillDanger - Time.deltaTime : 0f;
        timeTillIntruder = timeTillIntruder - Time.deltaTime >= 0f ? timeTillIntruder - Time.deltaTime : 0f;
        timeTillGameOver = timeTillGameOver - Time.deltaTime >= 0f ? timeTillGameOver - Time.deltaTime : 0f;
    }

    public IEnumerator Activate()
    {
        isActive = true;

        StartCoroutine(Disturb());
        StartCoroutine(Danger());

        yield return new WaitUntil(() => isIntruderActive);
        StartCoroutine(Intruder());
    }

    private IEnumerator Disturb()
    {
        disturbInterval = GetNewInterval(disturbParameters, TimeSystem.Instance.GetProgress());
        timeTillDisturb = disturbInterval;
        yield return new WaitForSeconds(disturbInterval);

        List<DisturbObject> inactives = disturbObjects.Where(currentObject => !currentObject.isActive).ToList();

        if (inactives.Count() != 0)
        {
            inactives[UnityEngine.Random.Range(0, inactives.Count())].Disturb();
        }

        StartCoroutine(Disturb());
    }

    private IEnumerator Danger()
    {
        dangerInterval = GetNewInterval(dangerParameters, TimeSystem.Instance.GetProgress());
        timeTillDanger = dangerInterval;
        yield return new WaitForSeconds(dangerInterval);

        List<DangerObject> dangers = dangerObjects.Where(currentObject => currentObject.stage < 2).ToList();

        if (dangers.Count() != 0)
        {
            dangers[UnityEngine.Random.Range(0, dangers.Count())].IncreaseStage();
        }

        StartCoroutine(Danger());
    }

    private IEnumerator Intruder()
    {
        intruderInterval = GetNewInterval(intruderParameters, TimeSystem.Instance.GetIntruderProgress(intruderHour));
        timeTillIntruder = intruderInterval;
        yield return new WaitForSeconds(intruderInterval);

        intruders[UnityEngine.Random.Range(0, intruders.Count)].Activate();

        StartCoroutine(Intruder());
    }

    private IEnumerator GameOverClock()
    {
        gameOverInterval = GetNewInterval(gameOverParameters, TimeSystem.Instance.GetProgress());
        timeTillGameOver = gameOverInterval;
        yield return new WaitForSeconds(gameOverInterval);

        if (dangerLevel >= dangerTreshold)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Debug.Log("Game is over");
    }

    public void ChangeSanity(float s)
    {
        sanity += s;
        sanity = Mathf.Clamp(sanity, 0f, 100f);
    }

    private float GetNewInterval(SanityEventParameters parameters, float t)
    {
        float normalDev = UnityEngine.Random.Range(-parameters.normalDev, parameters.normalDev);
        float sanityDev = -parameters.sanityDev * (1f - (sanity / 100f));
        float totalDev = normalDev + sanityDev;
        return Mathf.Lerp(parameters.startInterval, parameters.endInterval, t) * (1 + totalDev);
    }
}
