create table [User]
(
    Id           nvarchar(128)                  not null
        constraint User_pk
            primary key,
    FirstName    nvarchar(50)                   not null,
    LastName     nvarchar(50)                   not null,
    EmailAddress nvarchar(256)                  not null,
    CreatedDate  datetime2 default getutcdate() not null
)
go

create unique index User_Id_uindex
    on [User] (Id)
go

