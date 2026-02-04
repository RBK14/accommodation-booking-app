# 🏠 Accommodation Booking App

Accommodation booking system consisting of REST API (.NET 8) and frontend application (React + Vite).

## 📋 Table of Contents

- [About the project](#-about-the-project)
- [Features](#-features)
- [User roles](#-user-roles)
- [Architecture](#-architecture)
- [Technologies](#-technologies)
- [Requirements](#-requirements)
- [Quick start](#-quick-start)
- [Module documentation](#-module-documentation)
- [Testing](#-testing)
- [License](#-license)

## 📝 About the project

Accommodation Booking App is a comprehensive accommodation booking system that enables:

- **Browsing offers** - searching and filtering available accommodations
- **Booking accommodations** - intuitive reservation process
- **Managing offers** - creating and editing listings by hosts
- **Review system** - rating and reviewing accommodations by guests
- **Admin panel** - full control over the system

The application was designed with three types of users in mind: guests, hosts, and administrators.

## ✨ Features

### Public

- 🏠 Browse homepage with offers
- 🔍 Search accommodations with filters
- 📋 View offer details
- 🔐 Registration and login

### Guest Panel

- 👤 Account and profile management
- 📅 Booking accommodations
- 📝 Viewing reservation history
- ⭐ Posting and editing reviews

### Host Panel

- 🏡 Managing accommodation offers
- ➕ Adding new offers with prices and availability
- ✏️ Editing existing listings
- 📊 Viewing reservations for own offers
- 🔄 Changing reservation statuses (confirm, reject)

### Administrator Panel

- 👥 Managing all users
- 🏘️ Moderating accommodation offers
- 📈 Viewing all reservations
- ⚙️ Full control over the system

## 👥 User roles

| Role      | Description   | Permissions                                              |
| --------- | ------------- | -------------------------------------------------------- |
| **Guest** | Guest         | Browse offers, reservations, reviews, account management |
| **Host**  | Host          | Managing own offers, handling guest reservations         |
| **Admin** | Administrator | Full access to all resources and system functions        |

## 🏗 Architecture

The project consists of two main modules:

```
accommodation-booking-app/
├── accommodation-booking-api/    # Backend REST API (.NET 8)
│   └── Clean Architecture + CQRS + DDD
│
└── accommodation-booking-ui/     # Frontend (React + Vite)
    └── Material UI + React Router
```

### Backend (Clean Architecture)

```
┌─────────────────────────────────────────────────────────────┐
│                      Presentation                           │
│                   (AccommodationBooking.Api)                │
├─────────────────────────────────────────────────────────────┤
│                      Application                            │
│               (AccommodationBooking.Application)            │
│                    CQRS + MediatR                           │
├─────────────────────────────────────────────────────────────┤
│                        Domain                               │
│                 (AccommodationBooking.Domain)               │
│                    DDD Aggregates                           │
├─────────────────────────────────────────────────────────────┤
│                     Infrastructure                          │
│              (AccommodationBooking.Infrastructure)          │
│               EF Core + SQL Server + JWT                    │
└─────────────────────────────────────────────────────────────┘
```

### Design patterns

- **CQRS** - separation of read and write operations
- **MediatR** - implementation of Mediator pattern
- **Repository Pattern** - data access abstraction
- **Unit of Work** - transaction management
- **DDD** - domain modeling with aggregates and Value Objects

## 🛠 Technologies

### Backend

| Technology            | Version | Description      |
| --------------------- | ------- | ---------------- |
| .NET                  | 8.0     | Framework        |
| Entity Framework Core | 9.0.8   | ORM              |
| SQL Server            | -       | Database         |
| JWT Bearer            | 8.0.19  | Authentication   |
| FluentValidation      | 12.0.0  | Validation       |
| MediatR               | 13.0.0  | Mediator         |
| Mapster               | 7.4.0   | Mapping          |
| Swagger/OpenAPI       | 6.6.2   | API Documentation|

### Frontend

| Technology     | Version | Description     |
| -------------- | ------- | --------------- |
| React          | 19.2.0  | UI Library      |
| Vite           | 7.2.4   | Build tool      |
| Material UI    | 7.3.5   | UI Components   |
| React Router   | 7.9.6   | Routing         |
| Axios          | 1.13.2  | HTTP Client     |
| date-fns       | 4.1.0   | Date handling   |
| react-toastify | 11.0.5  | Notifications   |

## 📦 Requirements

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) >= 18.x
- [SQL Server](https://www.microsoft.com/pl-pl/sql-server/sql-server-downloads) (LocalDB, Express, or full version)
- npm >= 9.x

## 🚀 Quick start

### 1. Clone repository

```bash
git clone https://github.com/RBK14/accommodation-booking-app.git
cd accommodation-booking-app
```

### 2. Run backend

```bash
cd accommodation-booking-api
dotnet restore
cd src/AccommodationBooking.Api
dotnet ef database update
dotnet run
```

API will be available at: `https://localhost:7295`  
Swagger UI: `https://localhost:7295/swagger`

### 3. Run frontend

```bash
cd accommodation-booking-ui
npm install
npm run dev
```

Application will be available at: `http://localhost:5173`

## 📚 Module documentation

Detailed technical documentation can be found in the README of individual modules:

| Module          | Description                                 | Documentation                                                              |
| --------------- | ------------------------------------------- | -------------------------------------------------------------------------- |
| **Backend API** | REST API, configuration, endpoints, database| [accommodation-booking-api/README.md](accommodation-booking-api/README.md) |
| **Frontend UI** | React application, project structure, scripts| [accommodation-booking-ui/README.md](accommodation-booking-ui/README.md)   |

## 🧪 Testing

### Backend

```bash
cd accommodation-booking-api

# All tests
dotnet test

# Domain unit tests
dotnet test tests/AccommodationBooking.Domain.UnitTests

# Application unit tests
dotnet test tests/AccommodationBooking.Application.UnitTests

# Integration tests
dotnet test tests/AccommodationBooking.Application.IntegrationTests

# Acceptance tests (BDD)
dotnet test tests/AccommodationBooking.AcceptanceTests
```

### Frontend

```bash
cd accommodation-booking-ui

# Linting
npm run lint
```

## 📄 License

This project is released under the MIT License.

---

**Authors:**

- [Maciej](https://github.com/RBK14)
- [Wiktor](https://github.com/Czewski04)
