# BarcelonaAPI

CREACIÓN BASE DE DATOS SQL SERVER

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
Luego de la creación de la base de datos clone el proyecto en C# 

1. VERIFICACIÓN NUGETS
- Ingrese a herramientas -> Administrador de paqueted NuGet -> Administrador de paquetes NuGet para la solución. Verifique que tenga instalado los siguientes paquetes
![image](https://github.com/user-attachments/assets/b42626f6-9ba7-4118-83a2-f2d5719e0439)

2. MIGRAR BASE DE DATOS
- Ingrese al JSON y conecte su base de datos
![image](https://github.com/user-attachments/assets/58e2bf40-5cb0-4783-81c8-bfbf81908c99)

- Ingrese a herramientas -> Administrador de paqueted NuGet -> Consola del administrador de paquetes
- Ingrese el siguiente comando para realizar la migracoión:
  Add-Migration InitialCreate
  Update-Database   

3. En el loginController y program cambie el 7199 a los de su local host (Esto los puede evidenciar en al inicializar la API en la url puede ver el localhost)
issuer: "https://localhost:7199",
audience: "https://localhost:7199"
----------------------------------------

INICIALIZACIÓN API

1. Realice inicialmente el registro (SignUp)
![image](https://github.com/user-attachments/assets/150eb0bc-d54f-4d3c-a186-a6a3420edbed)

2. Realice el login con las credenciales que usted ingreso en el registro, y con el token que reciba ingreselo en el authorize.
   
![image](https://github.com/user-attachments/assets/cab0c19c-c678-4041-8b5b-56dbe07aed02)
![image](https://github.com/user-attachments/assets/93ff2ebf-8ce0-4abe-93ea-b84ed11db829)

----------------------
3. Cree las siguientes 3 Categorias:
   
![image](https://github.com/user-attachments/assets/a2ad8e08-a6bc-4cac-80e7-b4512cc703f1)
![image](https://github.com/user-attachments/assets/26569055-3c86-4888-bd68-99a0787d31c1)

---------------------
Cree los siguientes 12 productos 

![image](https://github.com/user-attachments/assets/4c7a628c-3ba8-497a-8387-37cdf6ba2dd1)
![image](https://github.com/user-attachments/assets/d385e105-132a-42fc-a2ae-12f653088833)

--------------------------------
SIGUIENTE PASO ----------> https://github.com/JuanFernandoo/fcbarcelonafront

