CREATE procedure spSaleDetail_Insert
    @SaleId int ,
    @ProductId int,
    @Quantity int,
    @PurchasedPrice money ,
    @Tax money
    as
        begin
            set nocount on;
            insert into dbo.SaleDetails (SaleId, ProductId, PurchasedPrice, Quantity, Tax)
            values
                (@SaleId, @ProductId, @PurchasedPrice,@Quantity, @Tax)
        end
go

