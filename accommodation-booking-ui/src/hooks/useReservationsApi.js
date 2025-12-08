import { useState } from 'react';
import * as reservationsApi from '../api/reservationsApi';

/**
 * Hook do obsługi API rezerwacji
 */
export const useReservationsApi = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  /**
   * Utworzenie nowej rezerwacji
   */
  const handleCreateReservation = async (data, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await reservationsApi.createReservation(data, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się utworzyć rezerwacji';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Aktualizacja statusu rezerwacji
   */
  const handleUpdateReservationStatus = async (id, status, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await reservationsApi.updateReservationStatus(id, { status }, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się zaktualizować statusu rezerwacji';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Usunięcie rezerwacji
   */
  const handleDeleteReservation = async (id, token) => {
    setLoading(true);
    setError(null);

    try {
      await reservationsApi.deleteReservation(id, token);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się usunąć rezerwacji';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Pobranie listy rezerwacji
   */
  const handleGetReservations = async (filters = {}) => {
    setLoading(true);
    setError(null);

    try {
      const response = await reservationsApi.getReservations(filters);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się pobrać rezerwacji';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Pobranie szczegółów rezerwacji
   */
  const handleGetReservation = async (id) => {
    setLoading(true);
    setError(null);

    try {
      const response = await reservationsApi.getReservation(id);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się pobrać rezerwacji';
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
    createReservation: handleCreateReservation,
    updateReservationStatus: handleUpdateReservationStatus,
    deleteReservation: handleDeleteReservation,
    getReservations: handleGetReservations,
    getReservation: handleGetReservation,

    // Pomocnicze
    clearError: () => setError(null),
  };
};
