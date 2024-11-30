using UnityEngine;

public class PopupRate : PopupBase
{
    [SerializeField] GameObject rateUs;
    [SerializeField] GameObject submit;
    [SerializeField] private RateStar[] rateStars;

    private GameData gameData => DataManager.Instance.GameData;

    public override void Open(object obj = null)
    {
        base.Open(obj);

        gameData.rateStar = 0;
    }

    public void Rate(int index)
    {
        gameData.rateStar = index;

        for (int i = 0; i < rateStars.Length; i++)
        {
            rateStars[i].UpdateState(index);
        }

        //if (rateUs.activeInHierarchy)
        //{
        //    rateUs.SetActive(false);
        //    submit.SetActive(true);
        //}
    }

    #region UI Events
    public void OnClickClose()
    {
        gameData.rateStar = 0;
        Close();
    }

    public void OnClickRate()
    {
        if (gameData.rateStar < 3)
        {
            Close();
            return;
        }
        
        if (true)
        {
            //Debug.Log("Rate game in app");
            GameManager.Instance.RateGameInApp();
        }
        else
        {
            //Debug.Log("Rate game in store");
#if UNITY_ANDROID
            Application.OpenURL("http://play.google.com/store/apps/details?id=" + Application.identifier);
#elif UNITY_IOS
            Application.OpenURL("");
#endif
        }

        Close();
    }
    #endregion
}
