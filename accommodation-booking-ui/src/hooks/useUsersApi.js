/**
 * Users API Hook
 * Provides user-related API operations with loading and error state management.
 */
import { useState } from 'react';
import * as usersApi from '../api/usersApi';

export const useUsersApi = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  /**
   * Updates user's personal details.
   * @param {string} id - User identifier
   * @param {object} data - Personal details (firstName, lastName, phone)
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and error if failed
   */
  const handleUpdatePersonalDetails = async (id, data, token) => {
    setLoading(true);
    setError(null);

    try {
      await usersApi.updatePersonalDetails(id, data, token);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Failed to update personal details';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Deletes a guest user account.
   * @param {string} id - User identifier
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and error if failed
   */
  const handleDeleteGuest = async (id, token) => {
    setLoading(true);
    setError(null);

    try {
      await usersApi.deleteGuest(id, token);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Failed to delete account';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Deletes a host user account.
   * @param {string} id - User identifier
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and error if failed
   */
  const handleDeleteHost = async (id, token) => {
    setLoading(true);
    setError(null);

    try {
      await usersApi.deleteHost(id, token);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Failed to delete account';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Deletes an admin user account.
   * @param {string} id - User identifier
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and error if failed
   */
  const handleDeleteAdmin = async (id, token) => {
    setLoading(true);
    setError(null);

    try {
      await usersApi.deleteAdmin(id, token);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Failed to delete account';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Fetches users list with optional role filter.
   * @param {string|null} userRole - Optional role filter
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and data/error
   */
  const handleGetUsers = async (userRole = null, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await usersApi.getUsers(userRole, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Failed to fetch users';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Fetches a single user by ID.
   * @param {string} id - User identifier
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and data/error
   */
  const handleGetUser = async (id, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await usersApi.getUser(id, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Failed to fetch user';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  return {
    loading,
    error,
    updatePersonalDetails: handleUpdatePersonalDetails,
    deleteGuest: handleDeleteGuest,
    deleteHost: handleDeleteHost,
    deleteAdmin: handleDeleteAdmin,
    getUsers: handleGetUsers,
    getUser: handleGetUser,
    clearError: () => setError(null),
  };
};
