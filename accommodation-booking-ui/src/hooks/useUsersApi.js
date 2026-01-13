import { useState } from 'react';
import * as usersApi from '../api/usersApi';

/**
 * Hook do obsługi API użytkowników
 */
export const useUsersApi = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  /**
   * Aktualizacja danych osobowych użytkownika
   */
  const handleUpdatePersonalDetails = async (id, data, token) => {
    setLoading(true);
    setError(null);

    try {
      await usersApi.updatePersonalDetails(id, data, token);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się zaktualizować danych osobowych';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Usunięcie konta gościa
   */
  const handleDeleteGuest = async (id, token) => {
    setLoading(true);
    setError(null);

    try {
      await usersApi.deleteGuest(id, token);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się usunąć konta';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Usunięcie konta gospodarza
   */
  const handleDeleteHost = async (id, token) => {
    setLoading(true);
    setError(null);

    try {
      await usersApi.deleteHost(id, token);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się usunąć konta';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Usunięcie konta admina
   */
  const handleDeleteAdmin = async (id, token) => {
    setLoading(true);
    setError(null);

    try {
      await usersApi.deleteAdmin(id, token);
      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się usunąć konta';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Pobranie listy użytkowników - WYMAGA TOKENU
   */
  const handleGetUsers = async (userRole = null, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await usersApi.getUsers(userRole, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się pobrać użytkowników';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    } finally {
      setLoading(false);
    }
  };

  /**
   * Pobranie szczegółów użytkownika - WYMAGA TOKENU
   */
  const handleGetUser = async (id, token) => {
    setLoading(true);
    setError(null);

    try {
      const response = await usersApi.getUser(id, token);
      return { success: true, data: response };
    } catch (err) {
      const errorMessage = err.message || 'Nie udało się pobrać użytkownika';
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
    updatePersonalDetails: handleUpdatePersonalDetails,
    deleteGuest: handleDeleteGuest,
    deleteHost: handleDeleteHost,
    deleteAdmin: handleDeleteAdmin,
    getUsers: handleGetUsers,
    getUser: handleGetUser,

    // Pomocnicze
    clearError: () => setError(null),
  };
};
