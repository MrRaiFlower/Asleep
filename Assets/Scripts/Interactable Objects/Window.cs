using DG.Tweening;
using UnityEngine;

public class Window : DangerObject, InteractableObject
{
    [SerializeField] GameObject frame;

    private void Start()
    {
        Tr0to1 = () =>
        {
            isSwitching = true;

            Sequence sequence = DOTween.Sequence();
            sequence.Pause();

            sequence.Append(gameObject.transform.DORotate(Vector3.down * 45f, 0.2f, RotateMode.LocalAxisAdd).SetEase(Ease.InExpo));
            sequence.AppendCallback(() => isSwitching = false);

            sequence.Play();
        };

        Tr1to2 = () =>
        {
            isSwitching = true;

            Sequence sequence = DOTween.Sequence();
            sequence.Pause();
            
            sequence.Append(gameObject.transform.DORotate(Vector3.down * 45f, 0.2f, RotateMode.LocalAxisAdd).SetEase(Ease.InExpo));
            sequence.Append(frame.transform.DORotate(Vector3.forward * 5f, 0.2f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear));
            sequence.AppendCallback(() => isSwitching = false);
            
            sequence.Play();
        };

        Tr2to0 = () =>
        {
            isSwitching = true;

            Sequence sequence = DOTween.Sequence();
            sequence.Pause();
            
            sequence.Append(frame.transform.DORotate(Vector3.back * 5f, 0.2f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear));
            sequence.Append(gameObject.transform.DORotate(Vector3.up * 90f, 0.4f, RotateMode.LocalAxisAdd).SetEase(Ease.InExpo));
            sequence.AppendCallback(() => {isSwitching = false;});
            
            sequence.Play();
        };

        Tr1to0 = () =>
        {
            isSwitching = true;

            Sequence sequence = DOTween.Sequence();
            sequence.Pause();

            sequence.Append(gameObject.transform.DORotate(Vector3.up * 45f, 0.2f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear));
            sequence.AppendCallback(() => isSwitching = false);

            sequence.Play();
        };

        SetUp();
    }

    public void Interact()
    {
        DecreaseStage();
    }
}
