/**
 * Konfiguracja użytkowników testowych
 * Zmień te dane na rzeczywiste dane z Twojej bazy danych
 */

export const testUsers = {
  guest: {
    email: 'guest1@example.com',
    password: 'P@ssw0rd',
  },
  host: {
    email: 'host1@example.com',
    password: 'P@ssw0rd',
  },
  admin: {
    email: 'admin@example.com',
    password: 'P@ssw0rd',
  },
};

/**
 * Konfiguracja aplikacji dla testów
 */
export const testConfig = {
  baseURL: 'http://localhost:5173',
  timeout: {
    default: 30000,
    navigation: 15000,
    element: 10000,
  },
};
