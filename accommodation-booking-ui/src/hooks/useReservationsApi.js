/**
 * Reservations API Hook
 * Provides reservation-related API operations with loading and error state management.
 */
import { useState } from 'react';
import * as reservationsApi from '../api/reservationsApi';

export const useReservationsApi = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  /**
   * Creates a new reservation.
   * @param {object} data - Reservation data (listingId, checkIn, checkOut)
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and data/error
   */
  const handleCreateReservation = async (data, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await reservationsApi.createReservation(data, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Failed to create reservation';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Updates reservation status.
   * @param {string} id - Reservation identifier
   * @param {string} status - New status
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and data/error
   */
  const handleUpdateReservationStatus = async (id, status, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await reservationsApi.updateReservationStatus(id, status, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Failed to update reservation status';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Deletes a reservation.
   * @param {string} id - Reservation identifier
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and error if failed
   */
  const handleDeleteReservation = async (id, token) => {
    setLoading(true);
    setError(null);

    try {
      await reservationsApi.deleteReservation(id, token);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Failed to delete reservation';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Fetches reservations with optional filters.
   * @param {object} filters - Filter options
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and data/error
   */
  const handleGetReservations = async (filters = {}, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await reservationsApi.getReservations(filters, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Failed to fetch reservations';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Fetches a single reservation by ID.
   * @param {string} id - Reservation identifier
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and data/error
   */
  const handleGetReservation = async (id, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await reservationsApi.getReservation(id, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Failed to fetch reservation';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  return {
    loading,
    error,
    createReservation: handleCreateReservation,
    updateReservationStatus: handleUpdateReservationStatus,
    deleteReservation: handleDeleteReservation,
    getReservations: handleGetReservations,
    getReservation: handleGetReservation,
    clearError: () => setError(null),
  };
};
