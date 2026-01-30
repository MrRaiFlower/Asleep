using DG.Tweening;
using UnityEngine;

public class FloorLampSwitch : LightSwitchObject
{
    [SerializeField] AudioSource clickSound;

    [SerializeField] float animationDuration;
    [SerializeField] float yOffset;

    private Vector3 defaultPosition;
    private Vector3 downPosition;

    void Start()
    {
        defaultPosition = gameObject.transform.localPosition;
        downPosition = gameObject.transform.localPosition - Vector3.up * yOffset;

        switchOnAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Pause();
            
            sequence.SetEase(Ease.InExpo);
            sequence.AppendCallback(() => clickSound.Play());
            sequence.Append(gameObject.transform.DOLocalMove(downPosition, animationDuration / 3f * 2f));
            sequence.AppendCallback(() => TrySwitchLight());
            sequence.SetEase(Ease.OutQuad);
            sequence.Append(gameObject.transform.DOLocalMove(defaultPosition, animationDuration / 3f));
            
            sequence.Play();
        };

        switchOffAction = switchOnAction;

        SetUp();
    }
}
