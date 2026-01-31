# AccommodationBooking API

REST API dla systemu rezerwacji noclegów zbudowane w architekturze Clean Architecture z wykorzystaniem .NET 8.

> 📖 Ogólny opis aplikacji, funkcjonalności i role użytkowników znajdziesz w [głównym README](../README.md).

## 📋 Spis treści

- [Architektura](#-architektura)
- [Technologie](#-technologie)
- [Wymagania](#-wymagania)
- [Instalacja i uruchomienie](#-instalacja-i-uruchomienie)
- [Konfiguracja](#-konfiguracja)
- [Struktura projektu](#-struktura-projektu)
- [API Endpoints](#-api-endpoints)
- [Autentykacja](#-autentykacja)
- [Testowanie](#-testowanie)
- [Baza danych](#-baza-danych)
- [Powiązane zasoby](#-powiązane-zasoby)

## 🏗 Architektura

Projekt został zbudowany zgodnie z zasadami **Clean Architecture**:

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

### Wzorce projektowe

- **CQRS (Command Query Responsibility Segregation)** - rozdzielenie operacji odczytu i zapisu
- **MediatR** - implementacja wzorca Mediator dla handlerów komend i zapytań
- **Repository Pattern** - abstrakcja dostępu do danych
- **Unit of Work** - zarządzanie transakcjami bazodanowymi
- **DDD (Domain-Driven Design)** - modelowanie domeny biznesowej z agregatami i Value Objects

## 🛠 Technologie

| Kategoria        | Technologia           | Wersja |
| ---------------- | --------------------- | ------ |
| Framework        | .NET                  | 8.0    |
| ORM              | Entity Framework Core | 9.0.8  |
| Baza danych      | SQL Server            | -      |
| Autentykacja     | JWT Bearer            | 8.0.19 |
| Walidacja        | FluentValidation      | 12.0.0 |
| Mediator         | MediatR               | 13.0.0 |
| Mapping          | Mapster               | 7.4.0  |
| Haszowanie haseł | BCrypt.Net            | 4.0.3  |
| Dokumentacja API | Swagger/OpenAPI       | 6.6.2  |

## 📦 Wymagania

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/pl-pl/sql-server/sql-server-downloads) (LocalDB, Express lub pełna wersja)
- IDE: Visual Studio 2022 / VS Code / Rider

## 🚀 Instalacja i uruchomienie

### 1. Klonowanie repozytorium

```bash
git clone https://github.com/RBK14/accommodation-booking-app.git
cd accommodation-booking-app/accommodation-booking-api
```

### 2. Przywrócenie pakietów

```bash
dotnet restore
```

### 3. Konfiguracja bazy danych

Upewnij się, że SQL Server jest uruchomiony i zaktualizuj connection string w `appsettings.json` (patrz sekcja [Konfiguracja](#-konfiguracja)).

### 4. Migracje bazy danych

```bash
cd src/AccommodationBooking.Api
dotnet ef database update
```

### 5. Uruchomienie aplikacji

```bash
dotnet run --project src/AccommodationBooking.Api
```

Aplikacja będzie dostępna pod adresami:

- **HTTPS**: https://localhost:7295
- **HTTP**: http://localhost:5016
- **Swagger UI**: https://localhost:7295/swagger

## ⚙ Konfiguracja

### appsettings.json

```json
{
  "JwtSettings": {
    "Secret": "YourSuperSecretKeyAtLeast32Characters!",
    "ExpiryMinutes": "120",
    "Issuer": "AccommodationBooking",
    "Audience": "AccommodationBooking"
  },
  "ConnectionStrings": {
    "SqlServer": "Server=localhost;Database=AccommodationBooking;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;"
  }
}
```

### Zmienne środowiskowe

| Zmienna                        | Opis                      | Domyślna wartość |
| ------------------------------ | ------------------------- | ---------------- |
| `ASPNETCORE_ENVIRONMENT`       | Środowisko uruchomieniowe | `Development`    |
| `ConnectionStrings__SqlServer` | Connection string do bazy | -                |
| `JwtSettings__Secret`          | Klucz JWT (min. 32 znaki) | -                |

## 📁 Struktura projektu

```
accommodation-booking-api/
├── src/
│   ├── AccommodationBooking.Api/               # Warstwa prezentacji (Controllers, Mapping)
│   ├── AccommodationBooking.Application/       # Warstwa aplikacji (CQRS, Handlers, Validators)
│   ├── AccommodationBooking.Contracts/         # DTOs (Request/Response)
│   ├── AccommodationBooking.Domain/            # Warstwa domeny (Aggregates, Entities, Value Objects)
│   └── AccommodationBooking.Infrastructure/    # Warstwa infrastruktury (EF Core, Repositories, JWT)
├── tests/
│   ├── AccommodationBooking.Domain.UnitTests/              # Testy jednostkowe domeny
│   ├── AccommodationBooking.Application.UnitTests/         # Testy jednostkowe aplikacji
│   ├── AccommodationBooking.Application.IntegrationTests/  # Testy integracyjne
│   ├── AccommodationBooking.AcceptanceTests/               # Testy akceptacyjne (BDD)
│   └── AccommodationBooking.FunctionalTests/               # Testy funkcjonalne (Selenium)
└── AccommodationBooking.sln
```

### Warstwa Domain

```
Domain/
├── Common/
│   ├── Models/             # Entity, AggregateRoot, ValueObject
│   ├── ValueObjects/       # Address, Price
│   ├── Enums/              # Currency
│   ├── Errors/             # Definicje błędów domenowych
│   └── Exceptions/         # DomainException, DomainValidationException
├── UserAggregate/          # User + UserRole
├── GuestProfileAggregate/  # GuestProfile
├── HostProfileAggregate/   # HostProfile
├── ListingAggregate/       # Listing + Review + ScheduleSlot
└── ReservationAggregate/   # Reservation + ReservationStatus
```

### Warstwa Application (CQRS)

```
Application/
├── Common/
│   ├── Interfaces/      # IUnitOfWork, Repositories, IJwtTokenGenerator
│   └── Behaviors/       # ValidationBehavior (Pipeline)
├── Authentication/
│   ├── Commands/        # RegisterGuest, RegisterHost, UpdateEmail, UpdatePassword
│   └── Queries/         # Login
├── Listings/
│   ├── Commands/        # CreateListing, UpdateListing, DeleteListing, CreateReview...
│   └── Queries/         # GetListing, GetListings, GetAvailableDates...
├── Reservations/
│   ├── Commands/        # CreateReservation, UpdateStatus, DeleteReservation
│   └── Queries/         # GetReservation, GetReservations
└── Users/
    ├── Commands/        # UpdatePersonalDetails, DeleteGuest, DeleteHost, DeleteAdmin
    └── Queries/         # GetUser, GetUsers
```

## 🔌 API Endpoints

### Authentication (`/api/auth`)

| Metoda | Endpoint                    | Opis                       | Autoryzacja |
| ------ | --------------------------- | -------------------------- | ----------- |
| POST   | `/register-guest`           | Rejestracja gościa         | -           |
| POST   | `/register-host`            | Rejestracja gospodarza     | -           |
| POST   | `/register-admin`           | Rejestracja administratora | -           |
| POST   | `/login`                    | Logowanie (zwraca JWT)     | -           |
| POST   | `/{userId}/update-email`    | Zmiana email               | User/Admin  |
| POST   | `/{userId}/update-password` | Zmiana hasła               | User        |

### Users (`/api/users`)

| Metoda | Endpoint                        | Opis                     | Autoryzacja |
| ------ | ------------------------------- | ------------------------ | ----------- |
| GET    | `/`                             | Lista użytkowników       | -           |
| GET    | `/{id}`                         | Szczegóły użytkownika    | User/Admin  |
| POST   | `/{id}/update-personal-details` | Aktualizacja danych      | User/Admin  |
| DELETE | `/delete-guest/{id}`            | Usunięcie gościa         | Guest/Admin |
| DELETE | `/delete-host/{id}`             | Usunięcie gospodarza     | Host/Admin  |
| DELETE | `/delete-admin/{id}`            | Usunięcie administratora | Admin       |

### Listings (`/api/listings`)

| Metoda | Endpoint          | Opis                | Autoryzacja |
| ------ | ----------------- | ------------------- | ----------- |
| GET    | `/`               | Lista ofert         | -           |
| GET    | `/{id}`           | Szczegóły oferty    | -           |
| GET    | `/{id}/get-dates` | Dostępne daty       | -           |
| POST   | `/`               | Utworzenie oferty   | Host        |
| POST   | `/{id}`           | Aktualizacja oferty | Host/Admin  |
| DELETE | `/{id}`           | Usunięcie oferty    | Host/Admin  |

### Reservations (`/api/reservations`)

| Metoda | Endpoint | Opis                  | Autoryzacja      |
| ------ | -------- | --------------------- | ---------------- |
| GET    | `/`      | Lista rezerwacji      | -                |
| GET    | `/{id}`  | Szczegóły rezerwacji  | -                |
| POST   | `/`      | Utworzenie rezerwacji | Guest            |
| POST   | `/{id}`  | Zmiana statusu        | Guest/Host/Admin |
| DELETE | `/{id}`  | Usunięcie rezerwacji  | Admin            |

### Reviews (`/api/reviews`)

| Metoda | Endpoint | Opis                | Autoryzacja |
| ------ | -------- | ------------------- | ----------- |
| GET    | `/`      | Lista opinii        | -           |
| GET    | `/{id}`  | Szczegóły opinii    | -           |
| POST   | `/`      | Dodanie opinii      | Guest       |
| POST   | `/{id}`  | Aktualizacja opinii | Guest/Admin |
| DELETE | `/{id}`  | Usunięcie opinii    | Guest/Admin |

## 🔐 Autentykacja

API wykorzystuje **JWT (JSON Web Tokens)** do autentykacji.

### Struktura tokena

Token zawiera następujące claims:

- `sub` (NameIdentifier) - ID użytkownika
- `email` - adres email
- `given_name` - imię
- `family_name` - nazwisko
- `phone` - numer telefonu
- `role` - rola użytkownika (Guest/Host/Admin)
- `ProfileId` - ID profilu (dla Guest/Host)

### Użycie tokena

Dodaj token do nagłówka `Authorization`:

```
Authorization: Bearer <your_jwt_token>
```

### Role użytkowników

| Rola      | Uprawnienia                                                            |
| --------- | ---------------------------------------------------------------------- |
| **Guest** | Przeglądanie ofert, tworzenie rezerwacji, dodawanie opinii             |
| **Host**  | Zarządzanie własnymi ofertami, przeglądanie rezerwacji na swoje oferty |
| **Admin** | Pełny dostęp do wszystkich zasobów                                     |

## 🧪 Testowanie

### Uruchomienie wszystkich testów

```bash
dotnet test
```

### Uruchomienie konkretnego projektu testowego

```bash
# Testy jednostkowe domeny
dotnet test tests/AccommodationBooking.Domain.UnitTests

# Testy jednostkowe aplikacji
dotnet test tests/AccommodationBooking.Application.UnitTests

# Testy integracyjne
dotnet test tests/AccommodationBooking.Application.IntegrationTests

# Testy akceptacyjne (BDD)
dotnet test tests/AccommodationBooking.AcceptanceTests
```

### Struktura testów

```
tests/
├── Domain.UnitTests/
│   └── Aggregates/
│       ├── UserAggregate/
│       ├── ListingAggregate/
│       ├── ReservationAggregate/
│       └── GuestProfileAggregate/
├── Application.UnitTests/
│   ├── Listings/
│   │   ├── Commands/
│   │   └── Queries/
│   ├── Reservations/
│   │   └── Commands/
│   └── Users/
│       └── Commands/
├── Application.IntegrationTests/
├── AcceptanceTests/                 # SpecFlow BDD
│   ├── Features/
│   └── StepDefinitions/
└── FunctionalTests/                 # Selenium
    ├── PageObjects/
    └── Configuration/
```

## 🗄 Baza danych

### Tabele

| Tabela          | Opis                                     |
| --------------- | ---------------------------------------- |
| `Users`         | Użytkownicy systemu (Guest, Host, Admin) |
| `GuestProfiles` | Profile gości z listą rezerwacji         |
| `HostProfiles`  | Profile gospodarzy z listą ofert         |
| `Listings`      | Oferty noclegów z adresem i ceną         |
| `ScheduleSlots` | Zarezerwowane przedziały czasowe         |
| `Reviews`       | Opinie gości o ofertach                  |
| `Reservations`  | Rezerwacje ze szczegółami i statusem     |

### Migracje

```bash
# Dodanie nowej migracji
cd src/AccommodationBooking.Api
dotnet ef migrations add <MigrationName> --project ../AccommodationBooking.Infrastructure

# Aktualizacja bazy danych
dotnet ef database update

# Cofnięcie migracji
dotnet ef database update <PreviousMigrationName>
```

## 🔗 Powiązane zasoby

- [Główne README](../README.md) - Opis aplikacji i funkcjonalności
- [Frontend UI](../accommodation-booking-ui/README.md) - Dokumentacja techniczna aplikacji React
- [Entity Framework Core](https://docs.microsoft.com/ef/core/) - Dokumentacja ORM
- [MediatR](https://github.com/jbogard/MediatR) - Dokumentacja wzorca Mediator

---
