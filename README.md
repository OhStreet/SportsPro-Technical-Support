# SportsPro

SportsPro is a concise ASP.NET Core web application for managing customer support data: customers, products, incidents, and technicians. It provides a straightforward administrative interface for listing, adding, editing, and deleting records related to support operations.

## Core Features

- **Customer management**: List, add, edit, and delete customer records.
- **Product management**: Manage products linked to support incidents.
- **Incident management**: Create and assign incidents to customers and technicians.
- **Technician management**: List, add, edit, and delete technician records.
- **Server-side rendering**: Utilize Razor views for dynamic content rendering.

## Architecture & Key Components

- **ASP.NET Core MVC**: Utilizes Razor views for user interface and routing.
- **Entity Framework Core**: Handles data access through a DbContext named `SportsProContext`.
- **Controllers**: Located in the `Controllers/` directory, including:
  - `CustomerController`
  - `ProductController`
  - `IncidentController`
  - `TechnicianController`
  - `HomeController`
- **Views**: Organized by controller in the `Views/` directory (e.g., `Views/Customer`, `Views/Technician`).
- **Models**: Defined in the `Models/` directory, including:
  - `Customer`
  - `Product`
  - `Incident`
  - `Technician`
  - `Country`
- **Database migrations**: Managed under the `Migrations/` directory for version control.

## Data Model

The primary domain entities include:

- **Customer**: Represents customer records and contact details.
- **Product**: Products linked to support incidents.
- **Incident**: Support incidents associated with customers and technicians.
- **Technician**: Support technicians assigned to incidents.
- **Country**: Lookup for customer address data.

## Technology Stack

- **.NET 6**
- **C# 10**
- **ASP.NET Core MVC**: Utilizes Razor views for the user interface.
- **Entity Framework Core**: Implements code-first migrations for database management.
- **Bootstrap**: Provides basic UI styling through the project layout.


