using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FurnitureLight : SwitchObject
{
    [SerializeField] List<Light> _lights = new List<Light>();

    private float _intensity;
    
    private void Start()
    {
        _intensity = _lights[0].intensity;
        foreach (Light light in _lights)
        {
            light.DOIntensity(0f, 0.1f);
        }
            
        turnOnAction = () =>
        {
            foreach (Light light in _lights)
            {
                light.DOIntensity(_intensity, 0.1f);
            }
            EndSwitch();
        };

        turnOffAction = () =>
        {
            foreach (Light light in _lights)
            {
                light.DOIntensity(0f, 0.1f);
            }
            EndSwitch();
        };
    }
}
