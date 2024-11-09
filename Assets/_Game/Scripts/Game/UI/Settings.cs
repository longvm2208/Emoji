using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] GameObject audioOn;
    [SerializeField] GameObject audioOff;
    [SerializeField] GameObject vibrateOn;
    [SerializeField] GameObject vibrateOff;

    private void OnEnable()
    {
        ReloadAudio();
        ReloadVibration();
    }

    void ReloadAudio()
    {
        audioOn.SetActive(GameData.Instance.IsAudioEnabled);
        audioOff.SetActive(!GameData.Instance.IsAudioEnabled);
    }

    void ReloadVibration()
    {
        vibrateOn.SetActive(GameData.Instance.IsVibrationEnabled);
        vibrateOff.SetActive(!GameData.Instance.IsVibrationEnabled);
    }

    #region UI EVENTS
    public void OnClickToggleAudio()
    {
        GameData.Instance.IsAudioEnabled = !GameData.Instance.IsAudioEnabled;
        ReloadAudio();
        AudioManager.Instance.ToggleAudioSource();
    }

    public void OnClickToggleVibration()
    {
        GameData.Instance.IsVibrationEnabled = !GameData.Instance.IsVibrationEnabled;
        ReloadVibration();
        VibrationManager.Instance.ToggleVibration();
    }
    #endregion
}
