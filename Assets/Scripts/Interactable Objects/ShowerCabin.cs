using System;
using DG.Tweening;
using UnityEngine;

public class ShowerCabin : SwitchObject, InteractableObject
{
    [SerializeField] private ParticleSystem showerParticles;
    [SerializeField] private ParticleSystem splahesParticles;
    [SerializeField] private GameObject handleObject;

    [SerializeField] private AudioSource showerSound;

    private Vector3 handleDefaultRotation;
    private Vector3 handlePressedRotation;

    private float showerDefaultEmissionRate;
    private float splashesDefaultEmissionRate;

    private ParticleSystem.EmissionModule showerEmission;
    private ParticleSystem.EmissionModule splashesEmission;

    private void Start()
    {
        handleDefaultRotation = handleObject.transform.localRotation.eulerAngles;
        handlePressedRotation = handleDefaultRotation - Vector3.right * 10f;

        showerEmission = showerParticles.emission;
        splashesEmission = splahesParticles.emission;

        showerDefaultEmissionRate = showerEmission.rateOverTimeMultiplier;
        splashesDefaultEmissionRate = splashesEmission.rateOverTimeMultiplier;

        showerEmission.rateOverTimeMultiplier = 0f;
        splashesEmission.rateOverTimeMultiplier = 0f;
        showerSound.volume = 0.0f;

        turnOnAction = () =>
        {
            Sequence turnOnSequence = DOTween.Sequence();
            turnOnSequence.SetEase(Ease.InOutSine);

            turnOnSequence.Pause();
            turnOnSequence.Append(handleObject.transform.DOLocalRotate(handlePressedRotation, 0.2f));

            turnOnSequence.Join(DOTween.To(() => showerSound.volume, x => showerSound.volume = x, 0.2f, 1f));
            turnOnSequence.Join(DOTween.To(() => showerEmission.rateOverTimeMultiplier, x => showerEmission.rateOverTimeMultiplier = x, showerDefaultEmissionRate, 1f));
            turnOnSequence.Join(DOTween.To(() => splashesEmission.rateOverTimeMultiplier, x => splashesEmission.rateOverTimeMultiplier = x, splashesDefaultEmissionRate, 1f));

            turnOnSequence.JoinCallback(() => { isSwitching = false; isOn = !isOn; });

            turnOnSequence.Play();
        };

        turnOffAction = () =>
        {
            Sequence turnOffSequence = DOTween.Sequence();
            turnOffSequence.SetEase(Ease.InOutSine);

            turnOffSequence.Pause();
            turnOffSequence.Append(handleObject.transform.DOLocalRotate(handleDefaultRotation, 0.2f));

            turnOffSequence.Join(DOTween.To(() => showerSound.volume, x => showerSound.volume = x, 0f, 1f));
            turnOffSequence.Join(DOTween.To(() => showerEmission.rateOverTimeMultiplier, x => showerEmission.rateOverTimeMultiplier = x, 0f, 1f));
            turnOffSequence.Join(DOTween.To(() => splashesEmission.rateOverTimeMultiplier, x => splashesEmission.rateOverTimeMultiplier = x, 0f, 1f));
            
            turnOffSequence.JoinCallback(() => { isSwitching = false; isOn = !isOn; });

            turnOffSequence.Play();
        };
    }

    public void Interact()
    {
        Switch();
    }
}
