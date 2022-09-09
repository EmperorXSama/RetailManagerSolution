CREATE procedure spSale_Insert
@Id int output ,
@CashierId nvarchar(128),
@SaleDate datetime2(7),
@SubTotal money ,
@Tax money,
@Total money

as
    begin
        set nocount on;
        insert into dbo.Sale (CashierId, SaleDate, SubTotal, Tax, Total)
        values
        (@CashierId, @SaleDate, @SubTotal, @Tax, @Total)
        set @Id = SCOPE_IDENTITY() return @Id
    end
go

