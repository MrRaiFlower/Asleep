using UnityEngine;

public class SofaPillow : MonoBehaviour, InteractableObject
{
    private Sofa _sofa;

    private void Start()
    {
        _sofa = GetComponentInParent<Sofa>();
    }

    public void Interact()
    {
        _sofa.Switch();
    }
}
