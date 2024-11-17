# BarcelonaAPI

1. Cree la base de datos en SQL server
CREATE DATABASE FcBarcelonaDB

use FcBarcelonaDB

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(512) NOT NULL,  
);

CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(50) NOT NULL UNIQUE
);

-- Tabla para los productos/inventario
CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(255),
    Price DECIMAL NOT NULL,
    CategoryId INT NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);

-- Tabla para los artículos del carrito de cada usuario
CREATE TABLE CartItems (
    CartItemId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0), -- Quantity siempre debe ser positivo
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

-- Tabla para las órdenes o pedidos realizados por los usuarios
CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    TotalAmount DECIMAL(10, 2) NOT NULL, 
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE PaymentAddresses (
    PaymentId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    CardNumber NVARCHAR(50) NOT NULL,         
    CardHolderName NVARCHAR(100) NOT NULL,
    ExpiryDate CHAR(5) NOT NULL,            
    Address NVARCHAR(100) NOT NULL,
    City NVARCHAR(50) NOT NULL,
    State NVARCHAR(50),
    ZipCode NVARCHAR(10) NOT NULL,
    Country NVARCHAR(50) NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);
--------------------------------------
2. Seguido de esto en el json ingrese la conexión a su base de datos y migre esta.
3. En el loginController y program cambie a los de su local host
issuer: "https://localhost:7199",
audience: "https://localhost:7199"
----------------------------------------
INICIACIÓN API

1. Realice inicialmente el registro (SignUp)
2. Realice el login con las credenciales que usted ingreso en el registro, y con el token que reciba ingreselo en el authorize.
3. Cree categorias y lo que desee para probar la aplicaión web.
