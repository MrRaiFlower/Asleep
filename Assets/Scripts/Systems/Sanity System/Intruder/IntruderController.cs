using System.Collections.Generic;
using UnityEngine;

public class IntruderController : MonoBehaviour
{
    public static IntruderController Instance;

    [SerializeField] private List<Intruder> _intruders = new List<Intruder>();

    [HideInInspector] public bool isActive;
    
    private void Awake()
    {
        Instance = this;
    }

    public void Intrude()
    {
        _intruders[UnityEngine.Random.Range(0, _intruders.Count)].Activate();
    }
}
