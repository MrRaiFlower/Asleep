using System;
using DG.Tweening;
using UnityEngine;

public class BreakerSwitch : SwitchObject, InteractableObject
{
    public event Action onSwitch;

    [SerializeField] AudioSource clickSound;

    private static float animationDuration = 0.1f;

    private Vector3 switchOnRotation;
    private Vector3 switchOffRotation;
    
    void Start()
    {
        switchOnRotation = transform.localRotation.eulerAngles;
        switchOffRotation = Vector3.zero;

        isOn = true;

        turnOnAction = () =>
        {
            Sequence turnOnSequence = DOTween.Sequence();
            turnOnSequence.SetEase(Ease.InOutExpo);
            turnOnSequence.Pause();
            turnOnSequence.Append(transform.DOLocalRotate(switchOnRotation, animationDuration));
            turnOnSequence.AppendInterval(animationDuration / 2f);
            turnOnSequence.AppendCallback(() => clickSound.Play());
            turnOnSequence.AppendCallback(() => {isSwitching = false;  isOn = !isOn; onSwitch?.Invoke();});
            turnOnSequence.Play();
        };

        turnOffAction = () =>
        {
            Sequence turnOffSequence = DOTween.Sequence();
            turnOffSequence.SetEase(Ease.InOutExpo);
            turnOffSequence.Pause();
            turnOffSequence.Append(transform.DOLocalRotate(switchOffRotation, animationDuration));
            turnOffSequence.AppendInterval(animationDuration / 2f);
            turnOffSequence.AppendCallback(() => clickSound.Play());
            turnOffSequence.AppendCallback(() => {isSwitching = false;  isOn = !isOn; onSwitch?.Invoke();});
            turnOffSequence.Play();
        };   
    }

    public void Interact()
    {
        Switch();
    }
}
