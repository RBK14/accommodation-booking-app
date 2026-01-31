/**
 * Authentication Provider Component
 * Manages user authentication state, JWT token handling, and session persistence.
 * Provides login, logout, and user data management functionality.
 */
import { useState, useEffect } from 'react';
import AuthContext from './AuthContext';

/**
 * Decodes a JWT token and extracts its payload.
 * @param {string} token - The JWT token to decode
 * @returns {object|null} The decoded token payload or null if invalid
 */
const decodeToken = (token) => {
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );
    return JSON.parse(jsonPayload);
  } catch (error) {
    return null;
  }
};

/**
 * Extracts user data from JWT token claims.
 * Maps standard and Microsoft-specific claim types to user properties.
 * @param {string} token - The JWT token containing user claims
 * @returns {object|null} User data object or null if token is invalid
 */
const extractUserDataFromToken = (token) => {
  const claims = decodeToken(token);
  if (!claims) return null;

  return {
    id:
      claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || claims.sub,
    email:
      claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || claims.email,
    firstName: claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname'] || '',
    lastName: claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname'] || '',
    phone: claims['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone'] || '',
    role: claims['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || claims.role,
    profileId: claims.ProfileId || null,
  };
};

export const AuthProvider = ({ children }) => {
  const [auth, setAuth] = useState(null);
  const [userData, setUserData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  /**
   * Authenticates user and stores session data.
   * @param {string} token - JWT authentication token
   * @param {string} userId - User identifier
   */
  const login = (token, userId) => {
    const user = extractUserDataFromToken(token);

    const authData = {
      id: userId,
      token: token,
      role: user?.role,
    };
    setAuth(authData);
    sessionStorage.setItem('auth', JSON.stringify(authData));
    setUserData(user);
  };

  /**
   * Clears authentication state and removes session data.
   */
  const logout = () => {
    setAuth(null);
    setUserData(null);
    sessionStorage.removeItem('auth');
  };

  /**
   * Updates user data in the context state.
   * @param {object} newData - Partial user data to merge with existing data
   */
  const updateUserData = (newData) => {
    setUserData((prev) => ({
      ...prev,
      ...newData,
    }));
  };

  useEffect(() => {
    const storedAuth = sessionStorage.getItem('auth');
    if (storedAuth) {
      try {
        const authData = JSON.parse(storedAuth);
        setAuth(authData);

        if (authData.token) {
          const user = extractUserDataFromToken(authData.token);
          setUserData(user);
        }
      } catch (error) {
        console.error('Error loading authentication data:', error);
        sessionStorage.removeItem('auth');
      }
    }
    setIsLoading(false);
  }, []);

  return (
    <AuthContext.Provider
      value={{ auth, userData, isLoading, login, logout, updateUserData, decodeToken }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export default AuthProvider;
