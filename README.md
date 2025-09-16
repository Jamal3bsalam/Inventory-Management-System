# 📦 Inventory Management System

## 📝 Overview
The **Inventory Management System** is a robust backend solution designed to manage inventory, purchase orders, expenses, and item tracking efficiently.  
This project is built using **ASP.NET Core** and follows **Clean Architecture** principles to ensure scalability, maintainability, and testability.  
It also leverages **CQRS**, **Generic Repository & Unit of Work Pattern**, **Global Error Handling**, and **In-Memory Caching** for optimal performance.

---

## 🎯 Features
- **Inventory Management** – Add, update, and track items with serial numbers.
- **Purchase Orders & Support** – Register and manage purchase orders.
- **Unit & Recipient Management** – Assign items to recipients and track transfers.
- **Expense Archiving** – Log and attach expense documents (PDF/Image).
- **Stock Tracking** – Monitor available and consumed stock in real-time.
- **Return & Disposal Management** – Handle returns and disposal of assets with proper documentation.
- **Reporting** – Generate reports for:
  - Purchase Orders / Support Orders
  - Remaining Stock
  - Unit Expenses
  - Returned Items
  - Disposed Assets
  - Recipient Item Assignments

---

## 🏗️ Architecture
This project follows **Clean Architecture**, which promotes separation of concerns and testability.

---

### 🧩 Design Patterns Used
- **CQRS (Command Query Responsibility Segregation)** – Separates read and write operations for better scalability.
- **Generic Repository** – Reduces boilerplate code for data access.
- **Unit of Work** – Manages transactions across multiple repositories.
- **Global Error Handling** – Centralized exception handling for better API responses.
- **In-Memory Caching** – Improves performance by caching frequently accessed data.

---

## 🚀 Tech Stack
- **Backend:** ASP.NET Core Web API
- **Architecture:** Clean Architecture + CQRS
- **Database:** SQL Server (EF Core)
- **Caching:** In-Memory Cache
- **Documentation:** Swagger / OpenAPI
- **Testing:** xUnit / Moq

---

## ⚙️ Installation & Setup

### 1️⃣ Clone the Repository
```bash
git clone https://github.com/your-username/inventory-management-system.git
cd inventory-management-system
```
### 2️⃣ Configure Database

- ** Update the connection string in appsettings.json:
``` bash
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=InventoryDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

- ** Run EF Core migrations:
``` bash
dotnet ef database update
```
### 3️⃣ Run the Application
``` bash
dotnet run --project API
```

- ** Navigate to:
``` bash
https://localhost:5000/swagger
```

- ** to test endpoints.

## 📊 API Documentation
The project uses Swagger for API documentation.
You can explore all available endpoints (CRUD for inventory, expenses, orders, and reports) via the Swagger UI.

