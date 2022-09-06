namespace RM.Library.Models;

public class SalesModel
{
    public string UserModel { get; set; } = "";
    public List<SaleDetailsModel> SaleDetails { get; set; } = new List<SaleDetailsModel>();
}