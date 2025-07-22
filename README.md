# 🖥️ PCShop - ASP.NET Core E-Commerce

PCShop is a full-featured e-commerce web application built with **ASP.NET Core**, allowing users to browse, filter, and purchase PC components and computers. It includes advanced features such as user authentication, shopping cart, order confirmation, product management, and responsive UI using Bootstrap.

---

## ✨ Features

- 🔐 User Registration, Login, and Profile Management (ASP.NET Identity)
- 🛍️ Browse, Filter, Sort, and Search Products by Type, Name, or Price
- 🖥️ Manage Computer Listings Separately from Products
- 🛒 Shopping Cart with Quantity Management and Order Finalization
- 📦 Order Confirmation with Pre-Filled User Profile Data
- 💳 Stripe for payment processing via card
- 📤 Product Image Upload, Replace, and Auto-delete Old Files
- 📃 Clean Razor Pages & MVC Architecture
- 💬 Global TempData Messages (Auto-hide Toasts with Icons)
- 📄 Pagination and Server-Side Filtering
- 🧰 Repository Pattern and Service Layer Separation
- 📦 Entity Framework Core with Migrations
- 🎨 Responsive UI with Bootstrap 5 & FontAwesome Icons
---

## 🛠️ Technologies Used

- **ASP.NET Core 8.0**
- **Entity Framework Core**
- **Razor Pages / MVC**
- **SQL Server**
- **Bootstrap 5**
- **FontAwesome / Bootstrap Icons**
- **jQuery** (for toast behavior)
- **LINQ, TempData, Tag Helpers, Model Validation**
- **Stripe** (for payment processing)

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

* `PCShop.Data` – DbContext, Repositories, Migrations, and seed data
* `PCShop.Data.Models` – Entity models 
* `PCShop.Data.Common` – Entity validation constants 
* `PCShop.Services.Core` – Business logic and service layer
* `PCShop.Services.Common` – Service constants
* `PCShop.Tests` – Unit testing using NUnit and Moq
* `PCShop.Web` – MVC presentation layer (controllers + views)
* `PCShop.Web.Infrastructure` – TagHelpers, Extensions and etc.
* `PCShop.Web.ViewModels` – All view models
* `PCShop.GCommon` – Constants and validation rules

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