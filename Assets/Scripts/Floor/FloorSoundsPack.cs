using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "FloorSoundsPack", menuName = "Scriptable Objects/FloorSoundsPack")]
public class FloorSoundsPack : ScriptableObject
{
    [SerializeField] public AudioResource footsteps;
    [SerializeField] public AudioResource jump;
    [SerializeField] public AudioResource landing;
}
