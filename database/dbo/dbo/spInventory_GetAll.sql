create procedure spInventory_GetAll

as 
    begin 
        set nocount on;
        select Id, ProductId, Quantity, PurchasedPrice, PurchasedDate 
        from dbo.Inventory
    end
go

