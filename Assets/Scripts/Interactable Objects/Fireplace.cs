using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Fireplace : SwitchObject, InteractableObject
{
    [SerializeField] private AudioSource _fireSound;
    [SerializeField] private AudioSource _clickSound;
    [Space(16)]
    [SerializeField] private List<ParticleSystem> _particles = new List<ParticleSystem>();
    [Space(16)]
    [SerializeField] private List<Light> _ligths = new List<Light>();
    [Space(16)]
    [SerializeField] private float _switchDuration;

    private List<ParticleSystem.EmissionModule> _particlesEmissions = new List<ParticleSystem.EmissionModule>();

    private List<float> _particlesDefaultEmissions = new List<float>();

    private float _fireVolume;
    private float _lightsIntensity;

    private void Start()
    {
        isOn = true;

        foreach (ParticleSystem system in _particles)
        {
            _particlesEmissions.Add(system.emission);
            _particlesDefaultEmissions.Add(system.emission.rateOverTimeMultiplier);
        }

        _fireVolume = _fireSound.volume;
        _lightsIntensity = _ligths[0].intensity;

        PowerSystem.OnStateChange += () => Refresh();

        turnOnAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.SetEase(Ease.InOutSine);
            sequence.Pause();

            sequence.Append(DOTween.To(() => _fireSound.volume, x => _fireSound.volume = x, _fireVolume, _switchDuration));
            sequence.JoinCallback(_clickSound.Play);
            for (int i = 0; i < _particles.Count; i++)
            {
                ParticleSystem.EmissionModule emission = _particlesEmissions[i];
                sequence.Join(DOTween.To(() => emission.rateOverTimeMultiplier, x => emission.rateOverTimeMultiplier = x, _particlesDefaultEmissions[i], _switchDuration));
            }
            foreach (Light light in _ligths)
            {
                sequence.Join(DOTween.To(() => light.intensity, x => light.intensity = x, _lightsIntensity, _switchDuration));
            }
            EndSwitchSequence(sequence);

            sequence.Play();
        };

        turnOffAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.SetEase(Ease.InOutSine);
            sequence.Pause();

            sequence.Append(DOTween.To(() => _fireSound.volume, x => _fireSound.volume = x, 0f, _switchDuration));
            sequence.JoinCallback(_clickSound.Play);
            for (int i = 0; i < _particles.Count; i++)
            {
                ParticleSystem.EmissionModule emission = _particlesEmissions[i];
                sequence.Join(DOTween.To(() => emission.rateOverTimeMultiplier, x => emission.rateOverTimeMultiplier = x, 0f, _switchDuration));
            }
            foreach (Light light in _ligths)
            {
                sequence.Join(DOTween.To(() => light.intensity, x => light.intensity = x, 0f, _switchDuration));
            }
            EndSwitchSequence(sequence);

            sequence.Play();
        };
    }

    public void Interact()
    {
        TrySwitch();
    }

    private void TrySwitch()
    {
        if (!PowerSystem.Instance.hasPower)
        {
            return;
        }

       Switch();
    }

    public void Refresh()
    {
        if (PowerSystem.Instance.hasPower && !isOn)
        {
            StartCoroutine(WaitToSwitch());
        }
        else if (!PowerSystem.Instance.hasPower && isOn)
        {
            StartCoroutine(WaitToSwitch());
        }
    }

    private IEnumerator WaitToSwitch()
    {
        while (isSwitching)
        {
            yield return null;
        }

        Switch();
    }
}
