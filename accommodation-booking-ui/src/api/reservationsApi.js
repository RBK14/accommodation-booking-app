/**
 * Reservations API module
 * Handles all reservation-related API calls including CRUD operations and status updates.
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
 * Creates a new reservation.
 * @param {object} data - Reservation data (listingId, checkIn, checkOut)
 * @param {string} token - JWT authentication token
 * @returns {Promise<object>} Created reservation data
 * @throws {Error} Creation error message
 */
export const createReservation = async (data, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.post('/reservations', data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};

/**
 * Updates the status of a reservation.
 * @param {string} id - Reservation identifier
 * @param {string} status - New status (Accepted, InProgress, Completed, Cancelled, NoShow)
 * @param {string} token - JWT authentication token
 * @returns {Promise<object>} Updated reservation data
 * @throws {Error} Update error message
 */
export const updateReservationStatus = async (id, status, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.post(`/reservations/${id}`, { status });
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title ||
        error.response?.data?.message ||
        error.response?.data?.detail ||
        'An error occurred'
    );
  }
};

/**
 * Deletes a reservation by ID.
 * @param {string} id - Reservation identifier
 * @param {string} token - JWT authentication token
 * @throws {Error} Deletion error message
 */
export const deleteReservation = async (id, token) => {
  try {
    const authClient = createAuthClient(token);
    await authClient.delete(`/reservations/${id}`);
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};

/**
 * Retrieves reservations with optional filters.
 * @param {object} filters - Filter options (listingId, guestProfileId, hostProfileId)
 * @param {string} token - JWT authentication token
 * @returns {Promise<Array>} Array of reservation objects
 * @throws {Error} Fetch error message
 */
export const getReservations = async (filters = {}, token) => {
  try {
    const authClient = createAuthClient(token);
    const params = {};
    if (filters.listingId) params.listingId = filters.listingId;
    if (filters.guestProfileId) params.guestProfileId = filters.guestProfileId;
    if (filters.hostProfileId) params.hostProfileId = filters.hostProfileId;

    const response = await authClient.get('/reservations', { params });
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};

/**
 * Retrieves a single reservation by ID.
 * @param {string} id - Reservation identifier
 * @param {string} token - JWT authentication token
 * @returns {Promise<object>} Reservation data
 * @throws {Error} Fetch error message
 */
export const getReservation = async (id, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.get(`/reservations/${id}`);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};
