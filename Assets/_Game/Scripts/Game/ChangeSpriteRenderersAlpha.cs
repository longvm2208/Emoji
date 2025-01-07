using UnityEngine;

public class ChangeSpriteRenderersAlpha : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] srs;

    public void Change(float alpha)
    {
        for (int i = 0; i < srs.Length; i++)
        {
            srs[i].ChangeAlpha(alpha);
        }
    }
}
