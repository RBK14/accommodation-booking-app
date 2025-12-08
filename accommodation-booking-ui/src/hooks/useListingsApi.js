import { useState } from 'react';
import * as listingsApi from '../api/listingsApi';

/**
 * Hook do obsługi API ofert
 */
export const useListingsApi = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  /**
   * Utworzenie nowej oferty
   */
  const handleCreateListing = async (data, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await listingsApi.createListing(data, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się utworzyć oferty';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Aktualizacja oferty
   */
  const handleUpdateListing = async (id, data, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await listingsApi.updateListing(id, data, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się zaktualizować oferty';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Usunięcie oferty
   */
  const handleDeleteListing = async (id, token) => {
    setLoading(true);
    setError(null);

    try {
      await listingsApi.deleteListing(id, token);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się usunąć oferty';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Pobranie listy ofert
   */
  const handleGetListings = async (hostProfileId = null) => {
    setLoading(true);
    setError(null);

    try {
      const response = await listingsApi.getListings(hostProfileId);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się pobrać ofert';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Pobranie szczegółów oferty
   */
  const handleGetListing = async (id) => {
    setLoading(true);
    setError(null);

    try {
      const response = await listingsApi.getListing(id);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się pobrać oferty';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Pobranie dostępnych dat dla oferty
   */
  const handleGetAvailableDates = async (id, from = null, days = null) => {
    setLoading(true);
    setError(null);

    try {
      const response = await listingsApi.getAvailableDates(id, from, days);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się pobrać dostępnych dat';
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
    createListing: handleCreateListing,
    updateListing: handleUpdateListing,
    deleteListing: handleDeleteListing,
    getListings: handleGetListings,
    getListing: handleGetListing,
    getAvailableDates: handleGetAvailableDates,

    // Pomocnicze
    clearError: () => setError(null),
  };
};
