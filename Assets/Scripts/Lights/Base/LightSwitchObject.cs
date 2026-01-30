using System;
using DG.Tweening;
using UnityEngine;

public class LightSwitchObject : SwitchObject, InteractableObject
{
    [SerializeField] private LightObject lightObject;

    private PowerSystem powerSystem;

    protected float switchDuration;

    protected Action switchOnAction;
    protected Action switchOffAction;

    public void Interact()
    {
        Switch();
    }

    protected void SwitchLight()
    {
        lightObject.Switch();
    }

    protected void TrySwitchLight()
    {
        if (!powerSystem.hasPower)
        {
            return;
        }

        lightObject.Switch();
    }

    protected void SetUp()
    {
        powerSystem = GameObject.Find("Power System").GetComponent<PowerSystem>();

        PowerSystem.OnStateChange += () => Refresh();

        turnOnAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Pause();
            
            sequence.AppendCallback(() => switchOnAction());
            sequence.AppendInterval(switchDuration);
            sequence.AppendCallback(() => EndSwitchSequence());
            
            sequence.Play();
        };

        turnOffAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Pause();
            
            sequence.AppendCallback(() => switchOffAction());
            sequence.AppendInterval(switchDuration);
            sequence.AppendCallback(() => EndSwitchSequence());
            
            sequence.Play();
        };
    }

    public void Refresh()
    {
        if (powerSystem.hasPower && isOn && !lightObject.isOn)
        {
            lightObject.Switch();
        }
        else if (!powerSystem.hasPower && lightObject.isOn)
        {
            lightObject.Switch();
        }
    }
}
