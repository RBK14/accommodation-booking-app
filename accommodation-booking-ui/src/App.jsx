import { Outlet, BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import MainLayout from './layouts/MainLayout';
import AuthLayout from './layouts/AuthLayout';
import AdminLayout from './layouts/AdminLayout';
import HostLayout from './layouts/HostLayout';

import ProtectedRoute from './router/ProtectedRoute';

import HomePage from './pages/guest/HomePage';
import ListingsPage from './pages/guest/ListingsPage';
import GuestListingPage from './pages/guest/GuestListingPage';
import ReservationPage from './pages/guest/ReservationPage';
import LoginPage from './pages/auth/LoginPage';
import RegisterPage from './pages/auth/RegisterPage';
import AdminUsersPage from './pages/admin/AdminUsersPage';
import AdminListingsPage from './pages/admin/AdminListingsPage';
import AdminListingPage from './pages/admin/AdminListingPage';
import AdminEditListingPage from './pages/admin/AdminEditListingPage';
import HostListingsPage from './pages/host/HostListingsPage';
import HostListingPage from './pages/host/HostListingPage';
import HostEditListingPage from './pages/host/HostEditListingPage';
import HostNewListingPage from './pages/host/HostNewListingPage';
import HostReservationsPage from './pages/host/HostReservationsPage';
import HostReviewPage from './pages/host/HostReviewPage';
import HostAccountPage from './pages/host/HostAccountPage';
import AccountPage from './pages/guest/AccountPage';
import AdminAccountPage from './pages/admin/AdminAccountPage';

function App() {
  return (
    <Router>
      <ToastContainer
        position="top-right"
        autoClose={3000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
      />
      <Routes>
        {/* PUBLIC */}
        <Route element={<MainLayout />}>
          <Route path="/" element={<HomePage />} />
          <Route path="/listings" element={<ListingsPage />} />
          <Route path="/listing/:id" element={<GuestListingPage />} />
          <Route path="/reservation/:listingId" element={<ReservationPage />} />
        </Route>

        {/* AUTH */}
        <Route element={<AuthLayout />}>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
        </Route>

        {/* GUEST */}
        <Route element={<ProtectedRoute allowedRoles={['Guest']} />}>
          <Route element={<MainLayout />}>
            <Route path="/account" element={<AccountPage />} />
          </Route>
        </Route>

        {/* HOST */}
        <Route element={<ProtectedRoute allowedRoles={['Host']} />}>
          <Route path="/host" element={<HostLayout />}>
            <Route index element={<HostListingsPage />} />
            <Route path="listings" element={<HostListingsPage />} />
            <Route path="new-listing" element={<HostNewListingPage />} />
            <Route path="listing/:id" element={<HostListingPage />} />
            <Route path="listing/:id/edit" element={<HostEditListingPage />} />
            <Route path="reservations" element={<HostReservationsPage />} />
            <Route path="review" element={<HostReviewPage />} />
            <Route path="account" element={<HostAccountPage />} />
          </Route>
        </Route>

        {/* ADMIN */}
        <Route element={<ProtectedRoute allowedRoles={['Admin']} />}>
          <Route path="/admin" element={<AdminLayout />}>
            <Route index element={<AdminUsersPage />} />
            <Route path="users" element={<AdminUsersPage />} />
            <Route path="listings" element={<AdminListingsPage />} />
            <Route path="listing/:id" element={<AdminListingPage />} />
            <Route path="listing/:id/edit" element={<AdminEditListingPage />} />
            <Route path="account" element={<AdminAccountPage />} />
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
