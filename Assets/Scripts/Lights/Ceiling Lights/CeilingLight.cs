using DG.Tweening;
using UnityEngine;

public class CeilingLight : MonoBehaviour
{
    [SerializeField] private Light[] lights;
    [SerializeField] private Renderer[] lightsModels;

    [SerializeField] private float intensity;
    [SerializeField] private float powerDrain;

    [SerializeField] private RoomSystem.roomsEnum room;

    private PowerSystem powerSystem;
    private RoomSystem roomSystem;

    [HideInInspector] public bool isOn;

    void Start()
    {
        powerSystem = GameObject.Find("Power System").GetComponent<PowerSystem>();
        roomSystem = GameObject.Find("Room System").GetComponent<RoomSystem>();

        foreach (Light light in lights)
        {
            light.intensity = 0.0f;
        }
        foreach (Renderer lightModel in lightsModels)
        {
            lightModel.material.SetColor("_EmissionColor", Color.black);
        }
    }

    public void TurnOn()
    {
        powerSystem.drain += powerDrain;

        Sequence tweenSequence = DOTween.Sequence();
        tweenSequence.Pause();

        foreach (Light light in lights)
        {
            tweenSequence.Append(light.DOIntensity(intensity, 0.1f));
        }
        foreach (Renderer lightModel in lightsModels)
        {
            tweenSequence.Join(lightModel.material.DOColor(Color.white, "_EmissionColor", 0.1f));
        }
        tweenSequence.AppendCallback(() => {isOn = true; roomSystem.SwitchLight(room, true);});

        tweenSequence.Play();
        
    }

    public void TurnOff()
    {
        powerSystem.drain -= powerDrain;

        Sequence tweenSequence = DOTween.Sequence();
        tweenSequence.Pause();

        foreach (Light light in lights)
        {
            tweenSequence.Append(light.DOIntensity(0.0f, 0.1f));
        }
        foreach (Renderer lightModel in lightsModels)
        {
            tweenSequence.Join(lightModel.material.DOColor(Color.black, "_EmissionColor", 0.1f));
        }
        tweenSequence.AppendCallback(() => {isOn = false; roomSystem.SwitchLight(room, false);});

        tweenSequence.Play();
    }

    public void Switch()
    {
        if (isOn)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }
}
