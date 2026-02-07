using DG.Tweening;
using UnityEngine;

public class FurnitureDoor : SwitchObject, InteractableObject
{
    [SerializeField] private SwitchObject _customSwitchObject;
    [SerializeField] private GameObject _doorObject;
    [SerializeField] private Collider _doorCollider;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float _animationDuration;
    [SerializeField] private AudioSource _sound;

    void Start()
    {
        turnOnAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.SetEase(Ease.InOutSine);
            sequence.Pause();

            sequence.AppendCallback(() =>
            {
                _doorCollider.enabled = false;
                _sound.Play();
                if (_customSwitchObject != null)
                {
                    _customSwitchObject.Switch();
                }
            });
            sequence.Join(transform.DOLocalRotate(_rotation, _animationDuration, RotateMode.LocalAxisAdd));
            sequence.AppendCallback(() => _doorCollider.enabled = true);
            EndSwitchSequence(sequence);

            sequence.Play();
        };

        turnOffAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.SetEase(Ease.InOutSine);
            sequence.Pause();

            sequence.AppendCallback(() =>
            {
                _doorCollider.enabled = false;
                _sound.Play();
                if (_customSwitchObject != null)
                {
                    _customSwitchObject.Switch();
                }
            });
            sequence.Join(transform.DOLocalRotate(-_rotation, _animationDuration, RotateMode.LocalAxisAdd));
            sequence.AppendCallback(() => _doorCollider.enabled = true);
            EndSwitchSequence(sequence);

            sequence.Play();
        };
    }

    public void Interact()
    {
        Switch();
    }
}
