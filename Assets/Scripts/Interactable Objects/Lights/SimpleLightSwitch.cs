using DG.Tweening;
using UnityEngine;

public class SimpleLightSwitch : LightSwitchObject
{
    [SerializeField] AudioSource clickSound;

    void Start()
    {
        switchOnAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Pause();

            sequence.AppendCallback(() => clickSound.Play());
            sequence.AppendCallback(() => TrySwitchLight());
            
            sequence.Play();
        };

        switchOffAction = switchOnAction;

        SetUp();
    }
}
