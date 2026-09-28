# Smart Inventory & Order Management System

## Project Description

Smart Inventory & Order Management System is a backend application developed using **ASP.NET Core Web API**.

The application allows customers to register, login, view products, add products to cart, place orders and view their order history.

Admin users can manage products and orders.

## Technologies Used

* C#
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* LINQ
* JWT Authentication
* Role-Based Authorization
* Swagger / OpenAPI

## Features

### Authentication

* User Registration
* User Login
* JWT Authentication
* Customer and Admin roles

### Product

* View Products
* Search Products
* Pagination
* Add Product
* Update Product
* Delete Product
* Stock Validation

### Cart

* Add Product to Cart
* View Cart
* Update Cart Quantity
* Remove Product from Cart

### Orders

* Place Order
* View My Orders
* View Order by ID
* Stock validation during order placement

### Admin

* View All Orders
* Update Order Status
* Manage Products

## API Endpoints

### Auth


POST /api/Auth/register
POST /api/Auth/login


### Product

GET    /api/Product
POST   /api/Product
PUT    /api/Product/{id}
DELETE /api/Product/{id}


### Cart


POST   /api/Cart/add
GET    /api/Cart
PUT    /api/Cart/update
DELETE /api/Cart/remove


### Order


GET  /api/Order
GET  /api/Order/{id}


### Admin

GET /api/Admin/orders
PUT /api/Admin/order-status/{id}


## Authentication

Protected APIs use JWT Bearer Authentication.

In Swagger:
text
Authorize
→ Bearer YOUR_JWT_TOKEN


Admin APIs can be accessed only with an Admin JWT token.

## Database

The project uses **SQL Server** with **Entity Framework Core**.

EF Core migrations are included in the project.

To update the database:

powershell
Update-Database


## Run the Project

### 1. Clone the Repository


git clone YOUR_GITHUB_REPOSITORY_URL


### 2. Open the Project

Open the project in Visual Studio.

### 3. Configure Database

Update the SQL Server connection string in:


appsettings.json

### 4. Update Database

powershell
Update-Database


### 5. Run the Application

bash
dotnet run


### 6. Open Swagger

text
https://localhost:7076/swagger


## Project Flow

text
Register
   ↓
Login
   ↓
JWT Token
   ↓
View Products
   ↓
Add to Cart
   ↓
Place Order
   ↓
Stock Updated
   ↓
View Order History


## Author

**Aswani Poleboina**

.NET Developer | C# | ASP.NET Core | Web API | SQL Server | Angular
