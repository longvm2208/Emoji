using Firebase.Analytics;
using System.Collections.Generic;

public struct ImpressionData
{
    public string adPlatform;
    public string adFormat;
    public string countryCode;
    public string adUnitIdentifier;
    public string networkName;
    public string placement;
    public double revenue;

    public ImpressionData(
        string adPlatform, string adFormat, string countryCode, string adUnitIdentifier,
        string networkName, string placement, double revenue)
    {
        this.adPlatform = adPlatform;
        this.adFormat = adFormat;
        this.countryCode = countryCode;
        this.adUnitIdentifier = adUnitIdentifier;
        this.networkName = networkName;
        this.placement = placement;
        this.revenue = revenue;
    }

    public Dictionary<string, string> ToDictionary()
    {
        Dictionary<string, string> dictionary = new()
        {
            { "ad_platform", adPlatform },
            { "ad_source", networkName },
            { "ad_unit_name", adUnitIdentifier },
            { "ad_format", adFormat },
            { "placement", placement },
            { "value", revenue.ToString() },
            { "currency", "USD" }
        };

        UnityEngine.Debug.Log(dictionary.ToString());

        return dictionary;
    }

    public Parameter[] ToParameters()
    {
        Parameter[] parameters =
        {
            new Parameter("ad_platform", adPlatform),
            new Parameter("ad_format", adFormat),
            new Parameter("country_code", countryCode),
            new Parameter("ad_unit_name", adUnitIdentifier),
            new Parameter("ad_source", networkName),
            new Parameter("placement", placement),
            new Parameter("value", revenue),
            new Parameter("currency", "USD"),
        };

        return parameters;
    }
}

