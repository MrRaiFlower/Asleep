using DG.Tweening;
using UnityEngine;

public class CeilingLightSwitch : LightSwitchObject
{
    [SerializeField] private GameObject switchObject;

    [SerializeField] private AudioSource clickSound;

    void Start()
    {
        switchOnAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.SetEase(Ease.InOutQuad);
            sequence.Pause();
            
            sequence.AppendCallback(() => clickSound.Play());
            sequence.Join(switchObject.transform.DOLocalRotate(Vector3.up * 5f, 0.05f, RotateMode.LocalAxisAdd));
            sequence.AppendCallback(() => TrySwitchLight());
            
            sequence.Play();
        };

        switchOffAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.SetEase(Ease.InOutQuad);
            sequence.Pause();
            
            sequence.AppendCallback(() => clickSound.Play());
            sequence.Join(switchObject.transform.DOLocalRotate(Vector3.down * 5f, 0.05f, RotateMode.LocalAxisAdd));
            sequence.AppendCallback(() => TrySwitchLight());
            
            sequence.Play();
        }; 

        SetUp();
    }
}
