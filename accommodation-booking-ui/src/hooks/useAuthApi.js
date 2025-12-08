import { useState } from 'react';
import * as authApi from '../api/authApi';

/**
 * Hook do obsługi API autentykacji
 * Uwaga: NIE zarządza stanem autoryzacji - to robi AuthContext
 */
export const useAuthApi = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  /**
   * Logowanie użytkownika
   */
  const handleLogin = async (credentials) => {
    setLoading(true);
    setError(null);

    try {
      const response = await authApi.login(credentials);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się zalogować';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Rejestracja gościa
   */
  const handleRegisterGuest = async (data) => {
    setLoading(true);
    setError(null);

    try {
      await authApi.registerGuest(data);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się zarejestrować';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Rejestracja gospodarza
   */
  const handleRegisterHost = async (data) => {
    setLoading(true);
    setError(null);

    try {
      await authApi.registerHost(data);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się zarejestrować';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Rejestracja admina
   */
  const handleRegisterAdmin = async (data) => {
    setLoading(true);
    setError(null);

    try {
      await authApi.registerAdmin(data);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się zarejestrować';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Aktualizacja email
   */
  const handleUpdateEmail = async (userId, email, token) => {
    setLoading(true);
    setError(null);

    try {
      await authApi.updateEmail(userId, { email }, token);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się zaktualizować email';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Aktualizacja hasła
   */
  const handleUpdatePassword = async (userId, passwordData, token) => {
    setLoading(true);
    setError(null);

    try {
      await authApi.updatePassword(userId, passwordData, token);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się zaktualizować hasła';
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
    login: handleLogin,
    registerGuest: handleRegisterGuest,
    registerHost: handleRegisterHost,
    registerAdmin: handleRegisterAdmin,
    updateEmail: handleUpdateEmail,
    updatePassword: handleUpdatePassword,

    // Pomocnicze
    clearError: () => setError(null),
  };
};
