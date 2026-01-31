/**
 * Listings API Hook
 * Provides listing-related API operations with loading and error state management.
 */
import { useState } from 'react';
import * as listingsApi from '../api/listingsApi';

export const useListingsApi = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  /**
   * Creates a new listing.
   * @param {object} data - Listing data
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and data/error
   */
  const handleCreateListing = async (data, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await listingsApi.createListing(data, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Failed to create listing';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Updates an existing listing.
   * @param {string} id - Listing identifier
   * @param {object} data - Updated listing data
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and data/error
   */
  const handleUpdateListing = async (id, data, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await listingsApi.updateListing(id, data, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Failed to update listing';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Deletes a listing.
   * @param {string} id - Listing identifier
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and error if failed
   */
  const handleDeleteListing = async (id, token) => {
    setLoading(true);
    setError(null);

    try {
      await listingsApi.deleteListing(id, token);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Failed to delete listing';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Fetches listings with optional host filter.
   * @param {string|null} hostProfileId - Optional host profile ID
   * @returns {Promise<object>} Result with success status and data/error
   */
  const handleGetListings = async (hostProfileId = null) => {
    setLoading(true);
    setError(null);

    try {
      const response = await listingsApi.getListings(hostProfileId);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Failed to fetch listings';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Fetches a single listing by ID.
   * @param {string} id - Listing identifier
   * @returns {Promise<object>} Result with success status and data/error
   */
  const handleGetListing = async (id) => {
    setLoading(true);
    setError(null);

    try {
      const response = await listingsApi.getListing(id);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Failed to fetch listing';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Fetches available dates for a listing.
   * @param {string} id - Listing identifier
   * @param {string|null} from - Start date (ISO format)
   * @param {number|null} days - Number of days to check
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and data/error
   */
  const handleGetAvailableDates = async (id, from = null, days = null, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await listingsApi.getAvailableDates(id, from, days, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Failed to fetch available dates';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  return {
    loading,
    error,
    createListing: handleCreateListing,
    updateListing: handleUpdateListing,
    deleteListing: handleDeleteListing,
    getListings: handleGetListings,
    getListing: handleGetListing,
    getAvailableDates: handleGetAvailableDates,
    clearError: () => setError(null),
  };
};
