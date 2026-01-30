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

    private void Start()
    {
        showerParticles.Stop();
        splahesParticles.Stop();
        showerSound.volume = 0.0f;

        handleDefaultRotation = handleObject.transform.localRotation.eulerAngles;
        handlePressedRotation = handleDefaultRotation - Vector3.right * 10f;

        turnOnAction = () =>
        {
            Sequence turnOnSequence = DOTween.Sequence();
            turnOnSequence.SetEase(Ease.InOutSine);

            turnOnSequence.Pause();
            turnOnSequence.Append(handleObject.transform.DOLocalRotate(handlePressedRotation, 0.3f));
            turnOnSequence.JoinCallback(() => showerSound.Play());
            turnOnSequence.Join(DOTween.To(() => showerSound.volume, x => showerSound.volume = x, 0.2f, 0.2f));
            turnOnSequence.JoinCallback(() => {showerParticles.Play(); splahesParticles.Play(); isSwitching = false; isOn = !isOn;});

            turnOnSequence.Play();
        };

        turnOffAction = () =>
        {
            Sequence turnOffSequence = DOTween.Sequence();
            turnOffSequence.SetEase(Ease.InOutSine);

            turnOffSequence.Pause();
            turnOffSequence.Append(handleObject.transform.DOLocalRotate(handleDefaultRotation, 0.3f));
            turnOffSequence.Join(DOTween.To(() => showerSound.volume, x => showerSound.volume = x, 0f, 1f));
            turnOffSequence.AppendCallback(() => showerSound.Stop());
            turnOffSequence.JoinCallback(() => {showerParticles.Stop(); splahesParticles.Stop(); isSwitching = false; isOn = !isOn;});

            turnOffSequence.Play();
        };
    }

    public void Interact()
    {
        Switch();
    }
}
