import { useLocation, Navigate, Outlet } from 'react-router-dom';
import { Box, CircularProgress } from '@mui/material';
import { useAuth } from '../hooks';

const ProtectedRoute = ({ allowedRoles }) => {
  const { auth, isLoading } = useAuth();
  const location = useLocation();

  // Czekaj na za�adowanie danych z sessionStorage
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

  // Je�li nie ma autoryzacji, przekieruj do logowania
  if (!auth) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  // Je�li u�ytkownik nie ma odpowiedniej roli, przekieruj do unauthorized
  if (!allowedRoles?.includes(auth.role)) {
    return <Navigate to="/unauthorized" state={{ from: location }} replace />;
  }

  return <Outlet />;
};

export default ProtectedRoute;
