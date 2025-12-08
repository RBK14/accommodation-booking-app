import { useState } from 'react';
import * as reviewsApi from '../api/reviewsApi';

/**
 * Hook do obsługi API opinii
 */
export const useReviewsApi = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  /**
   * Utworzenie nowej opinii
   */
  const handleCreateReview = async (data, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await reviewsApi.createReview(data, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się utworzyć opinii';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Aktualizacja opinii
   */
  const handleUpdateReview = async (id, data, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await reviewsApi.updateReview(id, data, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się zaktualizować opinii';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Usunięcie opinii
   */
  const handleDeleteReview = async (id, token) => {
    setLoading(true);
    setError(null);

    try {
      await reviewsApi.deleteReview(id, token);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się usunąć opinii';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Pobranie listy opinii
   */
  const handleGetReviews = async (filters = {}, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await reviewsApi.getReviews(filters, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się pobrać opinii';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Pobranie szczegółów opinii
   */
  const handleGetReview = async (id, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await reviewsApi.getReview(id, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się pobrać opinii';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  return {
    // Stan
    loading,
    error,

    // Metody
    createReview: handleCreateReview,
    updateReview: handleUpdateReview,
    deleteReview: handleDeleteReview,
    getReviews: handleGetReviews,
    getReview: handleGetReview,

    // Pomocnicze
    clearError: () => setError(null),
  };
};
