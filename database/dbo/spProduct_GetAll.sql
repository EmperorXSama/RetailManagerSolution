CREATE procedure spProduct_GetAll

as
    begin
        set nocount on;
        select Id, ProductName, [Description], RetailPrice, QuantityInStock ,IsTaxable from
        dbo.Product  order by ProductName
    end
go

