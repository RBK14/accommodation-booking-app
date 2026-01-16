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
 * Utworzenie nowej opinii (tylko dla Guest)
 * @param {Object} data - { listingId, rating, comment }
 * @param {string} token - Token autoryzacyjny
 */
export const createReview = async (data, token) => {
    try {
        const authClient = createAuthClient(token);
        const response = await authClient.post('/reviews', data);
        return response.data;
    } catch (error) {
        console.error('Create review error:', error.response?.data); // DEBUG
        throw new Error(
            error.response?.data?.title ||
            error.response?.data?.message ||
            error.response?.data?.detail ||
            JSON.stringify(error.response?.data) ||
            'Wystąpił błąd'
        );
    }
};

/**
 * Aktualizacja opinii (Admin i Guest)
 * @param {string} id - ID opinii
 * @param {Object} data - { comment, rating }
 * @param {string} token - Token autoryzacyjny
 */
export const updateReview = async (id, data, token) => {
    try {
        const authClient = createAuthClient(token);
        console.log('Update review API call:', { id, data }); // DEBUG
        const response = await authClient.post(`/reviews/${id}`, data);
        console.log('Update review response:', response.data); // DEBUG
        return response.data;
    } catch (error) {
        console.error('Update review error:', error.response?.data); // DEBUG
        console.error('Update review error status:', error.response?.status); // DEBUG
        throw new Error(
            error.response?.data?.title ||
            error.response?.data?.message ||
            error.response?.data?.detail ||
            JSON.stringify(error.response?.data) ||
            'Wystąpił błąd'
        );
    }
};

/**
 * Usunięcie opinii (Admin i Guest)
 * @param {string} id - ID opinii
 * @param {string} token - Token autoryzacyjny
 */
export const deleteReview = async (id, token) => {
    try {
        const authClient = createAuthClient(token);
        console.log('Delete review API call:', id); // DEBUG
        const response = await authClient.delete(`/reviews/${id}`);
        console.log('Delete review response:', response); // DEBUG
        return response.data;
    } catch (error) {
        console.error('Delete review error:', error.response?.data); // DEBUG
        console.error('Delete review error status:', error.response?.status); // DEBUG
        throw new Error(
            error.response?.data?.title ||
            error.response?.data?.message ||
            error.response?.data?.detail ||
            JSON.stringify(error.response?.data) ||
            'Wystąpił błąd'
        );
    }
};

/**
 * Pobranie listy opinii
 * @param {Object} filters - { listingId?, guestProfileId? }
 * @param {string} token - Token autoryzacyjny
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
        console.error('Get reviews error:', error.response?.data); // DEBUG
        throw new Error(
            error.response?.data?.title ||
            error.response?.data?.message ||
            error.response?.data?.detail ||
            'Wystąpił błąd'
        );
    }
};

/**
 * Pobranie szczegółów opinii
 * @param {string} id - ID opinii
 * @param {string} token - Token autoryzacyjny
 */
export const getReview = async (id, token) => {
    try {
        const authClient = createAuthClient(token);
        const response = await authClient.get(`/reviews/${id}`);
        return response.data;
    } catch (error) {
        console.error('Get review error:', error.response?.data); // DEBUG
        throw new Error(
            error.response?.data?.title ||
            error.response?.data?.message ||
            error.response?.data?.detail ||
            'Wystąpił błąd'
        );
    }
};