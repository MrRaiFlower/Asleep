using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SantyEffectsController : MonoBehaviour
{
    [Header("Common Settings")]
    [SerializeField] private float _sanityTreshold;

    [Header("Camera Shake")]
    [SerializeField] private CinemachineBasicMultiChannelPerlin _noise;
    [SerializeField] private float _minAmplitude;
    [SerializeField] private float _maxAmplitude;
    [SerializeField] private float _minFrequency;
    [SerializeField] private float _maxFrequency;

    [Header("Heartbeat")]
    [SerializeField] private AudioSource _heartbeatSound;
    [SerializeField] private float _minBeatFrequency;
    [SerializeField] private float _maxBeatFrequency;
    [SerializeField] private float _minBeatVolume;
    [SerializeField] private float _maxBeatVolume;

    [Header("Post Processing")]
    [SerializeField] private Volume _volume;
    [SerializeField] private float _minVignetteIntensity;
    [SerializeField] private float _maxVignetteIntensity;
    [SerializeField] private float _minMotionBlurIntensity;
    [SerializeField] private float _maxMotionBlurIntensity;
    [SerializeField] private float _minSaturation;
    [SerializeField] private float _maxSaturation;

    [Header("Camera Control")]
    [SerializeField] private CameraControl _cameraControlScript;
    [SerializeField] private float _maxAcceleration;
    [SerializeField] private float _maxDrag;

    private bool _isShaking;
    private bool _canStart;
    private float _shakeFactor;

    private Vignette _vignette;
    private float _defaultVignetteIntensity;
    private MotionBlur _motionBlur;
    private float _defaultMotionBlurIntensity;
    private ColorAdjustments _colorAdjustments;
    private float _defaultSaturation;

    private float _defaultAcceleration;
    private float _defaultDrag;

    private void Start()
    {
        _noise.AmplitudeGain = 0f;
        _noise.FrequencyGain = 0f;

        _canStart = true;

        if (_volume.profile.TryGet<Vignette>(out Vignette vignette))
        {
            _vignette = vignette;
            _vignette.intensity.overrideState = true;
            _defaultVignetteIntensity = _vignette.intensity.value;
        }
        if (_volume.profile.TryGet<MotionBlur>(out MotionBlur motionBlur))
        {
            _motionBlur = motionBlur;
            _motionBlur.intensity.overrideState = true;
            _defaultMotionBlurIntensity = _motionBlur.intensity.value;
        }
        if (_volume.profile.TryGet<ColorAdjustments>(out ColorAdjustments colorAdjustments))
        {
            _colorAdjustments = colorAdjustments;
            _colorAdjustments.saturation.overrideState = true;
            _defaultSaturation = _colorAdjustments.saturation.value;
        }

        _defaultAcceleration = _cameraControlScript.acceleration;
        _defaultDrag = _cameraControlScript.drag;
    }

    private void Update()
    {
        if (_canStart && SanitySystem.Instance.sanity <= _sanityTreshold)
        {
            StartCoroutine(Shake());
        }
    }

    private void UpdatePostProcessing()
    {
        if (_isShaking)
        {
            _vignette.intensity.value = Mathf.Lerp(_minVignetteIntensity, _maxVignetteIntensity, _shakeFactor);
            _motionBlur.intensity.value = Mathf.Lerp(_minMotionBlurIntensity, _maxMotionBlurIntensity, _shakeFactor);
            _colorAdjustments.saturation.value = Mathf.Lerp(_minSaturation, _maxSaturation, _shakeFactor);
        }
        else
        {
            _vignette.intensity.value = _defaultVignetteIntensity;
            _motionBlur.intensity.value = _defaultMotionBlurIntensity;
            _colorAdjustments.saturation.value = _defaultSaturation;
        }
    }

    private void UpdateCameraControls()
    {
        if (_isShaking)
        {
            _cameraControlScript.acceleration = Mathf.Lerp(_defaultAcceleration, _maxAcceleration, _shakeFactor);
            _cameraControlScript.drag = Mathf.Lerp(_defaultDrag, _maxDrag, _shakeFactor);
        }
        else
        {
            _cameraControlScript.acceleration = _defaultAcceleration;
            _cameraControlScript.drag = _defaultDrag;
        }
    }

    private IEnumerator Shake()
    {
        _isShaking = true;
        _canStart = false;
        StartCoroutine(Hearbeat());

        while (true)
        {
            _shakeFactor = 1f - (SanitySystem.Instance.sanity / _sanityTreshold);
            UpdatePostProcessing();
            UpdateCameraControls();

            _noise.AmplitudeGain = Mathf.Lerp(_minAmplitude, _maxAmplitude, _shakeFactor);
            _noise.FrequencyGain = Mathf.Lerp(_minFrequency, _maxFrequency, _shakeFactor);

            if (SanitySystem.Instance.sanity > _sanityTreshold)
            {
                _noise.AmplitudeGain = 0f;
                _noise.FrequencyGain = 0f;
                _isShaking = false;
                UpdatePostProcessing();
                UpdateCameraControls();
                yield return new WaitForSeconds(10f);
                _canStart = true;
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator Hearbeat()
    {
        while (true)
        {
            _heartbeatSound.volume = Mathf.Lerp(_minBeatVolume, _maxBeatVolume, _shakeFactor);
            _heartbeatSound.Play();

            if (!_isShaking)
            {
                yield break;
            }

            yield return new WaitForSeconds(Mathf.Lerp(_minBeatFrequency, _maxBeatFrequency, _shakeFactor));
        }
    }
}
