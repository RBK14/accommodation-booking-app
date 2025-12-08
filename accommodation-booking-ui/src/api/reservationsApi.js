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
 * Utworzenie nowej rezerwacji (tylko dla Guest)
 * @param {Object} data - { listingId, checkIn, checkOut }
 * @param {string} token - Token autoryzacyjny
 */
export const createReservation = async (data, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.post('/reservations', data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};

/**
 * Aktualizacja statusu rezerwacji (Admin, Host, Guest)
 * @param {string} id - ID rezerwacji
 * @param {Object} data - { status }
 * @param {string} token - Token autoryzacyjny
 */
export const updateReservationStatus = async (id, data, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.post(`/reservations/${id}`, data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};

/**
 * Usunięcie rezerwacji (tylko Admin)
 * @param {string} id - ID rezerwacji
 * @param {string} token - Token autoryzacyjny
 */
export const deleteReservation = async (id, token) => {
  try {
    const authClient = createAuthClient(token);
    await authClient.delete(`/reservations/${id}`);
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};

/**
 * Pobranie listy rezerwacji
 * @param {Object} filters - { listingId?, guestProfileId?, hostProfileId? }
 */
export const getReservations = async (filters = {}) => {
  try {
    const params = {};
    if (filters.listingId) params.listingId = filters.listingId;
    if (filters.guestProfileId) params.guestProfileId = filters.guestProfileId;
    if (filters.hostProfileId) params.hostProfileId = filters.hostProfileId;

    const response = await apiClient.get('/reservations', { params });
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};

/**
 * Pobranie szczegółów rezerwacji
 * @param {string} id - ID rezerwacji
 */
export const getReservation = async (id) => {
  try {
    const response = await apiClient.get(`/reservations/${id}`);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};
