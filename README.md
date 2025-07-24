# 🖥️ PCShop - ASP.NET Core E-Commerce

PCShop is a full-featured e-commerce web application built with **ASP.NET Core**, allowing users to browse, filter, and purchase PC components and computers. It includes advanced features such as user authentication, shopping cart, order confirmation, product management, and responsive UI using Bootstrap.

---

## ✨ Features

- 🔐 **User Authentication & Profile Management**
  - Register, login, and manage profile via ASP.NET Identity
- 🛍️ **Product Catalog**
  - Browse, filter, sort, and search products by type, name, or price
- 🖥️ **Computer Listings**
  - Manage computer systems separately from individual products
- 🛒 **Shopping Cart**
  - Add products or computers to cart with quantity management and finalization
- 📦 **Order Confirmation**
  - Pre-filled user details from profile; supports delivery method selection and comment
- 💳 **Payment Integration**
  - Stripe support for secure credit/debit card payments
- 📤 **Image Management**
  - Upload, replace, and automatically delete old product images
- 📃 **Clean MVC & Razor Architecture**
  - Separation of concerns with Controllers, Services, and Repositories
- 💬 **Global TempData Messaging**
  - Toast-style messages with icons and auto-hide animations
- 📄 **Pagination, Sorting, and Filtering**
  - Server-side implementation for both Products and Computers
- 🧰 **Repository Pattern & Service Layer**
  - Clean code architecture for maintainability and testability
- 📦 **Entity Framework Core**
  - Code-first migrations and query filters for soft deletes
- 🎨 **Responsive UI**
  - Built with Bootstrap 5, FontAwesome, and clean layout components
- 🗂️ **Area-Based Role Management**
  - 💼 **Admin Area** – Manage users, products, and restore/delete soft-deleted records  
  - 📦 **Manager Area** – View, filter, approve, and delete pending orders  
  - 👤 **User Area** – View orders, manage profile, and complete purchases

---

## 🛠️ Technologies Used

- **ASP.NET Core 8.0** – Main web framework (MVC & Razor Pages)
- **Entity Framework Core** – ORM for database access with LINQ
- **SQL Server** – Relational database (code-first migrations)
- **Razor Pages / MVC** – View rendering and structured routing
- **Bootstrap 5** – Responsive UI framework
- **FontAwesome / Bootstrap Icons** – UI icons and visuals
- **jQuery** – Used for dynamic features like toast auto-dismiss
- **TempData, Tag Helpers, Model Validation** – Built-in ASP.NET Core features
- **Stripe** – Payment processing via credit/debit cards

---

## 🧪 Test Credentials
You can use the following test accounts to try out the application:

- **Stripe** test card:
  * Number: 4242 4242 4242 4242
  * Date: 10/34 (any future date)
  * CVC: 123 (any 3 digits)
  
- **Admin:**
  * Email: admin@pcshop.com
  * Password: Admin123!

- **Manager:**
  * Email: manager@pcshop.com
  * Password: Manager123!
  
- **User:**
  * Email: user@pcshop.com
  * Password: User123!

---

## 📂 Project Structure

- `PCShop.Data` – Contains the `DbContext`, repositories, migrations, and seed data
- `PCShop.Data.Models` – Entity models representing database tables
- `PCShop.Data.Common` – Common validation constants for entities
- `PCShop.Services.Core` – Core business logic and service interfaces/implementations
- `PCShop.Services.Common` – Shared constants used across services
- `PCShop.Tests` – Unit tests using NUnit and Moq
- `PCShop.Web` – Presentation layer (MVC controllers, Razor views, and startup config)
- `PCShop.Web.Infrastructure` – TagHelpers, middleware, extensions, and settings
- `PCShop.Web.ViewModels` – ViewModels used for data transfer between views and controllers
- `PCShop.GCommon` – Global constants, enums, and shared validation logic

---

## 🚀 How to Run the Project

1. Clone the repository:
   ```bash
   git clone https://github.com/Alexandr0w/PCShop.git
   ```

2. Navigate to the project directory:
   ```bash
   cd PCShop
   ```

3. Restore dependencies:
   ```bash
   dotnet restore
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

5. Open your browser and navigate to:
   ```
   http://localhost:{localPort}
   ```

---

## 📜 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

Enjoy exploring the **PCShop** project! 🖥️