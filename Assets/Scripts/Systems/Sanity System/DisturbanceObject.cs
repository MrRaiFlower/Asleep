using UnityEngine;

public class DisturbanceObject : MonoBehaviour
{
    [SerializeField] SwitchObject switchObject;
    [Space(8)]
    [SerializeField] int disturbanceRate;

    private SanitySystem sanitySystem;

    [HideInInspector] public bool isActive;

    private void Start()
    {
        sanitySystem = GameObject.Find("Sanity System").GetComponent<SanitySystem>();
        sanitySystem.disturbanceObjects.Add(this);
        sanitySystem.disturbanceMaxLevel += disturbanceRate;
        switchObject.OnSwitch += () => {isActive = !isActive; sanitySystem.disturbanceLevel += isActive ? disturbanceRate : -disturbanceRate; sanitySystem.UpdateActualDisturbanceAmplifier();};
    }

    public void Disturb()
    {
        switchObject.Switch();
    }
    
}
