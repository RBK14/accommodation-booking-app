import { useLocation, Navigate, Outlet } from 'react-router-dom';
import useAuth from '../hooks/useAuth';

const ProtectedRoute = ({ allowedRoles }) => {
  const { auth } = useAuth();
  const location = useLocation();

  if (!auth /*?.accessToken*/) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  if (!allowedRoles?.includes(auth.role)) {
    return <Navigate to="/unauthorized" state={{ from: location }} replace />;
  }

  return <Outlet />;
};

export default ProtectedRoute;
