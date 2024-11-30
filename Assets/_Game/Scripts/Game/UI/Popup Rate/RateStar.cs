using UnityEngine;
using UnityEngine.UI;

public class RateStar : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] PopupRate popupRate;
    [SerializeField] Image starImage;

    public void UpdateState(int index)
    {
        if (index >= this.index)
        {
            starImage.color = Color.white;
        }
        else
        {
            starImage.color = Color.black;
            starImage.ChangeAlpha(0.2f);
        }
    }

    public void OnClickStar()
    {
        popupRate.Rate(index);
    }
}
