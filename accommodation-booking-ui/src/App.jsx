import { Outlet, BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import MainLayout from './layouts/MainLayout';
import AuthLayout from './layouts/AuthLayout';
import AdminLayout from './layouts/AdminLayout';
import HostLayout from './layouts/HostLayout';

import ProtectedRoute from './router/ProtectedRoute';

import HomePage from './pages/guest/HomePage';
import LoginPage from './pages/auth/LoginPage';
import RegisterPage from './pages/auth/RegisterPage';
import AdminDashboard from './pages/admin/AdminDashboard';
import HostOffersPage from './pages/host/HostOffersPage';
import HostOfferPage from './pages/host/HostOfferPage';
import HostEditOfferPage from './pages/host/HostEditOfferPage';
import HostNewOfferPage from './pages/host/HostNewOfferPage';
import HostReservationsPage from './pages/host/HostReservationsPage';
import HostReviewPage from './pages/host/HostReviewPage';
import HostAccountPage from './pages/host/HostAccountPage';

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
          <Route path="/register" element={<RegisterPage />} />
        </Route>

        {/* HOST */}
        <Route element={<ProtectedRoute allowedRoles={['Host']} />}>
          <Route path="/host" element={<HostLayout />}>
            <Route index element={<HostOffersPage />} />
            <Route path="new-offer" element={<HostNewOfferPage />} />
            <Route path="offer/:id" element={<HostOfferPage />} />
            <Route path="offer/:id/edit" element={<HostEditOfferPage />} />
            <Route path="reservations" element={<HostReservationsPage />} />
            <Route path="review" element={<HostReviewPage />} />
            <Route path="account" element={<HostAccountPage />} />
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
