using DG.Tweening;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    [SerializeField] bool playAnimation = true;
    [SerializeField] bool playSound = true;
    [SerializeField] bool vibrate = false;
    [SerializeField] RectTransform rt;
    [SerializeField, ExposedScriptableObject] ButtonClickConfig config;

    Tween tween;

    void OnValidate()
    {
        if (rt == null)
        {
            rt = transform as RectTransform;
        }
    }

    public void OnClick()
    {
        if (playAnimation)
        {
            if (tween == null)
            {
                tween = rt.DOScale(config.Scale, config.Duration).SetEase(config.Curve).SetAutoKill(false);
            }

            tween.Restart();
        }

        if (playSound)
        {
            AudioManager.Instance.PlaySound(SoundId.pop_click);
        }

        if (vibrate)
        {
            //VibrationManager.Instance.Vibrate();
        }
    }
}
