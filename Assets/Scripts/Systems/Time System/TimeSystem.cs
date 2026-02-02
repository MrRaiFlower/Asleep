using System;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    public static TimeSystem Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    public static event Action OnClockTick;

    [SerializeField] public float realDurationMinutes;
    [SerializeField] public float durationHours;

    [SerializeField] public float startHour;

    [HideInInspector] public float realCurrentSeconds;
    [HideInInspector] public int currentMinutes;

    [HideInInspector] public float minuteTickLenght;

    private float durationMinutes;

    private float delta;

    private float minuteTickCounter;

    private void Start()
    {
        durationMinutes = durationHours * 60f;
        minuteTickLenght = realDurationMinutes / durationMinutes * 60f;
        currentMinutes += (int) (startHour * 60);
    }

    private void Update()
    {
        realCurrentSeconds += Time.deltaTime;
        minuteTickCounter += Time.deltaTime;
        
        if (minuteTickCounter > minuteTickLenght)
        {
            minuteTickCounter = 0f;
            currentMinutes += 1;
            OnClockTick?.Invoke();
        }
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
