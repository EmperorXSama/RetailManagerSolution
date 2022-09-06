using RMWPFUserInterface.Library.Models;

namespace RMWPFUserInterface.Library.Helpers;

public interface ISaleEndPoint
{
    Task PostSale(SalesModel sale, string token);
}