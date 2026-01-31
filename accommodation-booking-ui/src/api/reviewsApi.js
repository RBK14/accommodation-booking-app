/**
 * Reviews API module
 * Handles all review-related API calls including CRUD operations.
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
 * Creates a new review for a listing.
 * @param {object} data - Review data (listingId, rating, comment)
 * @param {string} token - JWT authentication token
 * @returns {Promise<object>} Created review data
 * @throws {Error} Creation error message
 */
export const createReview = async (data, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.post('/reviews', data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title ||
        error.response?.data?.message ||
        error.response?.data?.detail ||
        JSON.stringify(error.response?.data) ||
        'An error occurred'
    );
  }
};

/**
 * Updates an existing review.
 * @param {string} id - Review identifier
 * @param {object} data - Updated review data (rating, comment)
 * @param {string} token - JWT authentication token
 * @returns {Promise<object>} Updated review data
 * @throws {Error} Update error message
 */
export const updateReview = async (id, data, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.post(`/reviews/${id}`, data);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title ||
        error.response?.data?.message ||
        error.response?.data?.detail ||
        JSON.stringify(error.response?.data) ||
        'An error occurred'
    );
  }
};

/**
 * Deletes a review by ID.
 * @param {string} id - Review identifier
 * @param {string} token - JWT authentication token
 * @returns {Promise<object>} Deletion response
 * @throws {Error} Deletion error message
 */
export const deleteReview = async (id, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.delete(`/reviews/${id}`);
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.title ||
        error.response?.data?.message ||
        error.response?.data?.detail ||
        JSON.stringify(error.response?.data) ||
        'An error occurred'
    );
  }
};

/**
 * Retrieves reviews with optional filters.
 * @param {object} filters - Filter options (listingId, guestProfileId)
 * @param {string} token - JWT authentication token
 * @returns {Promise<Array>} Array of review objects
 * @throws {Error} Fetch error message
 */
export const getReviews = async (filters = {}, token) => {
  try {
    const authClient = createAuthClient(token);
    const params = {};
    if (filters.listingId) params.listingId = filters.listingId;
    if (filters.guestProfileId) params.guestProfileId = filters.guestProfileId;

    const response = await authClient.get('/reviews', { params });
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
 * Retrieves a single review by ID.
 * @param {string} id - Review identifier
 * @param {string} token - JWT authentication token
 * @returns {Promise<object>} Review data
 * @throws {Error} Fetch error message
 */
export const getReview = async (id, token) => {
  try {
    const authClient = createAuthClient(token);
    const response = await authClient.get(`/reviews/${id}`);
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
