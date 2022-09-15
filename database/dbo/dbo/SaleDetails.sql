create table SaleDetails
(
    Id             int identity
        constraint SaleDetails_pk
            primary key,
    SaleId         int           not null
        constraint SaleDetails_Sale_Id_fk
            references Sale,
    ProductId      int           not null
        constraint SaleDetails_Product_Id_fk
            references Product,
    PurchasedPrice money         not null,
    Quantity       int default 1 not null,
    Tax            money         not null
)
go

