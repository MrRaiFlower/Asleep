using UnityEngine;

public class DisturbObject : MonoBehaviour
{
    [SerializeField] SwitchObject switchObject;
    [Space(8)]
    [SerializeField] int disturbRate;

    [HideInInspector] public bool isActive;

    private void Start()
    {
        SanitySystem.Instance.disturbObjects.Add(this);
        SanitySystem.Instance.disturbMaxLevel += disturbRate;
        switchObject.OnSwitch += () => { isActive = !isActive; SanitySystem.Instance.disturbLevel += isActive ? disturbRate : -disturbRate; };
    }

    public void Disturb()
    {
        switchObject.Switch();
    }

}
