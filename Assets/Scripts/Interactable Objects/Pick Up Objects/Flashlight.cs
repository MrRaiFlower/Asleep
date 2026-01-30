using System;

public class Flashlight: PickUpObject, InteractableObject
{
    public static event Action OnPickup;

    public void Interact()
    {
        PickUp(() => OnPickup.Invoke(), GetModel(gameObject));
    }
}
