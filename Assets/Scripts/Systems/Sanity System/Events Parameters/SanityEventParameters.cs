using UnityEngine;

[CreateAssetMenu(fileName = "SanityEventParameters", menuName = "Scriptable Objects/SanityEventParameters")]
public class SanityEventParameters : ScriptableObject
{
    public float startInterval;
    public float endInterval;
    public float normalDev;
    public float sanityDev;
}
