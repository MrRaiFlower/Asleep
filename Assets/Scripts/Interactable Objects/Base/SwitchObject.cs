using System;
using UnityEngine;

public class SwitchObject : MonoBehaviour
{
    public event Action OnSwitch;

    [HideInInspector] public bool isOn;
    protected bool isSwitching;

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

    protected void EndSwitchSequence()
    {
        isSwitching = false;
        isOn = !isOn;
    }
}
