using System;
using UnityEngine;

public class DangerObject : MonoBehaviour
{
    [HideInInspector] public int stage;

    protected Action Tr0to1;
    protected Action Tr1to2;

    protected Action Tr2to0;
    protected Action Tr1to0;

    protected bool isSwitching;

    protected void SetUp()
    {
        SanitySystem.Instance.dangerObjects.Add(this);
    }

    public void IncreaseStage()
    {
        if (isSwitching)
        {
            return;
        }

        if (stage == 0)
        {
            stage += 1;
            SanitySystem.Instance.dangerLevel += 0.1f;
            SanitySystem.OnDangerChange.Invoke();
            Tr0to1.Invoke();
        }
        else if (stage == 1)
        {
            stage += 1;
            SanitySystem.Instance.dangerLevel += 0.9f;
            SanitySystem.OnDangerChange.Invoke();
            Tr1to2.Invoke();
        }
    }

    public void DecreaseStage()
    {
        if (isSwitching)
        {
            return;
        }

        if (stage == 2)
        {
            stage -= 2;
            SanitySystem.Instance.dangerLevel -= 0.9f;
            SanitySystem.OnDangerChange.Invoke();
            Tr2to0.Invoke();
        }
        else if (stage == 1)
        {
            stage -= 1;
            SanitySystem.Instance.dangerLevel -= 0.1f;
            SanitySystem.OnDangerChange.Invoke();
            Tr1to0.Invoke();
        }
    }
}
