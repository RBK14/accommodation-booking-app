import { Outlet, BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import MainLayout from './layouts/MainLayout';
import AuthLayout from './layouts/AuthLayout';
import AdminLayout from './layouts/AdminLayout';
import HostLayout from './layouts/HostLayout';

import HomePage from './pages/guest/HomePage';
import LoginPage from './pages/auth/LoginPage';
import AdminDashboard from './pages/admin/AdminDashboard';
import HostDashboard from './pages/host/HostDashboard';
import ProtectedRoute from './router/ProtectedRoute';

function App() {
  return (
    <Router>
      <Routes>
        {/* PUBLIC */}
        <Route element={<MainLayout />}>
          <Route path="/" element={<HomePage />} />
        </Route>

        {/* AUTH */}
        <Route element={<AuthLayout />}>
          <Route path="/login" element={<LoginPage />} />
        </Route>

        {/* HOST */}
        <Route element={<ProtectedRoute allowedRoles={['Host']} />}>
          <Route path="/host" element={<HostLayout />}>
            <Route index element={<HostDashboard />} />
          </Route>
        </Route>

        {/* ADMIN */}
        <Route element={<ProtectedRoute allowedRoles={['Admin']} />}>
          <Route path="/admin" element={<AdminLayout />}>
            <Route index element={<AdminDashboard />} />
          </Route>
        </Route>

        {/* 403 */}
        <Route path="/unauthorized" element={<div>403 - Brak dostępu</div>} />

        {/* 404 */}
        <Route path="*" element={<div>404 - Nie znaleziono</div>} />
      </Routes>
    </Router>
  );
}

export default App;
