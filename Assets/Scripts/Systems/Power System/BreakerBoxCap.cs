using DG.Tweening;
using UnityEngine;

public class BreakerBoxCap : MonoBehaviour, InteractableObject
{
    [SerializeField] BoxCollider[] switchesColliders;

    [SerializeField] AudioSource clickSound;

    private bool isOpen;

    private Vector3 closedRotation;
    private Vector3 openRotation;

    private bool isSwitchingState;

    void Start()
    {
        closedRotation = transform.localRotation.eulerAngles;
        openRotation = closedRotation - Vector3.right * 179.9f;

        SwitchColliders();
    }

    public void Interact()
    {
        if (isSwitchingState)
        {
            return;
        }

        Sequence tweenSequence = DOTween.Sequence();
        tweenSequence.SetEase(Ease.InOutSine);
        tweenSequence.Pause();

        tweenSequence.AppendCallback(() => isSwitchingState = true);
        if (isOpen)
        {
            tweenSequence.Join(transform.DOLocalRotate(closedRotation, 0.5f));
            tweenSequence.AppendCallback(() => SwitchColliders());
            tweenSequence.AppendCallback(() => clickSound.Play());
        }
        else
        {
            tweenSequence.JoinCallback(() => clickSound.Play());
            tweenSequence.JoinCallback(() => SwitchColliders());
            tweenSequence.Join(transform.DOLocalRotate(openRotation, 0.5f));
        }
        tweenSequence.AppendCallback(() => {isSwitchingState = false; isOpen = !isOpen;});

        tweenSequence.Play();
    }

    private void SwitchColliders()
    {
        foreach (BoxCollider collider in switchesColliders)
        {
            collider.enabled = !collider.enabled;
        }
    }
}
