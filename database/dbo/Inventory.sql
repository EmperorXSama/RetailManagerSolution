create table Inventory
(
    Id             int identity
        constraint Inventory_pk
            primary key,
    ProductId      int                            not null
        constraint Inventory_Product_Id_fk
            references Product,
    Quantity       int       default 1            not null,
    PurchasedPrice money                          not null,
    PurchasedDate  datetime2 default getutcdate() not null
)
go

