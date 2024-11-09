using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ScreenshotToSprite : SingletonMonoBehaviour<ScreenshotToSprite>
{
    [SerializeField] GameObject flashGo;
    [SerializeField] UnityEvent onComplete;
    [SerializeField] UnityEvent[] onCompletes;

    int index;

    public Sprite ScreenshotSprite { get; private set; }

    public void TakeScreenshot()
    {
        StartCoroutine(CaptureScreenshot());
    }

    private IEnumerator CaptureScreenshot()
    {
        UIManager.Instance.Panel.EnableCanvas(false);
        yield return new WaitForEndOfFrame(); // Wait until the end of the frame
        // Capture the screen into a Texture2D
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();
        // Convert the Texture2D to a Sprite
        ScreenshotSprite = Sprite.Create(
            screenshot,
            new Rect(0, 0, screenshot.width, screenshot.height),
            new Vector2(0.5f, 0.5f) // Pivot point in the center
        );
        flashGo.SetActive(true);
        yield return null;
        yield return null;
        yield return null;
        flashGo.SetActive(false);
        UIManager.Instance.Panel.EnableCanvas(true);
        onCompletes[index++]?.Invoke();
    }
}
