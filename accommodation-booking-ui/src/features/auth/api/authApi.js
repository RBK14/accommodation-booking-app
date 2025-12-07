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
 * Rejestracja gościa
 * @param {Object} data - { email, password, firstName, lastName, phone }
 */
export const registerGuest = async (data) => {
  try {
    const response = await apiClient.post('/auth/register-guest', data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};

/**
 * Rejestracja gospodarza
 * @param {Object} data - { email, password, firstName, lastName, phone }
 */
export const registerHost = async (data) => {
  try {
    const response = await apiClient.post('/auth/register-host', data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};

/**
 * Rejestracja admina
 * @param {Object} data - { email, password, firstName, lastName, phone }
 */
export const registerAdmin = async (data) => {
  try {
    const response = await apiClient.post('/auth/register-admin', data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};

/**
 * Logowanie
 * @param {Object} credentials - { email, password }
 * @returns {Promise<{id: string, accessToken: string}>}
 */
export const login = async (credentials) => {
  try {
    const response = await apiClient.post('/auth/login', credentials);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};

/**
 * Aktualizacja email
 * @param {string} userId - ID użytkownika
 * @param {Object} data - { email }
 * @param {string} token - Token autoryzacyjny
 */
export const updateEmail = async (userId, data, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.post(`/auth/${userId}/update-email`, data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};

/**
 * Aktualizacja hasła
 * @param {string} userId - ID użytkownika
 * @param {Object} data - { password, newPassword }
 * @param {string} token - Token autoryzacyjny
 */
export const updatePassword = async (userId, data, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.post(`/auth/${userId}/update-password`, data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'Wystąpił błąd'
    );
  }
};
