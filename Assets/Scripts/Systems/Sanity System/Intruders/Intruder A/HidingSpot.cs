using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    [SerializeField] private SwitchObject _switchObject;

    [SerializeField] private Movement _playerMovementScript;

    [SerializeField] public GameObject hidingBody;

    private bool _playerIsInside;

    private void Start()
    {
        hidingBody.SetActive(false);
    }

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
