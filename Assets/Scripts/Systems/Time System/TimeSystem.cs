using System;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    public static event Action OnClockTick;
    public static event Action OnSanityTick;

    [SerializeField] public float realDurationMinutes;
    [SerializeField] public float durationHours;

    [SerializeField] public float startHours;
    [SerializeField] public float scale;

    [HideInInspector] public float realCurrentSeconds;
    [HideInInspector] public int currentMinutes;

    [HideInInspector] public float clockTickLenght;
    [HideInInspector] public float sanityTickLenght;

    private float durationMinutes;

    private float delta;

    private float clockTickCounter;
    private float sanityTickCounter;

    private SanitySystem sanitySystem;
    private int timeTillIntruderMinutes;

    private void Start()
    {
        sanitySystem = GameObject.Find("Sanity System").GetComponent<SanitySystem>();

        durationMinutes = durationHours * 60f;
        clockTickLenght = realDurationMinutes / durationMinutes * 60f;
        currentMinutes += (int) (startHours * 60);
    }

    private void Update()
    {
        delta = scale * Time.deltaTime;

        realCurrentSeconds += delta;
        clockTickCounter += delta;
        sanityTickCounter += delta;
        
        if (clockTickCounter > clockTickLenght)
        {
            clockTickCounter = 0f;
            currentMinutes += 1;
            OnClockTick?.Invoke();
        }

        if (sanityTickCounter > sanityTickLenght)
        {
            sanityTickCounter = 0f;
            OnSanityTick?.Invoke();
        }

        DebugUI.instance.currentTime = String.Format("{0:00}:{1:00}", 
        currentMinutes / 60, 
        currentMinutes % 60);

        DebugUI.instance.endTime = String.Format("{0:00}:{1:00}", 
        (int) (startHours + durationHours), 
        (int) ((startHours + durationHours - (int) (startHours + durationHours)) * 60));

        DebugUI.instance.timeScale = scale.ToString();

        timeTillIntruderMinutes = ((int) (sanitySystem.intruderHour * 60)) - currentMinutes;
        DebugUI.instance.timeTillIntruder = String.Format("{0:00}:{1:00}", 
            timeTillIntruderMinutes / 60, 
            timeTillIntruderMinutes % 60
        );
    }

    public void SetSanityTickLenght(float t)
    {
        sanityTickLenght = t;
    }

    public float GetProgress()
    {
        return realCurrentSeconds / 60f / realDurationMinutes;
    }

    public float  GetIntruderProgress(float intruderHour)
    {
        return (currentMinutes / 60f - intruderHour) / (durationHours - intruderHour);
    }
    
}
