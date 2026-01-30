using System;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPile : PickUpObject, InteractableObject
{
    public static event Action OnPickUp;

    [SerializeField] GameObject[] models;
    
    private GameObject activeModel;

    public void Interact()
    {
        if (!GameObject.Find("Player").GetComponent<FlashlightControl>().isActive)
        {
            return;
        }
        PickUp(() => OnPickUp.Invoke(), new List<GameObject>(){activeModel});
    }

    void Start()
    {
        foreach (GameObject model in models)
        {
            model.SetActive(false);
        }

        activeModel = models[UnityEngine.Random.Range(0, models.Length)];

        activeModel.SetActive(true);
    }
}
