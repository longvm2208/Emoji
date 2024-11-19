using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] SoundId soundId;
    [SerializeField] float duration = -1;

    public void Play()
    {
        if (duration < 0)
        {
            AudioManager.Instance.PlaySound(soundId);
        }
        else
        {
            AudioManager.Instance.PlaySound(soundId, duration);
        }
    }

    public void Stop()
    {
        AudioManager.Instance.StopSound();
    }
}
