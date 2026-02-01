using DG.Tweening;
using UnityEngine;

public class Drawer : SwitchObject, InteractableObject
{
    [SerializeField] private AudioSource _sound;
    [SerializeField] private Vector3 _openDirection;
    [SerializeField] private float _animationDuration = 0.3f;

    private Vector3 _defaultPosition;
    private Vector3 _openPosition;

    void Start()
    {
        _defaultPosition = transform.localPosition;
        _openPosition = _defaultPosition + _openDirection;

        turnOnAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.SetEase(Ease.InOutSine);
            sequence.Pause();
            
            sequence.AppendCallback(() => _sound.Play());
            sequence.Join(transform.DOLocalMove(_openPosition, _animationDuration));
            EndSwitchSequence(sequence);
            
            sequence.Play();
        };

        turnOffAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.SetEase(Ease.InOutSine);
            sequence.Pause();
            
            sequence.AppendCallback(() => _sound.Play());
            sequence.Join(transform.DOLocalMove(_defaultPosition, _animationDuration));
            EndSwitchSequence(sequence);
            
            sequence.Play();
        };
    }

    public void Interact()
    {
        Switch();
    }
}
