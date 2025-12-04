import { createContext, useState, useEffect } from 'react';

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [auth, setAuth] = useState(null);

  const login = (role) => {
    const fakeAuth = {
      firstName: 'John',
      lastName: 'Doe',
      role: role,
    };
    setAuth(fakeAuth);
    localStorage.setItem('auth', JSON.stringify(fakeAuth));
  };

  const logout = () => {
    setAuth(null);
    localStorage.removeItem('auth');
  };

  useEffect(() => {
    const storedAuth = localStorage.getItem('auth');
    if (storedAuth) {
      setAuth(JSON.parse(storedAuth));
    }
  }, []);

  return <AuthContext.Provider value={{ auth, login, logout }}>{children}</AuthContext.Provider>;
};

export default AuthContext;
