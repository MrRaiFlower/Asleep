using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    [SerializeField] AudioSource pickUpSound;

    private bool pickedUp;

    public void PickUp(Action onPickupAction, List<GameObject> model)
    {
        if (pickedUp)
        {
            return;
        }

        Sequence sequence = DOTween.Sequence();
        sequence.Pause();

        sequence.AppendCallback(() => {
            pickedUp = true; 
            pickUpSound.Play();
            foreach(GameObject mesh in model)
            {
                mesh.SetActive(false);
            }
        });
        
        sequence.AppendInterval(pickUpSound.clip.length);
        sequence.AppendCallback(() => {Destroy(gameObject); onPickupAction.Invoke();});

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
}
