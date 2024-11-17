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

![image](https://github.com/user-attachments/assets/a0a1b5ec-06bf-47d5-9d9b-11f9e66c1bd7)

--------------------------------------
2. Seguido de esto en el json ingrese la conexión a su base de datos y migre esta.
3. En el loginController y program cambie a los de su local host
issuer: "https://localhost:7199",
audience: "https://localhost:7199"
----------------------------------------
INICIACIÓN API

1. Realice inicialmente el registro (SignUp)
2. Realice el login con las credenciales que usted ingreso en el registro, y con el token que reciba ingreselo en el authorize.
----------------------
3. Cree las siguientes 3 Categorias:
CategoryID | Category
1	Hombres
2	Mujeres
3	Niños
![image](https://github.com/user-attachments/assets/a2ad8e08-a6bc-4cac-80e7-b4512cc703f1)


---------------------
Cree los siguientes 12 productos 
ProductId| ProductName | ProductDescription | Price | CategoryId
1	UCL Camiseta hombre LOCAL |	Primera equipación 24/25 FC Barcelona | 100 | 1
2	UCL Camiseta hombre AWAY |	Segunda equipación 24/25 FC Barcelona |	100 | 1
3	Sudadera retro FC barcelona | Sudadera negra restro para hombre | 90 | 1
4	Chaqueta ligera FC barcelona | Chaqueta negra street para hombre | 150	| 1
5	UCL camiseta mujer LOCAL | Primera equipación 24/25 FC Barcelona | 100 | 2
6	UCL camiseta mujer AWAY	| Segunda equipación 24/25 FC Barcelona | 100 |	2
7	Chaqueta ligera Mujer |	Chaqueta ligera beige Barça – Mujer | 120 | 2
8	Camisa de viaje Mujer | Camiseta de viaje Barça Nike - Mujer | 80 | 2
9	Conjunto LOCAL FC Barcelona | Niño/a pequeño/a | 70	| 3
10	Conjunto AWAY FC Barcelona | Niño/a pequeño/a | 70 | 3
11	Sudadera negra Barça Nike | Junior sudadera negra | 90 | 3
12	Sudadera Barça con cremallera | Junior sudadera gris | 70 | 3

![image](https://github.com/user-attachments/assets/4c7a628c-3ba8-497a-8387-37cdf6ba2dd1)


--------------------------------
SIGUIENTE PASO ----------> https://github.com/JuanFernandoo/fcbarcelonafront

