using UnityEngine;

public class ChangeAudioClip : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;

    private void OnValidate()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void Change()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
