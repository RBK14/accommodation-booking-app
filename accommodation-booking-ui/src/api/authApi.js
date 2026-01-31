/**
 * Authentication API module
 * Handles all authentication-related API calls including registration, login, and credential updates.
 */
import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'https://localhost:7295/api';

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

/**
 * Creates an authenticated axios client with Bearer token.
 * @param {string} token - JWT authentication token
 * @returns {AxiosInstance} Configured axios instance with auth header
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
 * Registers a new guest user account.
 * @param {object} data - Registration data (email, password, firstName, lastName)
 * @returns {Promise<object>} Created user data
 * @throws {Error} Registration error message
 */
export const registerGuest = async (data) => {
  try {
    const response = await apiClient.post('/auth/register-guest', data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};

/**
 * Registers a new host user account.
 * @param {object} data - Registration data (email, password, firstName, lastName)
 * @returns {Promise<object>} Created user data
 * @throws {Error} Registration error message
 */
export const registerHost = async (data) => {
  try {
    const response = await apiClient.post('/auth/register-host', data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};

/**
 * Registers a new admin user account.
 * @param {object} data - Registration data (email, password, firstName, lastName)
 * @returns {Promise<object>} Created user data
 * @throws {Error} Registration error message
 */
export const registerAdmin = async (data) => {
  try {
    const response = await apiClient.post('/auth/register-admin', data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};

/**
 * Authenticates user with credentials.
 * @param {object} credentials - Login credentials (email, password)
 * @returns {Promise<object>} Authentication response with token
 * @throws {Error} Login error message
 */
export const login = async (credentials) => {
  try {
    const response = await apiClient.post('/auth/login', credentials);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};

/**
 * Updates user email address.
 * @param {string} userId - User identifier
 * @param {object} data - New email data
 * @param {string} token - JWT authentication token
 * @returns {Promise<object>} Update response
 * @throws {Error} Update error message
 */
export const updateEmail = async (userId, data, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.post(`/auth/${userId}/update-email`, data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};

/**
 * Updates user password.
 * @param {string} userId - User identifier
 * @param {object} data - Password data (currentPassword, newPassword)
 * @param {string} token - JWT authentication token
 * @returns {Promise<object>} Update response
 * @throws {Error} Update error message
 */
export const updatePassword = async (userId, data, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.post(`/auth/${userId}/update-password`, data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};
