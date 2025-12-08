import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'https://localhost:7295/api';

/**
 * Konfiguracja axios instance
 */
const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

/**
 * Interceptor do dodawania tokenu do requestów
 */
const createAuthClient = (token) => {
  const authClient = axios.create({
    baseURL: API_BASE_URL,
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    },
  });
  return authClient;
};

/**
 * Utworzenie nowej oferty (tylko dla Host)
 * @param {Object} data - { title, description, accommodationType, beds, maxGuests, country, city, postalCode, street, buildingNumber, amountPerDay, currency }
 * @param {string} token - Token autoryzacyjny
 */
export const createListing = async (data, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.post('/listings', data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};

/**
 * Aktualizacja oferty (Host i Admin)
 * @param {string} id - ID oferty
 * @param {Object} data - Dane do aktualizacji
 * @param {string} token - Token autoryzacyjny
 */
export const updateListing = async (id, data, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.post(`/listings/${id}`, data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};

/**
 * Usunięcie oferty (Host i Admin)
 * @param {string} id - ID oferty
 * @param {string} token - Token autoryzacyjny
 */
export const deleteListing = async (id, token) => {
  try {
    const authClient = createAuthClient(token);
    await authClient.delete(`/listings/${id}`);
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};

/**
 * Pobranie listy ofert
 * @param {string|null} hostProfileId - Opcjonalne ID profilu gospodarza do filtrowania
 * @param {string} token - Token autoryzacyjny
 */
export const getListings = async (hostProfileId = null, token) => {
  try {
    const authClient = createAuthClient(token);
    const params = hostProfileId ? { hostProfileId } : {};
    const response = await authClient.get('/listings', { params });
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};

/**
 * Pobranie szczegółów oferty
 * @param {string} id - ID oferty
 * @param {string} token - Token autoryzacyjny
 */
export const getListing = async (id, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.get(`/listings/${id}`);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};

/**
 * Pobranie dostępnych dat dla oferty
 * @param {string} id - ID oferty
 * @param {string|null} from - Data początkowa (opcjonalna)
 * @param {number|null} days - Liczba dni (opcjonalna)
 * @param {string} token - Token autoryzacyjny
 */
export const getAvailableDates = async (id, from = null, days = null, token) => {
  try {
    const authClient = createAuthClient(token);
    const params = {};
    if (from) params.from = from;
    if (days) params.days = days;

    const response = await authClient.get(`/listings/${id}/get-dates`, { params });
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};
