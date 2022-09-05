using RMWPFUserInterface.Library.Models;

namespace RMWPFUserInterface.Library.Helpers;

public interface IApiHelper
{
    Task<ILoggedInUserModel> GetLoggedInUserInfo(string stringId, string token);
    HttpClient ApiClient { get; }
}