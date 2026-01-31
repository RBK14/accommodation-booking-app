# Accommodation Booking UI

Frontend aplikacji do rezerwacji noclegów, zbudowany przy użyciu React i Vite.

> 📖 Ogólny opis aplikacji, funkcjonalności i role użytkowników znajdziesz w [głównym README](../README.md).

## Spis treści

- [Technologie](#technologie)
- [Wymagania](#wymagania)
- [Instalacja](#instalacja)
- [Uruchomienie](#uruchomienie)
- [Struktura projektu](#struktura-projektu)
- [Konfiguracja](#konfiguracja)
- [Skrypty](#skrypty)
- [Powiązane zasoby](#powiązane-zasoby)

## Technologie

| Technologia    | Wersja | Opis                    |
| -------------- | ------ | ----------------------- |
| React          | 19.2.0 | Biblioteka UI           |
| Vite           | 7.2.4  | Build tool i dev server |
| Material UI    | 7.3.5  | Biblioteka komponentów  |
| React Router   | 7.9.6  | Routing aplikacji       |
| Axios          | 1.13.2 | Klient HTTP             |
| date-fns       | 4.1.0  | Obsługa dat             |
| react-toastify | 11.0.5 | Powiadomienia           |

## Wymagania

- Node.js >= 18.x
- npm >= 9.x
- Uruchomiony backend API (domyślnie na `https://localhost:7295`)

## Instalacja

1. Sklonuj repozytorium:

```bash
git clone <repository-url>
cd accommodation-booking-ui
```

2. Zainstaluj zależności:

```bash
npm install
```

## Uruchomienie

### Tryb deweloperski

```bash
npm run dev
```

Aplikacja będzie dostępna pod adresem `http://localhost:5173`

### Build produkcyjny

```bash
npm run build
```

### Podgląd buildu produkcyjnego

```bash
npm run preview
```

## Struktura projektu

```
src/
├── api/                    # Funkcje komunikacji z API
│   ├── authApi.js          # Autentykacja (login, rejestracja)
│   ├── listingsApi.js      # Oferty noclegów
│   ├── reservationsApi.js  # Rezerwacje
│   ├── reviewsApi.js       # Opinie
│   └── usersApi.js         # Użytkownicy
│
├── assets/                 # Zasoby statyczne
│   ├── icons/              # Ikony
│   └── styles/             # Style i kolory
│
├── components/             # Komponenty React
│   ├── auth/               # Komponenty autentykacji
│   ├── guest/              # Komponenty panelu gościa
│   ├── host/               # Komponenty panelu gospodarza
│   └── shared/             # Współdzielone komponenty
│
├── constants/              # Stałe aplikacji
│   └── routes.js           # Definicje ścieżek
│
├── context/                # React Context
│   ├── AuthContext.js      # Kontekst autentykacji
│   └── AuthProvider.jsx    # Provider autentykacji
│
├── hooks/                  # Custom hooks
│   ├── useAuth.js          # Hook autentykacji
│   ├── useAuthApi.js       # Hook API autentykacji
│   ├── useListingsApi.js   # Hook API ofert
│   ├── useReservationsApi.js # Hook API rezerwacji
│   ├── useReviewsApi.js    # Hook API opinii
│   └── useUsersApi.js      # Hook API użytkowników
│
├── layouts/                # Layouty stron
│   ├── AdminLayout.jsx     # Layout panelu admina
│   ├── AuthLayout.jsx      # Layout stron logowania
│   ├── GuestLayout.jsx     # Layout panelu gościa
│   ├── HostLayout.jsx      # Layout panelu gospodarza
│   └── MainLayout.jsx      # Główny layout publiczny
│
├── pages/                  # Strony aplikacji
│   ├── admin/              # Strony panelu admina
│   ├── auth/               # Strony logowania/rejestracji
│   ├── guest/              # Strony panelu gościa
│   └── host/               # Strony panelu gospodarza
│
├── router/                 # Konfiguracja routingu
│   └── ProtectedRoute.jsx  # Ochrona tras
│
├── utils/                  # Funkcje pomocnicze
│   ├── accommodationTypeMapper.js  # Mapowanie typów noclegów
│   └── reservationStatusMapper.js  # Mapowanie statusów rezerwacji
│
├── App.jsx                 # Główny komponent aplikacji
├── main.jsx                # Punkt wejścia aplikacji
└── index.css               # Globalne style
```

## Konfiguracja

### Zmienne środowiskowe

Aplikacja korzysta z domyślnego adresu API: `https://localhost:7295/api`

Aby zmienić adres API, zmodyfikuj pliki w katalogu `src/api/`.

### Konfiguracja Vite

Plik `vite.config.js` zawiera konfigurację build tool:

- Plugin React
- Konfiguracja serwera deweloperskiego

## Skrypty

| Skrypt            | Opis                                 |
| ----------------- | ------------------------------------ |
| `npm run dev`     | Uruchomienie serwera deweloperskiego |
| `npm run build`   | Build produkcyjny                    |
| `npm run preview` | Podgląd buildu produkcyjnego         |
| `npm run lint`    | Sprawdzenie kodu przez ESLint        |

## Powiązane zasoby

- [Główne README](../README.md) - Opis aplikacji i funkcjonalności
- [Backend API](../accommodation-booking-api/README.md) - Dokumentacja techniczna REST API
- [Material UI](https://mui.com/) - Dokumentacja komponentów
- [React Router](https://reactrouter.com/) - Dokumentacja routingu

---
