using System;
using UnityEngine;

[Serializable]
public class MusicByIdDictionary : SerializedDictionary<MusicId, AudioClip> { }
[Serializable]
public class SoundByIdDictionary : SerializedDictionary<SoundId, AudioClip> { }

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private MusicByIdDictionary musicById;
    [SerializeField] private SoundByIdDictionary soundById;

    Coroutine soundCoroutine;

    public void Initialize()
    {
        ToggleAudioSource();
    }

    public void ToggleAudioSource()
    {
        musicSource.enabled = GameData.Instance.IsAudioEnabled;
        soundSource.enabled = GameData.Instance.IsAudioEnabled;
    }

    public void PlayMusic(MusicId id)
    {
        musicSource.clip = musicById[id];
        musicSource.Play();
    }

    public void PauseMusic()
    {
        musicSource.enabled = false;
    }

    public void ResumeMusic()
    {
        musicSource.enabled = true;
    }

    public void ChangeMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void PlaySound(SoundId id)
    {
        if (!soundById.ContainsKey(id) || soundById[id] == null) return;

        if (soundSource.loop)
        {
            soundSource.loop = false;
            soundSource.Stop();
        }

        soundSource.PlayOneShot(soundById[id]);
    }

    public void PlaySound(SoundId id, float duration)
    {
        soundSource.loop = true;
        soundSource.clip = soundById[id];
        soundSource.Play();

        soundCoroutine = ScheduleUtils.DelayTask(duration, () =>
        {
            if (soundSource.loop)
            {
                soundSource.loop = false;
                soundSource.Stop();
            }
        });
    }

    public void StopSound()
    {
        soundSource.loop = false;
        soundSource.Stop();
        ScheduleUtils.StopCoroutine(soundCoroutine);
    }
}

public enum MusicId
{
    [HideInInspector]
    None = -1,
    Home = 0,
    Game = 1,
}

public enum SoundId
{
    [HideInInspector]
    None = -1,
    pop_click = 0,
    toy_2024,
    cleaning,
    sword,
    tap_click,
    idea_shine,
    wind_blow,
    woosh,
    transition,
    evil_force,
    pool_ball,
    washing,
    liquid,
    painting_02,
    very,
    eating,
    science,
    monkey,
    take_a_photo,
}
