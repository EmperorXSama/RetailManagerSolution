namespace RMWPFUserInterface.Library.Models;

public class SalesModel
{
    public string UserModel { get; set; } = "";// hold the id of the logged in user
    public List<SaleDetailsModel> SaleDetails { get; set; } = new List<SaleDetailsModel>();
}