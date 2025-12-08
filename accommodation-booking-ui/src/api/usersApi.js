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
 * Aktualizacja danych osobowych użytkownika
 * @param {string} id - ID użytkownika
 * @param {Object} data - { firstName, lastName, phone }
 * @param {string} token - Token autoryzacyjny
 */
export const updatePersonalDetails = async (id, data, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.post(`/users/${id}/update-personal-details`, data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};

/**
 * Usunięcie konta gościa (Admin i Guest)
 * @param {string} id - ID użytkownika
 * @param {string} token - Token autoryzacyjny
 */
export const deleteGuest = async (id, token) => {
  try {
    const authClient = createAuthClient(token);
    await authClient.delete(`/users/delete-guest/${id}`);
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};

/**
 * Usunięcie konta gospodarza (Admin i Host)
 * @param {string} id - ID użytkownika
 * @param {string} token - Token autoryzacyjny
 */
export const deleteHost = async (id, token) => {
  try {
    const authClient = createAuthClient(token);
    await authClient.delete(`/users/delete-host/${id}`);
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};

/**
 * Usunięcie konta admina (tylko Admin)
 * @param {string} id - ID użytkownika
 * @param {string} token - Token autoryzacyjny
 */
export const deleteAdmin = async (id, token) => {
  try {
    const authClient = createAuthClient(token);
    await authClient.delete(`/users/delete-admin/${id}`);
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};

/**
 * Pobranie listy użytkowników
 * @param {string|null} userRole - Opcjonalnie filtruj według roli (Guest, Host, Admin)
 */
export const getUsers = async (userRole = null) => {
  try {
    const params = userRole ? { userRole } : {};
    const response = await apiClient.get('/users', { params });
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};

/**
 * Pobranie szczegółów użytkownika
 * @param {string} id - ID użytkownika
 */
export const getUser = async (id) => {
  try {
    const response = await apiClient.get(`/users/${id}`);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};
