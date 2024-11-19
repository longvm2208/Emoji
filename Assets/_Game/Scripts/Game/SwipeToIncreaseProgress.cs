using UnityEngine;
using UnityEngine.Events;

public class SwipeToIncreaseProgress : MonoBehaviour
{
    [SerializeField] float progressIncreaseRate = 0.1f;
    [SerializeField] Collider2D swipeOn;
    [SerializeField] UnityEvent onEnter;
    [SerializeField] UnityEvent onExit;
    [SerializeField] UnityEvent<float> onProgress;
    [SerializeField] UnityEvent onComplete;

    bool isSwiping;
    bool isPlayingSound;
    float progress;
    Vector3 lastPos;

    void Start()
    {
        lastPos = transform.position;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == swipeOn)
        {
            isSwiping = true;
            lastPos = transform.position;
            onEnter?.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == swipeOn)
        {
            isSwiping = false;
            onExit?.Invoke();
        }
    }

    void Update()
    {
        if (isSwiping)
        {
            float distane = Vector3.Distance(transform.position, lastPos);
            if (Mathf.Approximately(distane, 0) && isPlayingSound)
            {
                isPlayingSound = false;
                AudioManager.Instance.StopSound();
            }
            else if (distane > 0 && !isPlayingSound)
            {
                isPlayingSound = true;
                AudioManager.Instance.PlaySound(SoundId.cleaning, 100);
            }
            progress += distane * progressIncreaseRate;
            progress = Mathf.Clamp01(progress);
            onProgress?.Invoke(progress);
            if (Mathf.Approximately(progress, 1))
            {
                onComplete?.Invoke();
                isSwiping = false;
                isPlayingSound = false;
                AudioManager.Instance.StopSound();
            }
            lastPos = transform.position;
        }
    }
}
