using DG.Tweening;
using UnityEngine;

public class LightObject : SwitchObject
{
    [SerializeField] Light[] lights;
    [SerializeField] MeshRenderer[] emissives;

    [SerializeField] private RoomSystem.roomsEnum room;

    [SerializeField] private float powerDrain;

    private PowerSystem powerSystem;
    private RoomSystem roomSystem;

    private float lightIntencity;
    private float emissionIntencity;

    private void Start()
    {
        powerSystem = GameObject.Find("Power System").GetComponent<PowerSystem>();
        roomSystem = GameObject.Find("Room System").GetComponent<RoomSystem>();

        lightIntencity = lights[0].intensity;
        emissionIntencity = emissives[0].GetComponent<MeshRenderer>().material.GetFloat("_Intensity");

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].intensity = 0f;
        }

        for (int i = 0; i < emissives.Length; i++)
        {
            emissives[i].material.SetFloat("_Intensity", 0f);
        }

        turnOnAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Pause();
            
            sequence.AppendCallback(() => powerSystem.drain += powerDrain);

            for (int i = 0; i < lights.Length; i++)
            {
                sequence.Join(lights[i].DOIntensity(lightIntencity, 0.05f));
            }

            for (int i = 0; i < emissives.Length; i++)
            {
                sequence.Join(emissives[i].material.DOFloat(emissionIntencity, "_Intensity", 0.05f));
            }

            sequence.AppendCallback(() => {EndSwitchSequence(); roomSystem.SwitchLight(room, true);});
            
            sequence.Play();
        };

        turnOffAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Pause();

            sequence.AppendCallback(() => powerSystem.drain -= powerDrain);
            
            for (int i = 0; i < lights.Length; i++)
            {
                sequence.Join(lights[i].DOIntensity(0f, 0.05f));
            }

            for (int i = 0; i < emissives.Length; i++)
            {
                sequence.Join(emissives[i].material.DOFloat(0f, "_Intensity", 0.05f));
            }

            sequence.AppendCallback(() => {EndSwitchSequence(); roomSystem.SwitchLight(room, false);});
            
            sequence.Play();
        };
    }
}
