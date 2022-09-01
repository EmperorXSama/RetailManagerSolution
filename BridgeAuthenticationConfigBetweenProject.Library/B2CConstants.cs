namespace BridgeAuthenticationConfigBetweenProject.Library;

public class B2CConstants
{
    public const string AzureAdConfigSection = "AzureAdB2C";
    public const string ApiConfigSection = "DataApi";
    public static readonly List<string> Scopes = new List<string> { "data.view", "data.write" };
    public const string Bearer = nameof(Bearer);
    public const string ProductClientName = "DataClient";
}