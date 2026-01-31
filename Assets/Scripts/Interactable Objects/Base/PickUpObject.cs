using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    [SerializeField] AudioSource pickUpSound;

    [SerializeField] private bool hasSpot;

    private bool pickedUp;

    protected ItemSpot spot;

    public void PickUp(Action onPickupAction, List<GameObject> model)
    {
        if (pickedUp)
        {
            return;
        }

        Sequence sequence = DOTween.Sequence();
        sequence.Pause();

        sequence.AppendCallback(() =>
        {
            pickedUp = true;
            pickUpSound.Play();
            foreach (GameObject mesh in model)
            {
                mesh.SetActive(false);
            }
            onPickupAction.Invoke();
        });

        sequence.AppendInterval(pickUpSound.clip.length);
        sequence.AppendCallback(() => { Destroy(gameObject); if (hasSpot) { spot.IsFree = true; } });

        sequence.Play();
    }

    public static List<GameObject> GetModel(GameObject model)
    {
        List<GameObject> result = new List<GameObject>();

        for (int i = 0; i < model.transform.childCount; i++)
        {
            result.Add(model.transform.GetChild(i).gameObject);
        }

        return result;
    }

    protected void SetUpSpot()
    {
        spot = GetComponentInParent<ItemSpot>();
    }
}
