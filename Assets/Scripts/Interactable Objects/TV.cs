using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TV : SwitchObject, InteractableObject
{
    [SerializeField] private GameObject _normalScreen;
    [SerializeField] private GameObject _staticScreen;
    [Space(16)]
    [SerializeField] private List<Light> _lights = new List<Light>();
    [Space(16)]
    [SerializeField] private AudioSource _switchSound;
    [SerializeField] private AudioSource _staticSound;
    [Space(16)]
    [SerializeField] private float _switchDuration;

    private float _lightsIntensity;
    private float _staticVolume;

    private void Start()
    {
        _lightsIntensity = _lights[0].intensity;
        foreach (Light light in _lights)
        {
            light.intensity = 0f;
        }

        _staticVolume = _staticSound.volume;
        _staticSound.volume = 0f;

        turnOnAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.SetEase(Ease.InOutSine);
            sequence.Pause();

            sequence.AppendCallback(() => { 
                _switchSound.Play(); 
                _normalScreen.SetActive(false); 
                _staticScreen.SetActive(true); 
                foreach (Light light in _lights)
                {
                    light.intensity = _lightsIntensity;
                }
                });
            sequence.Join(DOTween.To(() => _staticSound.volume, x => _staticSound.volume = x, _staticVolume, _switchDuration));
            EndSwitchSequence(sequence);

            sequence.Play();
        };

        turnOffAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.SetEase(Ease.InOutSine);
            sequence.Pause();

            sequence.AppendCallback(() => { 
                _switchSound.Play(); 
                _normalScreen.SetActive(true); 
                _staticScreen.SetActive(false); 
                foreach (Light light in _lights)
                {
                    light.intensity = 0f;
                }
                });
            sequence.Join(DOTween.To(() => _staticSound.volume, x => _staticSound.volume = x, 0f, _switchDuration));
            EndSwitchSequence(sequence);

            sequence.Play();
        };
    }

    public void Interact()
    {
        Switch();
    }
}
