using RM.Library.Models;

namespace RM.Library.DataAccesss;

public interface IInventoryData
{
    List<InventoryModel> GetInventory();
    Task SaveInventoryRecord(InventoryModel inventoryItem);
}