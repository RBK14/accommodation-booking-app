import { useState, useEffect } from 'react';
import AuthContext from './AuthContext';

// Funkcja do dekodowania JWT tokena
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

// Funkcja do wyciągania danych z claims tokena
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
  const [isLoading, setIsLoading] = useState(true); // NOWY STAN

  const login = (token, userId) => {
    // Wyciągnij dane użytkownika z tokena
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

  const logout = () => {
    setAuth(null);
    setUserData(null);
    sessionStorage.removeItem('auth');
  };

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

        // Wyciągnij dane użytkownika z tokena
        if (authData.token) {
          const user = extractUserDataFromToken(authData.token);
          setUserData(user);
        }
      } catch (error) {
        console.error('Błąd podczas ładowania danych autoryzacji:', error);
        sessionStorage.removeItem('auth');
      }
    }
    setIsLoading(false); // ZAKOŃCZ ŁADOWANIE
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
