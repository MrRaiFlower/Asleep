using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour, InteractableObject
{
    [SerializeField] public GameObject handle;

    [SerializeField] public BoxCollider colliderComponent;

    [SerializeField] public AudioSource doorOpenSound;
    [SerializeField] public AudioSource doorCloseSound;

    [SerializeField] public float direction;
    [SerializeField] public bool hasAnimatedHandle;
    [SerializeField] public float openingSpeed;
    
    private bool isOpen;

    private bool isActing;

    private Vector3 closedRotation;
    private Vector3 openRotation;

    private Vector3 handleDefaultRotation;
    private Vector3 handlePressedRotation;

    private void Start()
    {
        closedRotation = transform.localRotation.eulerAngles;
        openRotation = closedRotation - Vector3.forward * 90f * direction;

        if (hasAnimatedHandle)
        {
            handleDefaultRotation = handle.transform.localRotation.eulerAngles;
            handlePressedRotation = handleDefaultRotation + Vector3.up * 45f;
        }
    }

    public void Interact()
    {
        if (isActing)
        {
            return;
        }

        Sequence tweenSequence = DOTween.Sequence();
        tweenSequence.SetEase(Ease.InOutSine);
        tweenSequence.Pause();

        tweenSequence.AppendCallback(() => {isActing = true; colliderComponent.enabled = false; doorOpenSound.Play();});

        Sequence thudSequence = DOTween.Sequence();
        thudSequence.SetEase(Ease.InOutSine);
        thudSequence.Pause();

        Sequence handleSequence = DOTween.Sequence();
        handleSequence.SetEase(Ease.InOutSine);
        handleSequence.Pause();

        if (isOpen)
        {
            if (hasAnimatedHandle)
            {
                thudSequence.AppendInterval(openingSpeed - 0.2f);
                thudSequence.AppendCallback(() => doorCloseSound.Play());
            }
            
            tweenSequence.Join(this.transform.DOLocalRotate(closedRotation, openingSpeed));
        }
        else
        {
            if (hasAnimatedHandle)
            {
                handleSequence.Append(handle.transform.DOLocalRotate(handlePressedRotation, openingSpeed / 2f));
                handleSequence.Append(handle.transform.DOLocalRotate(handleDefaultRotation, openingSpeed / 2f));
            }

            tweenSequence.Append(this.transform.DOLocalRotate(openRotation, openingSpeed));
        }

        tweenSequence.AppendCallback(() => {colliderComponent.enabled = true; isOpen = !isOpen; isActing = false;});

        tweenSequence.Play();
        if (isOpen)
        {
            thudSequence.Play();

        }
        else
        {
            handleSequence.Play();
        }
    }
}
