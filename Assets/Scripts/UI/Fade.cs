using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] private Image _square;

    [Space(16)]
    [SerializeField] private Color _fadeColor;

    private Color _transperentColor;

    public static Fade Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _transperentColor = new Color(0f, 0f, 0f, 0f);
    }

    public void DoFade(Action action, float duration = 0.5f)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.SetEase(Ease.InOutSine);
        sequence.Pause();
        
        sequence.Append(_square.DOColor(_fadeColor, duration * 0.33f));
        sequence.AppendCallback(() => action.Invoke());
        sequence.AppendInterval(duration * 0.33f);
        sequence.Append(_square.DOColor(_transperentColor, duration * 0.33f));
        
        sequence.Play();
    }
}
