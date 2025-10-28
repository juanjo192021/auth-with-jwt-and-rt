--Scaffold-DbContext "Server=Juanjo\SQLEXPRESS;Database=DB_AUTH_WITH_JWT_RF;Integrated Security=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context AuthWithJwtAndRefreshTokenDbContext -Force -Project Auth.Infrastructure -StartupProject Auth.API
--Scaffold-DbContext "Server=Juanjo\SQLEXPRESS;Database=DB_AUTH_WITH_JWT_RF;Integrated Security=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -ContextDir Persistence -OutputDir ..\Auth.Domain\Entities -Context AuthWithJwtAndRefreshTokenDbContext -Force -Project Auth.Infrastructure -StartupProject Auth.API
--Scaffold-DbContext "Server=DESKTOP-5KD39FJ\SQLEXPRESS;Database=DB_AUTH_WITH_JWT_RF;Integrated Security=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -ContextDir Persistence -OutputDir ..\Auth.Domain\Entities -Context AuthWithJwtAndRefreshTokenDbContext -Force -Project Auth.Infrastructure -StartupProject Auth.API

--CREATE DATABASE DB_AUTH_WITH_JWT_RF

--USE DB_AUTH_WITH_JWT_RF

GO

-- =============================================
-- TABLE: UserType
-- =============================================
CREATE TABLE UserType (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(50) NOT NULL UNIQUE, -- Admin, Customer, Employee, etc.
    Description VARCHAR(500),
    IsActive BIT NOT NULL DEFAULT 1
);

-- =============================================
-- TABLE: [User]
-- =============================================
CREATE TABLE [User] (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Email VARCHAR(100) NOT NULL UNIQUE,
    [Password] VARCHAR(200) NOT NULL,
    FirstName VARCHAR(60) NOT NULL,
    LastName VARCHAR(60) NOT NULL,
    ImageUrl VARCHAR(200) NULL,
    DocumentType VARCHAR(20) NOT NULL,
    DocumentNumber VARCHAR(20) NOT NULL UNIQUE,
    BirthDate DATE NOT NULL,
    Phone VARCHAR(20) NULL,
    Mobile VARCHAR(20) NOT NULL,
    Gender VARCHAR(20) NULL,
    Address VARCHAR(200) NOT NULL,
    IsConfirmed BIT NOT NULL DEFAULT 1,
    IsActive BIT NOT NULL DEFAULT 1,
    RegistrationDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    UserTypeId INT NOT NULL,
    CONSTRAINT FK_User_UserType FOREIGN KEY (UserTypeId) REFERENCES UserType(Id)
);

-- =============================================
-- TABLE: Role
-- =============================================
CREATE TABLE Role (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(50) NOT NULL UNIQUE,
    Description VARCHAR(200),
    IsActive BIT NOT NULL DEFAULT 1
);

-- =============================================
-- TABLE: UserRole (Relación muchos a muchos)
-- =============================================
CREATE TABLE UserRole (
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    --AssignedAt DATETIME NOT NULL DEFAULT GETDATE(),
    --AssignedBy INT NULL, -- Usuario que asignó el rol
    IsActive BIT NOT NULL DEFAULT 1,
    --RevokedAt DATETIME NULL, -- Fecha en la que se quitó el rol (si aplica)
    --RevokedBy INT NULL,      -- Usuario que quitó el rol
    --Notes VARCHAR(200) NULL, -- Motivo o descripción del cambio
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES [User](Id),
    FOREIGN KEY (RoleId) REFERENCES Role(Id),
    -- FOREIGN KEY (AssignedBy) REFERENCES [User](Id),
    -- FOREIGN KEY (RevokedBy) REFERENCES [User](Id)
);

-- =============================================
-- TABLE: RefreshTokenHistory
-- =============================================
CREATE TABLE RefreshTokenHistory (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    Token VARCHAR(500) NOT NULL,
    RefreshToken VARCHAR(200) NOT NULL,
    CreationDate DATETIME NOT NULL DEFAULT GETDATE(),
    ExpirationDate DATETIME NOT NULL,
    IsActive AS (IIF(ExpirationDate < GETDATE(), CONVERT(BIT,0), CONVERT(BIT,1))),
    FOREIGN KEY (UserId) REFERENCES [User](Id)
);

INSERT INTO UserType (Name, Description, IsActive) VALUES
('Admin', 'Administrador del sistema con acceso completo.', 1),
('Employee', 'Empleado interno de la organización.', 1),
('Customer', 'Cliente que utiliza los servicios o productos.', 1);

INSERT INTO Role (Name, Description, IsActive) VALUES
('ManageUsers', 'Puede crear, editar y eliminar usuarios.', 1),
('ManageProducts', 'Puede crear, editar y eliminar productos.', 1),
('ViewReports', 'Puede ver reportes y estadísticas.', 1),
('ProcessOrders', 'Puede procesar y despachar pedidos.', 1),
('AccessDashboard', 'Puede acceder al panel de administración.', 1),
('MakePurchases', 'Puede realizar compras en la tienda.', 1);
