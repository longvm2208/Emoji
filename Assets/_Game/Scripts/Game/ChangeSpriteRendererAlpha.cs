using UnityEngine;

public class ChangeSpriteRendererAlpha : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;

    private void OnValidate()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
    }

    public void Change(float alpha)
    {
        sr.ChangeAlpha(alpha);
    }
}
