create table Sale
(
    Id        int                            not null
        constraint Sale_pk
            primary key,
    CashierId nvarchar(128)                  not null
        constraint Sale_User_Id_fk
            references [User],
    SaleDate  datetime2 default getutcdate() not null,
    SubTital  money                          not null,
    Tax       money                          not null,
    Total     money                          not null
)
go

create unique index Sale_Id_uindex
    on Sale (Id)
go

