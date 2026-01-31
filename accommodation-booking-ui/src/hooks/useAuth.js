/**
 * Custom hook for accessing authentication context.
 * Provides access to auth state, user data, and auth methods.
 * @returns {object} Authentication context value
 */
import { useContext } from 'react';
import { AuthContext } from '../context';

const useAuth = () => {
  return useContext(AuthContext);
};

export default useAuth;
