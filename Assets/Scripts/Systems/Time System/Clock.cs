using DG.Tweening;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private GameObject minuteArrow;
    [SerializeField] private GameObject hourArrow;

    private TimeSystem timeSystem;

    private void Start()
    {
        timeSystem = GameObject.Find("Time System").GetComponent<TimeSystem>();

        TimeSystem.OnClockTick += () => Tick();

        minuteArrow.transform.Rotate(Vector3.left, 6f * timeSystem.startHours * 60f, Space.Self);
        hourArrow.transform.Rotate(Vector3.left, 0.5f * timeSystem.startHours * 60f, Space.Self);
    }

    private void Tick()
    {
        minuteArrow.transform.DOLocalRotate(Vector3.left * 6f, 0.05f, RotateMode.LocalAxisAdd).SetEase(Ease.InExpo);
        hourArrow.transform.DOLocalRotate(Vector3.left * 0.5f, 0.05f, RotateMode.LocalAxisAdd);
    }
}
