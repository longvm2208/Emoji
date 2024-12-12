using UnityEngine;

public class ChangeBackgroundSprite : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite sprite;

    public void Change()
    {
        spriteRenderer.sprite = sprite;
    }
}
