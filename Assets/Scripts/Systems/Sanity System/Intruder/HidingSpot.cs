using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    [SerializeField] SwitchObject _switchObject;

    [SerializeField] Movement _playerMovementScript;

    private bool _playerIsInside;

    public bool HidesPlayer()
    {
        return _playerIsInside && _playerMovementScript.GetSpeedRatio() == 0 && !_switchObject.isOn;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _playerIsInside = true;
        }
    }

    private void OTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _playerIsInside = false;
        }
    }
}
