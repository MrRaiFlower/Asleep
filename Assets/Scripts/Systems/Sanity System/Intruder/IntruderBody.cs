using NaughtyAttributes;
using UnityEngine;

public class IntruderBody : MonoBehaviour
{
    [SerializeField] private Transform _raycastOrigin;
    [SerializeField] private Transform _playerCamera;
    [Space(16)]
    [SerializeField] private float _lookAngletreshold;

    public bool IsSeen()
    {
        Vector3 direction = _raycastOrigin.position - _playerCamera.position;
        RaycastHit hit;

        if (Physics.Raycast(_playerCamera.position, direction, out hit))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Intruder"))
            {
                return Vector3.Angle(direction, _playerCamera.forward) < _lookAngletreshold;
            }
        }

        return false;
    }
}
