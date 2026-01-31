using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemPile : PickUpObject, InteractableObject
{
    public static event Action OnPickUp;

    [SerializeField] GameObject[] models;
    [SerializeField] bool needsFlashlight;
    [SerializeField] bool needsUnfulSanity;

    private GameObject activeModel;

    public void Interact()
    {
        if (needsFlashlight && !GameObject.Find("Player").GetComponent<FlashlightControl>().isActive)
        {
            return;
        }
        if (needsUnfulSanity && SanitySystem.Instance.sanity >= SanitySystem.Instance.maxSanity - SanitySystem.Instance.minPillRegen)
        {
            return;
        }
        PickUp(() => OnPickUp.Invoke(), new List<GameObject>() { activeModel });
    }

    void Start()
    {
        foreach (GameObject model in models)
        {
            model.SetActive(false);
        }

        activeModel = models[UnityEngine.Random.Range(0, models.Length)];
        activeModel.SetActive(true);

        SetUpSpot();
    }
}
