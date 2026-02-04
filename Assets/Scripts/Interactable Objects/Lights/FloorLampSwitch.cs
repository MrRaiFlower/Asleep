using DG.Tweening;
using UnityEngine;

public class FloorLampSwitch : LightSwitchObject
{
    [SerializeField] GameObject switchObject;
    [Space(16)]
    [SerializeField] AudioSource clickSound;
    [Space(16)]
    [SerializeField] float animationDuration;
    [SerializeField] float yOffset;

    private Vector3 defaultPosition;
    private Vector3 downPosition;

    void Start()
    {
        defaultPosition = switchObject.transform.localPosition;
        downPosition = switchObject.transform.localPosition - Vector3.up * yOffset;

        switchOnAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Pause();
            
            sequence.SetEase(Ease.InExpo);
            sequence.AppendCallback(() => clickSound.Play());
            sequence.Append(switchObject.transform.DOLocalMove(downPosition, animationDuration / 3f * 2f));
            sequence.AppendCallback(() => TrySwitchLight());
            sequence.SetEase(Ease.OutQuad);
            sequence.Append(switchObject.transform.DOLocalMove(defaultPosition, animationDuration / 3f));
            
            sequence.Play();
        };

        switchOffAction = switchOnAction;

        SetUp();
    }
}
