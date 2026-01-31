using DG.Tweening;
using UnityEngine;

public class EntranceDoor : DangerObject, InteractableObject
{
    [SerializeField] GameObject lockObject;
    [SerializeField] GameObject doorObject;

    private void Start()
    {
        Tr0to1 = () =>
        {
            isSwitching = true;

            Sequence sequence = DOTween.Sequence();
            sequence.Pause();

            sequence.Append(lockObject.transform.DOLocalRotate(Vector3.up * 45f, 0.2f, RotateMode.LocalAxisAdd));
            sequence.AppendCallback(() => isSwitching = false);

            sequence.Play();
        };

        Tr1to2 = () =>
        {
            isSwitching = true;

            Sequence sequence = DOTween.Sequence();
            sequence.Pause();
            
            sequence.Append(lockObject.transform.DOLocalRotate(Vector3.up * 45f, 0.2f, RotateMode.LocalAxisAdd));
            sequence.Append(doorObject.transform.DOLocalRotate(Vector3.forward * 10f, 0.2f, RotateMode.LocalAxisAdd));
            sequence.AppendCallback(() => isSwitching = false);
            
            sequence.Play();
        };

        Tr2to0 = () =>
        {
            isSwitching = true;

            Sequence sequence = DOTween.Sequence();
            sequence.Pause();
            
            sequence.Append(doorObject.transform.DOLocalRotate(Vector3.back * 10f, 0.1f, RotateMode.LocalAxisAdd));
            sequence.Append(lockObject.transform.DOLocalRotate(Vector3.down * 90f, 0.1f, RotateMode.LocalAxisAdd));
            sequence.AppendCallback(() => isSwitching = false);
            
            sequence.Play();
        };

        Tr1to0 = () =>
        {
            isSwitching = true;

            Sequence sequence = DOTween.Sequence();
            sequence.Pause();

            sequence.Append(lockObject.transform.DOLocalRotate(Vector3.down * 45f, 0.1f, RotateMode.LocalAxisAdd));
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
