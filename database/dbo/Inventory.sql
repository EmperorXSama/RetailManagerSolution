create table Inventory
(
    Id             int identity
        constraint Inventory_pk
            primary key,
    ProductId      int                            not null,
    Quantity       int       default 1            not null,
    PurchasedPrice money                          not null,
    PurchasedDate  datetime2 default getutcdate() not null
)
go

