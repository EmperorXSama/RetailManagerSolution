CREATE procedure spProduct_GetById
        @Id int
     as
begin
    set nocount on;
    select Id, ProductName, [Description], RetailPrice, QuantityInStock ,IsTaxable from
        dbo.Product  where Id = @Id

end
go

