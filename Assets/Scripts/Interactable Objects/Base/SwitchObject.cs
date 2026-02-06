using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SwitchObject : MonoBehaviour
{
    public event Action OnSwitch;

    [HideInInspector] public bool isOn;
    [HideInInspector] public bool isSwitching;

    protected Action turnOnAction;
    protected Action turnOffAction;

    public void Switch()
    {
        if (isSwitching)
        {
            return;
        }

        isSwitching = true;

        OnSwitch?.Invoke();

        if (isOn)
        {
            turnOffAction.Invoke();
        }
        else
        {
            turnOnAction.Invoke();
        }
    }

    protected void EndSwitchSequence(Sequence sequence)
    {
        sequence.AppendCallback(EndSwitch);
    }

    protected void EndSwitch()
    {
        isSwitching = false; isOn = !isOn;
    }
}
