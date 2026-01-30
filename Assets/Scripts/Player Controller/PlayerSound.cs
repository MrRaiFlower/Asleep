using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioSource jumpSoundComponent;
    [SerializeField] private AudioSource landngSoundComponent;
    [SerializeField] private AudioSource footstepSoundComponent;

    public void PlayJumpSound()
    {
        jumpSoundComponent.Play();
    }

    public void PlayLandingSound()
    {
        landngSoundComponent.Play();
    }

    public void PlayFootstepSound(float volume)
    {
        footstepSoundComponent.volume = volume;
        footstepSoundComponent.Play();
    }

    public void ChangeFloor(FloorSoundsPack soundPack)
    {
        jumpSoundComponent.resource = soundPack.jump;
        landngSoundComponent.resource = soundPack.landing;
        footstepSoundComponent.resource = soundPack.footsteps;
    }
}
