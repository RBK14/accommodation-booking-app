/**
 * Users API module
 * Handles all user-related API calls including profile updates and user management.
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
 * Updates user's personal details (name, phone).
 * @param {string} id - User identifier
 * @param {object} data - Personal details (firstName, lastName, phone)
 * @param {string} token - JWT authentication token
 * @returns {Promise<object>} Update response
 * @throws {Error} Update error message
 */
export const updatePersonalDetails = async (id, data, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.post(`/users/${id}/update-personal-details`, data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};

/**
 * Deletes a guest user account.
 * @param {string} id - User identifier
 * @param {string} token - JWT authentication token
 * @throws {Error} Deletion error message
 */
export const deleteGuest = async (id, token) => {
  try {
    const authClient = createAuthClient(token);
    await authClient.delete(`/users/delete-guest/${id}`);
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};

/**
 * Deletes a host user account.
 * @param {string} id - User identifier
 * @param {string} token - JWT authentication token
 * @throws {Error} Deletion error message
 */
export const deleteHost = async (id, token) => {
  try {
    const authClient = createAuthClient(token);
    await authClient.delete(`/users/delete-host/${id}`);
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};

/**
 * Deletes an admin user account.
 * @param {string} id - User identifier
 * @param {string} token - JWT authentication token
 * @throws {Error} Deletion error message
 */
export const deleteAdmin = async (id, token) => {
  try {
    const authClient = createAuthClient(token);
    await authClient.delete(`/users/delete-admin/${id}`);
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};

/**
 * Retrieves users list, optionally filtered by role.
 * @param {string|null} userRole - Optional role filter (Guest, Host, Admin)
 * @param {string} token - JWT authentication token
 * @returns {Promise<Array>} Array of user objects
 * @throws {Error} Fetch error message
 */
export const getUsers = async (userRole = null, token) => {
  try {
    const authClient = createAuthClient(token);
    const params = userRole ? { userRole } : {};
    const response = await authClient.get('/users', { params });
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};

/**
 * Retrieves a single user by ID.
 * @param {string} id - User identifier
 * @param {string} token - JWT authentication token
 * @returns {Promise<object>} User data
 * @throws {Error} Fetch error message
 */
export const getUser = async (id, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.get(`/users/${id}`);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};
