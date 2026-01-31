/**
 * Protected Route Component
 * Wraps routes that require authentication and role-based authorization.
 * Redirects to login if not authenticated, or to unauthorized page if role not allowed.
 */
import { useLocation, Navigate, Outlet } from 'react-router-dom';
import { Box, CircularProgress } from '@mui/material';
import { useAuth } from '../hooks';

/**
 * @param {object} props - Component props
 * @param {Array<string>} props.allowedRoles - List of roles allowed to access the route
 */
const ProtectedRoute = ({ allowedRoles }) => {
  const { auth, isLoading } = useAuth();
  const location = useLocation();

  if (isLoading) {
    return (
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          minHeight: '100vh',
        }}
      >
        <CircularProgress />
      </Box>
    );
  }

  if (!auth) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  if (!allowedRoles?.includes(auth.role)) {
    return <Navigate to="/unauthorized" state={{ from: location }} replace />;
  }

  return <Outlet />;
};

export default ProtectedRoute;
