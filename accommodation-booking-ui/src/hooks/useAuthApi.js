/**
 * Authentication API Hook
 * Provides authentication-related API operations with loading and error state management.
 */
import { useState } from 'react';
import * as authApi from '../api/authApi';

export const useAuthApi = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  /**
   * Handles user login.
   * @param {object} credentials - User credentials (email, password)
   * @returns {Promise<object>} Result with success status and data/error
   */
  const handleLogin = async (credentials) => {
    setLoading(true);
    setError(null);

    try {
      const response = await authApi.login(credentials);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Login failed';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Handles guest user registration.
   * @param {object} data - Registration data
   * @returns {Promise<object>} Result with success status and error if failed
   */
  const handleRegisterGuest = async (data) => {
    setLoading(true);
    setError(null);

    try {
      await authApi.registerGuest(data);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Registration failed';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Handles host user registration.
   * @param {object} data - Registration data
   * @returns {Promise<object>} Result with success status and error if failed
   */
  const handleRegisterHost = async (data) => {
    setLoading(true);
    setError(null);

    try {
      await authApi.registerHost(data);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Registration failed';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Handles admin user registration.
   * @param {object} data - Registration data
   * @returns {Promise<object>} Result with success status and error if failed
   */
  const handleRegisterAdmin = async (data) => {
    setLoading(true);
    setError(null);

    try {
      await authApi.registerAdmin(data);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Registration failed';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Handles email update.
   * @param {string} userId - User identifier
   * @param {string} email - New email address
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and error if failed
   */
  const handleUpdateEmail = async (userId, email, token) => {
    setLoading(true);
    setError(null);

    try {
      await authApi.updateEmail(userId, { email }, token);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Email update failed';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Handles password update.
   * @param {string} userId - User identifier
   * @param {object} passwordData - Password data (currentPassword, newPassword)
   * @param {string} token - JWT token
   * @returns {Promise<object>} Result with success status and error if failed
   */
  const handleUpdatePassword = async (userId, passwordData, token) => {
    setLoading(true);
    setError(null);

    try {
      await authApi.updatePassword(userId, passwordData, token);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Password update failed';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  return {
    loading,
    error,
    login: handleLogin,
    registerGuest: handleRegisterGuest,
    registerHost: handleRegisterHost,
    registerAdmin: handleRegisterAdmin,
    updateEmail: handleUpdateEmail,
    updatePassword: handleUpdatePassword,
    clearError: () => setError(null),
  };
};
