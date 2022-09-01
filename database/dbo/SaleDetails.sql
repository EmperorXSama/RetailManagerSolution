create table SaleDetails
(
    Id             int identity,
    SaleId         int           not null,
    ProductId      int           not null,
    PurchasedPrice money         not null,
    Quantity       int default 1 not null,
    Tax            int default 0 not null,
    column_7       int
)
go

