using System;
using System.Collections;
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
            EndSwitchSequence(sequence);
            
            sequence.Play();
        };

        turnOffAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Pause();
            
            sequence.AppendCallback(() => switchOffAction());
            sequence.AppendInterval(switchDuration);
            EndSwitchSequence(sequence);
            
            sequence.Play();
        };
    }

    public void Refresh()
    {
        if (powerSystem.hasPower && isOn && !lightObject.isOn)
        {
            StartCoroutine(WaitToSwitch());
        }
        else if (!powerSystem.hasPower && lightObject.isOn)
        {
            StartCoroutine(WaitToSwitch());
        }
    }

    private IEnumerator WaitToSwitch()
    {
        while (lightObject.isSwitching)
        {
            yield return null;
        }

        lightObject.Switch();
    }
}
