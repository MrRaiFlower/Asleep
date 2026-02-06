using System.Collections.Generic;
using UnityEngine;

public class Sofa : SwitchObject
{
    [SerializeField] private List<GameObject> _sofaPillows = new List<GameObject>();
    [SerializeField] private List<GameObject> _floorPillows = new List<GameObject>();
    [Space(16)]
    [SerializeField] private AudioSource _pillowsSound;

    private void Start()
    {
        turnOnAction = () => { SwitchPillows(); EndSwitch(); };

        turnOffAction = turnOnAction;
    }

    private void SwitchPillows()
    {
        foreach (GameObject pillow in _sofaPillows)
        {
            pillow.SetActive(!pillow.activeSelf);
        }
        foreach (GameObject pillow in _floorPillows)
        {
            pillow.SetActive(!pillow.activeSelf);
        }
        _pillowsSound.Play();
    }
}
