using DG.Tweening;
using UnityEngine;

public class LightSwitch : MonoBehaviour, InteractableObject
{
    [SerializeField] private GameObject switchObject;

    [SerializeField] private CeilingLight[] ceilingLightScripts;

    [SerializeField] private AudioSource clickSound;

    private PowerSystem powerSystem;

    private bool isOn;

    private bool isSwitchingState;

    void Start()
    {
        powerSystem = GameObject.Find("Power System").GetComponent<PowerSystem>();
        
        PowerSystem.OnStateChange += () => Refresh();
    }

    public void Interact()
    {
        if (isSwitchingState)
        {
            return;
        }

        Sequence tweenSequence = DOTween.Sequence();
        tweenSequence.SetEase(Ease.InOutQuad);
        tweenSequence.Pause();

        tweenSequence.AppendCallback(() => clickSound.Play());

        if (isOn)
        {
            tweenSequence.Join(switchObject.transform.DOLocalRotate(Vector3.down * 5f, 0.05f, RotateMode.LocalAxisAdd));
        }
        else
        {
            tweenSequence.Join(switchObject.transform.DOLocalRotate(Vector3.up * 5f, 0.05f, RotateMode.LocalAxisAdd));
        }

        tweenSequence.AppendCallback(() => 
        {
            isOn = !isOn; 
            if (powerSystem.hasPower)
            {
                SwitchLights();
            }
        });

        tweenSequence.Play();
    }

    public void Refresh()
    {
        if (powerSystem.hasPower && isOn && !ceilingLightScripts[0].isOn)
        {
            SwitchLights();
        }
        else if (!powerSystem.hasPower && ceilingLightScripts[0].isOn)
        {
            SwitchLights();
        }
    }

    private void SwitchLights()
    {
        foreach (CeilingLight ceilingLightScript in ceilingLightScripts)
        {
            ceilingLightScript.Switch();
        }
    }
}
