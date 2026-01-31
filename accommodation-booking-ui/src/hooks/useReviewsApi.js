/**
 * Reviews API Hook
 * Provides review-related API operations with loading and error state management.
 */
import { useState } from 'react';
import * as reviewsApi from '../api/reviewsApi';

export const useReviewsApi = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  /**
   * Creates a new review.
   * @param {object} data - Review data (listingId, rating, comment)
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and data/error
   */
  const handleCreateReview = async (data, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await reviewsApi.createReview(data, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Failed to create review';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Updates an existing review.
   * @param {string} id - Review identifier
   * @param {object} data - Updated review data
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and data/error
   */
  const handleUpdateReview = async (id, data, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await reviewsApi.updateReview(id, data, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Failed to update review';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Deletes a review.
   * @param {string} id - Review identifier
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and error if failed
   */
  const handleDeleteReview = async (id, token) => {
    setLoading(true);
    setError(null);

    try {
      await reviewsApi.deleteReview(id, token);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Failed to delete review';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Fetches reviews with optional filters.
   * @param {object} filters - Filter options
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and data/error
   */
  const handleGetReviews = async (filters = {}, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await reviewsApi.getReviews(filters, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Failed to fetch reviews';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Fetches a single review by ID.
   * @param {string} id - Review identifier
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and data/error
   */
  const handleGetReview = async (id, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await reviewsApi.getReview(id, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Failed to fetch review';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  return {
    loading,
    error,
    createReview: handleCreateReview,
    updateReview: handleUpdateReview,
    deleteReview: handleDeleteReview,
    getReviews: handleGetReviews,
    getReview: handleGetReview,
    clearError: () => setError(null),
  };
};
