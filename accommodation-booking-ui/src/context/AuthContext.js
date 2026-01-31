/**
 * Authentication context for managing user session state across the application.
 * Provides access to authentication data and user information.
 */
import { createContext } from 'react';

const AuthContext = createContext(null);

export default AuthContext;
