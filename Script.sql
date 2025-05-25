--Scaffold-DbContext "Server=DESKTOP-5KD39FJ\SQLEXPRESS;Database=DB_AUTH_WITH_JWT_RF;Integrated Security=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context AuthWithJwtAndRefreshTokenDbContext -Force -Project Auth.Infrastructure -StartupProject Auth.API
--Scaffold-DbContext "Server=DESKTOP-5KD39FJ\SQLEXPRESS;Database=DB_AUTH_WITH_JWT_RF;Integrated Security=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -ContextDir Persistence -OutputDir ..\Auth.Domain\Entities -Context AuthWithJwtAndRefreshTokenDbContext -Force -Project Auth.Infrastructure -StartupProject Auth.API

--CREATE DATABASE DB_AUTH_WITH_JWT_RF

--USE DB_AUTH_WITH_JWT_RF

GO

CREATE TABLE [User](
Id int primary key identity,
Username varchar(20),
Password varchar(20)
)

insert into [User](Username,Password) 
values
('Admin','123')

select * from [User]

CREATE TABLE [RefreshTokenHistory](
Id int primary key identity,
UserId int references [User](Id),
Token varchar(500),
RefreshToken varchar(200),
CreationDate datetime,
ExpirationDate datetime,
Status AS ( iif(ExpirationDate < getdate(), convert(bit,0),convert(bit,1)))--columna calculada
)

select * from [RefreshTokenHistory]