using DG.Tweening;
using UnityEngine;
using UnityEngine.Video;

public class TV : SwitchObject, InteractableObject
{
    [SerializeField] private GameObject _screen;
    [Space(16)]
    [SerializeField] private Light _light;
    [Space(16)]
    [SerializeField] private AudioSource _switchSound;
    [SerializeField] private AudioSource _staticSound;
    [Space(16)]
    [SerializeField] private float _switchDuration;

    private Material _screenMaterial;
    private VideoPlayer _screenVideoPlayer;

    private float _lightsIntensity;
    private float _staticVolume;

    private void Start()
    {
        _screenMaterial = _screen.GetComponent<MeshRenderer>().material;
        _screenVideoPlayer = _screen.GetComponent<VideoPlayer>();

        _screenVideoPlayer.Prepare();

        _lightsIntensity = _light.intensity;
        _light.intensity = 0f;

        _staticVolume = _staticSound.volume;
        _staticSound.volume = 0f;

        turnOnAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.SetEase(Ease.InOutSine);
            sequence.Pause();

            sequence.AppendCallback(_switchSound.Play);
            sequence.AppendCallback(_screenVideoPlayer.Play);
            sequence.Join(DOTween.To(() => _light.intensity, x => _light.intensity = x, _lightsIntensity, _switchDuration));
            sequence.Join(DOTween.To(() => _staticSound.volume, x => _staticSound.volume = x, _staticVolume, _switchDuration));
            sequence.Join(DOTween.To(() => _screenMaterial.GetFloat("_Tint"), x => _screenMaterial.SetFloat("_Tint", x), 0f, _switchDuration));
            EndSwitchSequence(sequence);

            sequence.Play();
        };

        turnOffAction = () =>
        {
            Sequence sequence = DOTween.Sequence();
            sequence.SetEase(Ease.InOutSine);
            sequence.Pause();

            sequence.AppendCallback(_switchSound.Play);
            sequence.Join(DOTween.To(() => _light.intensity, x => _light.intensity = x, 0f, _switchDuration));
            sequence.Join(DOTween.To(() => _staticSound.volume, x => _staticSound.volume = x, 0f, _switchDuration));
            sequence.Join(DOTween.To(() => _screenMaterial.GetFloat("_Tint"), x => _screenMaterial.SetFloat("_Tint", x), 1f, _switchDuration));
            sequence.AppendCallback(_screenVideoPlayer.Stop);
            EndSwitchSequence(sequence);

            sequence.Play();
        };
    }

    public void Interact()
    {
        Switch();
    }
}
