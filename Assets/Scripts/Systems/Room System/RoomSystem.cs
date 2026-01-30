using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomSystem : MonoBehaviour
{
    public enum roomsEnum
    {
        MainSpace,
        Bedroom,
        LivingRoom,
        Bathroom
    };

    [HideInInspector] public Dictionary<roomsEnum, bool> rooms = new Dictionary<roomsEnum, bool>();
    [HideInInspector] public roomsEnum room;

    [HideInInspector] public Dictionary<roomsEnum, int> lights = new Dictionary<roomsEnum, int>();
    [HideInInspector] public bool isInLight;

    private void Start()
    {
        foreach (roomsEnum roomName in Enum.GetValues(typeof(roomsEnum)))
        {
            rooms.Add(roomName, false);
            lights.Add(roomName, 0);
        }
    }

    private void Update()
    {
        DebugUI.instance.room = room;
        DebugUI.instance.isInLight = isInLight;
    }

    public void SwitchRoom(roomsEnum roomName, bool haveEntered)
    {
        if (haveEntered)
        {
            if (roomName != room)
            {
                // Debug.Log("Entered: " + roomName);
            }
            room = roomName;
            isInLight = lights[room] > 0;
        }
        else
        {
            if (roomName != room)
            {
                // Debug.Log("Exited: " + roomName);
            }
        }
        
        rooms[roomName] = haveEntered;
    }

    public void SwitchLight(roomsEnum roomName, bool state)
    {
        lights[roomName] += state ? 1 : -1;
        if (roomName == room)
        {
            isInLight = lights[room] > 0;
        }
    }
}
