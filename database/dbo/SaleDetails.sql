create table SaleDetails
(
    Id             int identity,
    SaleId         int           not null
        constraint SaleDetails_Sale_Id_fk
            references Sale,
    ProductId      int           not null
        constraint SaleDetails_Product_Id_fk
            references Product,
    PurchasedPrice money         not null,
    Quantity       int default 1 not null,
    Tax            int default 0 not null,
    column_7       int
)
go

