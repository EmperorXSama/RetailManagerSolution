using Microsoft.Extensions.Configuration;
using System.Configuration;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace RMWPFUserInterface.Library.Helpers;

public class ConfigHelper : IConfigHelper
{
    // Todo : Move this from app sitting to the Api 
    
    public decimal GetTaxRate()
    {
        string? rateText = ConfigurationManager.AppSettings["taxRate"];

        bool isValidTaxRate = Decimal.TryParse(rateText, out decimal output );

        if (!isValidTaxRate)
        {
            throw new ConfigurationErrorsException("the tax rate is not set up properly ");
        }

        return output;
    }
}