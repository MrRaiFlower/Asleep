using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private RoomSystem roomDetectorScript;

    [SerializeField] private LayerMask playerLayerMask;

    [SerializeField] private RoomSystem.roomsEnum roomName;

    private void OnTriggerEnter(Collider other)
    {
        if (playerLayerMask == 1 << other.gameObject.layer && !roomDetectorScript.rooms[roomName])
        {
            roomDetectorScript.SwitchRoom(roomName, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerLayerMask == 1 << other.gameObject.layer)
        {
            roomDetectorScript.SwitchRoom(roomName, false);
        }
    }
}
