# 🚴‍♂️ Cycling Races Web Application

This web application allows users to create, manage, and participate in cycling races. It supports event organization, participant registration, results management, and automatic ranking generation.

## 📌 Features

- 🛠️ Create and manage cycling races
- 👤 User registration and login (cyclists & organizers)
- 📝 Participant and volunteer sign-up
- 🏁 Record and view race results
- 🥇 Auto-generated rankings
- 📊 Filter and sort events
- 📁 Admin-only management options

## 🧱 Technologies Used

- **ASP.NET Core MVC** – Backend framework
- **Entity Framework Core** – ORM and database access
- **SQL Server** – Relational database
- **Razor Views (.cshtml)** – Server-side rendering
- **JavaScript, Bootstrap 5, CSS** – Frontend styling and interactivity
- **ASP.NET Core Identity** – Authentication and authorization

## 🧑‍💻 User Roles

- **Admin** – Full access to all features and users
- **Organiser** – Can create and manage races
- **Cyclist** – Can register for races and view results

## 🚀 Getting Started

1. Clone the repository:
   git clone https://github.com/EmanuilGenov/Web-Application-for-Cycling-Race-Rankings-and-Results#
   
Open the solution in Visual Studio 2022+

Update appsettings.json with your local SQL Server connection string.

Apply migrations and seed the database:

Update-Database
Run the application using:

dotnet run

🔐 Authentication
The system uses ASP.NET Core Identity for:

Email-based registration

Role-based access (Admin, Organiser, Cyclist)
