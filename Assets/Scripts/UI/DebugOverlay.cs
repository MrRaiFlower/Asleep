using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugOverlay : MonoBehaviour
{
    public static DebugOverlay Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private TMP_Text _textObject;

    private string _text;

    [HideInInspector] public float flashlightCharge;

    [HideInInspector] public float sanity;

    [HideInInspector] public float timeTillDisturb;
    [HideInInspector] public float timeTillDanger;
    [HideInInspector] public float timeTillIntruder;
    [HideInInspector] public float timeTillGameOver;

    private InputAction _debugAction;

    private void Start()
    {
        _debugAction = InputSystem.actions.FindAction("Debug");

        _textObject.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_debugAction.WasPressedThisFrame())
        {
            _textObject.gameObject.SetActive(!_textObject.gameObject.activeSelf);
        }
    }

    private void LateUpdate()
    {
        if (!_textObject.gameObject.activeSelf)
        {
            return;
        }

        _text = "<color=yellow>Debug</color>\n";
        _text += "\n";
        _text += String.Format("Flashlight charge: {0:F2}%\n", flashlightCharge).Replace(',', '.');
        _text += "\n";
        _text += String.Format("Sanity: {0:F2}%\n", sanity).Replace(',', '.');
        _text += "\n";
        _text += String.Format("Till next disturb: {0:F2} seconds\n", timeTillDisturb).Replace(',', '.');
        _text += String.Format("Till next danger: {0:F2} seconds\n", timeTillDanger).Replace(',', '.');
        _text += String.Format("Till next intruder: {0:F2} seconds\n", timeTillIntruder).Replace(',', '.');
        _text += String.Format("Till next game over: {0:F2} seconds\n", timeTillGameOver).Replace(',', '.');

        _textObject.text = _text;
    }
}
