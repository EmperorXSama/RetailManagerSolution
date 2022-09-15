CREATE procedure spUser_Insert
        @Id varchar(128),
        @FirstName nvarchar(50),
        @LastName nvarchar(50),
        @EmailAddress nvarchar(256)
     as
begin
    set nocount on;
    insert into dbo.[User] (Id, FirstName, LastName, EmailAddress)
    values
        (@Id, @FirstName, @LastName, @EmailAddress)

end
go

