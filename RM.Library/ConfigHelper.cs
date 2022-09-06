using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace RM.Library;

public  class ConfigHelper : IConfigHelper
{
    private readonly IConfiguration _config;

    public  ConfigHelper(IConfiguration config)
    {
        _config = config;
    }
    public decimal GetTaxRate()
    {
        string? rateText = _config.GetSection("taxRate").Value;

        bool isValidTaxRate = Decimal.TryParse(rateText, out decimal output );

        if (!isValidTaxRate)
        {
            throw new ConfigurationErrorsException("the tax rate is not set up properly ");
        }

        return output/100;
    }
}