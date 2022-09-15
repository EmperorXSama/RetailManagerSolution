create procedure spInventory_Insert
    @ProductId int,
    @Quantity  int,
    @PurchasedPrice money,
    @PurchasedDate datetime2
as 
    begin 
        set nocount on;
        insert into dbo.Inventory (ProductId , Quantity,PurchasedPrice ,PurchasedDate) 
        values (@ProductId , @Quantity,@PurchasedPrice ,@PurchasedDate)
    end
go

