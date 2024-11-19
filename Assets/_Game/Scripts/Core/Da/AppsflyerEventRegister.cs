using System.Collections.Generic;
using UnityEngine;
using AppsFlyerSDK;

public class AppsflyerEventRegister : MonoBehaviour
{
    public static void af_purchase_abi(decimal af_revenue, string af_currency, int af_quantity, string af_content_id)
    {
        float fCost = (float)af_revenue;
        fCost *= 0.63f;
        AppsFlyer.sendEvent("af_Purchase_abi",
            new Dictionary<string, string>() {
                { "af_revenue", fCost.ToString(System.Globalization.NumberFormatInfo.InvariantInfo) },
                { "af_currency", af_currency },
                { "af_quantity", af_quantity.ToString() },
                { "af_content_id", af_content_id }
            });
    }

    #region GAMEPLAY
    public static void af_session_start(string count)
    {
        AppsFlyer.sendEvent("session_start_" + count, new Dictionary<string, string>());
    }

    public static void af_tutorial_completed(string tutId, bool isComplete)
    {
        AppsFlyer.sendEvent("af_tutorial_completion",
            new Dictionary<string, string>() {
                {"af_success", isComplete.ToString() },
                {"af_tutorial_id", tutId }
            });
    }

    public static void af_level_achieved(string level, int score)
    {
        AppsFlyer.sendEvent("af_level_achieved",
            new Dictionary<string, string>() {
                {"af_level", level },
                {"af_score", score.ToString() }
            });
    }

    public static void af_level_achieved(int chapter, int level)
    {
        AppsFlyer.sendEvent("af_level_achieved_" + chapter + "_" + level, new());
    }
    #endregion

    #region INTERSTITIAL AD
    /// <summary>
    /// Bắn lên khi ấn nút bất kỳ theo logic show inter của game
    /// <br>(bắn lên khi ấn nút show ads theo logic của game)</br>
    /// <br>Khi đủ capping time + đã load được ads</br>
    /// </summary>
    public static void af_interstitial_ad_eligible()
    {
        AppsFlyer.sendEvent("af_inters_ad_eligible", new Dictionary<string, string>());
    }

    /// <summary>
    /// Bắn lên khi check đã có sẵn ads lưu về máy thành công
    /// <br>(bắn lên khi ads available)</br>
    /// <br>Trong sự kiện của mediation manager</br>
    /// </summary>
    public static void af_interstitial_ad_api_called()
    {
        AppsFlyer.sendEvent("af_inters_api_called", new Dictionary<string, string>());
    }

    /// <summary>
    /// Bắn lên khi ad hiện lên màn hình cho user xem (open inter)
    /// <br>Trong sự kiện của mediation manager</br>
    /// </summary>
    public static void af_interstitial_ad_displayed()
    {
        AppsFlyer.sendEvent("af_inters_displayed", new Dictionary<string, string>());
    }
    #endregion

    #region REWARDED AD
    /// <summary>
    /// Bắn lên khi ấn nút bất kỳ theo logic show Reward của game
    /// <br>(bắn lên khi ấn nút show ads theo logic của game)</br>
    /// <br>Khi đủ capping time + đã load được ads</br>
    /// </summary>
    public static void af_rewarded_ad_eligible()
    {
        AppsFlyer.sendEvent("af_rewarded_ad_eligible", new Dictionary<string, string>());
    }

    /// <summary>
    /// Bắn lên khi check đã có sẵn ads lưu về máy thành công (bắn lên khi ads available)
    /// <br>Trong sự kiện của mediation manager</br>
    /// </summary>
    public static void af_rewarded_ad_api_called()
    {
        AppsFlyer.sendEvent("af_rewarded_api_called", new Dictionary<string, string>());
    }

    /// <summary>
    /// Bắn lên khi ad hiện lên màn hình cho user xem (open inter)
    /// <br>Trong sự kiện của mediation manager</br>
    /// </summary>
    public static void af_rewarded_ad_displayed()
    {
        AppsFlyer.sendEvent("af_rewarded_displayed", new Dictionary<string, string>());
    }

    /// <summary>
    /// Bắn lên khi User tắt ads và nhận được reward
    /// <br>Trong sự kiện của mediation manager</br>
    /// </summary>
    public static void af_rewarded_ad_completed()
    {
        AppsFlyer.sendEvent("af_rewarded_ad_completed", new Dictionary<string, string>());
    }
    #endregion
}
