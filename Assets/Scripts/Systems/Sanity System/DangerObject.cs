using System;
using UnityEngine;

public class DangerObject : MonoBehaviour
{
    private SanitySystem sanitySystem;

    [HideInInspector] public int stage;

    protected Action Tr0to1;
    protected Action Tr1to2;

    protected Action Tr2to0;
    protected Action Tr1to0;

    protected bool isSwitching;

    protected void SetUp()
    {
        sanitySystem = GameObject.Find("Sanity System").GetComponent<SanitySystem>();
        sanitySystem.dangerObjects.Add(this);
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
            Tr0to1.Invoke();
        }
        else if (stage == 1)
        {
            stage += 1;
            sanitySystem.dangerLevel += 1f;
            sanitySystem.OnDangerChange.Invoke();
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
            sanitySystem.dangerLevel -= 1f;
            sanitySystem.OnDangerChange.Invoke();
            Tr2to0.Invoke();
        }
        else if (stage == 1)
        {
            stage -= 1;
            Tr1to0.Invoke();
        }
    }
}
