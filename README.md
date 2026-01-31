# 🏠 Accommodation Booking App

System rezerwacji noclegów składający się z REST API (.NET 8) oraz aplikacji frontendowej (React + Vite).

## 📋 Spis treści

- [O projekcie](#-o-projekcie)
- [Funkcjonalności](#-funkcjonalności)
- [Role użytkowników](#-role-użytkowników)
- [Architektura](#-architektura)
- [Technologie](#-technologie)
- [Wymagania](#-wymagania)
- [Szybki start](#-szybki-start)
- [Dokumentacja modułów](#-dokumentacja-modułów)
- [Testowanie](#-testowanie)
- [Licencja](#-licencja)

## 📝 O projekcie

Accommodation Booking App to kompleksowy system rezerwacji noclegów umożliwiający:

- **Przeglądanie ofert** - wyszukiwanie i filtrowanie dostępnych noclegów
- **Rezerwację noclegów** - intuicyjny proces składania rezerwacji
- **Zarządzanie ofertami** - tworzenie i edycja ogłoszeń przez gospodarzy
- **System opinii** - ocenianie i recenzowanie noclegów przez gości
- **Panel administracyjny** - pełna kontrola nad systemem

Aplikacja została zaprojektowana z myślą o trzech typach użytkowników: gościach, gospodarzach oraz administratorach.

## ✨ Funkcjonalności

### Publiczne

- 🏠 Przeglądanie strony głównej z ofertami
- 🔍 Wyszukiwanie noclegów z filtrami
- 📋 Podgląd szczegółów oferty
- 🔐 Rejestracja i logowanie

### Panel Gościa

- 👤 Zarządzanie kontem i profilem
- 📅 Rezerwacja noclegów
- 📝 Przeglądanie historii rezerwacji
- ⭐ Wystawianie i edycja opinii

### Panel Gospodarza

- 🏡 Zarządzanie ofertami noclegów
- ➕ Dodawanie nowych ofert z cenami i dostępnością
- ✏️ Edycja istniejących ogłoszeń
- 📊 Przeglądanie rezerwacji na swoje oferty
- 🔄 Zmiana statusów rezerwacji (potwierdzenie, odrzucenie)

### Panel Administratora

- 👥 Zarządzanie wszystkimi użytkownikami
- 🏘️ Moderacja ofert noclegów
- 📈 Przeglądanie wszystkich rezerwacji
- ⚙️ Pełna kontrola nad systemem

## 👥 Role użytkowników

| Rola      | Opis          | Uprawnienia                                                |
| --------- | ------------- | ---------------------------------------------------------- |
| **Guest** | Gość          | Przeglądanie ofert, rezerwacje, opinie, zarządzanie kontem |
| **Host**  | Gospodarz     | Zarządzanie własnymi ofertami, obsługa rezerwacji gości    |
| **Admin** | Administrator | Pełny dostęp do wszystkich zasobów i funkcji systemu       |

## 🏗 Architektura

Projekt składa się z dwóch głównych modułów:

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

### Wzorce projektowe

- **CQRS** - rozdzielenie operacji odczytu i zapisu
- **MediatR** - implementacja wzorca Mediator
- **Repository Pattern** - abstrakcja dostępu do danych
- **Unit of Work** - zarządzanie transakcjami
- **DDD** - modelowanie domeny z agregatami i Value Objects

## 🛠 Technologie

### Backend

| Technologia           | Wersja | Opis             |
| --------------------- | ------ | ---------------- |
| .NET                  | 8.0    | Framework        |
| Entity Framework Core | 9.0.8  | ORM              |
| SQL Server            | -      | Baza danych      |
| JWT Bearer            | 8.0.19 | Autentykacja     |
| FluentValidation      | 12.0.0 | Walidacja        |
| MediatR               | 13.0.0 | Mediator         |
| Mapster               | 7.4.0  | Mapping          |
| Swagger/OpenAPI       | 6.6.2  | Dokumentacja API |

### Frontend

| Technologia    | Wersja | Opis          |
| -------------- | ------ | ------------- |
| React          | 19.2.0 | Biblioteka UI |
| Vite           | 7.2.4  | Build tool    |
| Material UI    | 7.3.5  | Komponenty UI |
| React Router   | 7.9.6  | Routing       |
| Axios          | 1.13.2 | Klient HTTP   |
| date-fns       | 4.1.0  | Obsługa dat   |
| react-toastify | 11.0.5 | Powiadomienia |

## 📦 Wymagania

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) >= 18.x
- [SQL Server](https://www.microsoft.com/pl-pl/sql-server/sql-server-downloads) (LocalDB, Express lub pełna wersja)
- npm >= 9.x

## 🚀 Szybki start

### 1. Klonowanie repozytorium

```bash
git clone https://github.com/RBK14/accommodation-booking-app.git
cd accommodation-booking-app
```

### 2. Uruchomienie backendu

```bash
cd accommodation-booking-api
dotnet restore
cd src/AccommodationBooking.Api
dotnet ef database update
dotnet run
```

API będzie dostępne pod adresem: `https://localhost:7295`  
Swagger UI: `https://localhost:7295/swagger`

### 3. Uruchomienie frontendu

```bash
cd accommodation-booking-ui
npm install
npm run dev
```

Aplikacja będzie dostępna pod adresem: `http://localhost:5173`

## 📚 Dokumentacja modułów

Szczegółowa dokumentacja techniczna znajduje się w README poszczególnych modułów:

| Moduł           | Opis                                           | Dokumentacja                                                               |
| --------------- | ---------------------------------------------- | -------------------------------------------------------------------------- |
| **Backend API** | REST API, konfiguracja, endpointy, baza danych | [accommodation-booking-api/README.md](accommodation-booking-api/README.md) |
| **Frontend UI** | Aplikacja React, struktura projektu, skrypty   | [accommodation-booking-ui/README.md](accommodation-booking-ui/README.md)   |

## 🧪 Testowanie

### Backend

```bash
cd accommodation-booking-api

# Wszystkie testy
dotnet test

# Testy jednostkowe domeny
dotnet test tests/AccommodationBooking.Domain.UnitTests

# Testy jednostkowe aplikacji
dotnet test tests/AccommodationBooking.Application.UnitTests

# Testy integracyjne
dotnet test tests/AccommodationBooking.Application.IntegrationTests

# Testy akceptacyjne (BDD)
dotnet test tests/AccommodationBooking.AcceptanceTests
```

### Frontend

```bash
cd accommodation-booking-ui

# Linting
npm run lint
```

## 📄 Licencja

Ten projekt jest udostępniony na licencji MIT.

---

**Autorzy:**

- [Maciej](https://github.com/RBK14)
- [Wiktor](https://github.com/Czewski04)
