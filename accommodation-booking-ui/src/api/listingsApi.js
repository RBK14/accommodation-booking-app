/**
 * Listings API module
 * Handles all listing-related API calls including CRUD operations and availability queries.
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
 * Creates a new accommodation listing.
 * @param {object} data - Listing data (title, description, price, etc.)
 * @param {string} token - JWT authentication token
 * @returns {Promise<object>} Created listing data
 * @throws {Error} Creation error message
 */
export const createListing = async (data, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.post('/listings', data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};

/**
 * Updates an existing listing.
 * @param {string} id - Listing identifier
 * @param {object} data - Updated listing data
 * @param {string} token - JWT authentication token
 * @returns {Promise<object>} Updated listing data
 * @throws {Error} Update error message
 */
export const updateListing = async (id, data, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.post(`/listings/${id}`, data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};

/**
 * Deletes a listing by ID.
 * @param {string} id - Listing identifier
 * @param {string} token - JWT authentication token
 * @throws {Error} Deletion error message
 */
export const deleteListing = async (id, token) => {
  try {
    const authClient = createAuthClient(token);
    await authClient.delete(`/listings/${id}`);
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};

/**
 * Retrieves listings, optionally filtered by host profile.
 * @param {string|null} hostProfileId - Optional host profile ID filter
 * @returns {Promise<Array>} Array of listing objects
 * @throws {Error} Fetch error message
 */
export const getListings = async (hostProfileId = null) => {
  try {
    const params = hostProfileId ? { hostProfileId } : {};
    const response = await apiClient.get('/listings', { params });
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};

/**
 * Retrieves a single listing by ID.
 * @param {string} id - Listing identifier
 * @returns {Promise<object>} Listing data
 * @throws {Error} Fetch error message
 */
export const getListing = async (id) => {
  try {
    const response = await apiClient.get(`/listings/${id}`);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};

/**
 * Retrieves available dates for a listing.
 * @param {string} id - Listing identifier
 * @param {string|null} from - Start date for availability query (ISO format)
 * @param {number|null} days - Number of days to check availability for
 * @param {string} token - JWT authentication token
 * @returns {Promise<Array>} Array of available date strings
 * @throws {Error} Fetch error message
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
      error.response?.data?.title || error.response?.data?.message || 'An error occurred'
    );
  }
};
