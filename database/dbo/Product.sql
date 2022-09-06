create table Product
(
    Id              int identity
        constraint Product_pk
            primary key,
    ProductName     nvarchar(100)                  not null,
    Description     nvarchar(max)                  not null,
    CreateDate      datetime2 default getutcdate() not null,
    LastModified    datetime2 default getutcdate() not null,
    RetailPrice     money                          not null,
    QuantityInStock int       default 1            not null,
    IsTaxable       bit       default 1            not null
)
go

