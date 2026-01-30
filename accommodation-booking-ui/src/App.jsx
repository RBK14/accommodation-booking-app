import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

// Layouts
import { MainLayout, AuthLayout, AdminLayout, HostLayout, GuestLayout } from './layouts';

// Router
import { ProtectedRoute } from './router';

// Pages
import {
  // Auth
  LoginPage,
  RegisterPage,
  // Guest
  HomePage,
  ListingsPage,
  GuestListingPage,
  ReservationPage,
  GuestAccountPage,
  GuestReservationsPage,
  GuestCreateReviewPage,
  // Host
  HostListingsPage,
  HostListingPage,
  HostEditListingPage,
  HostNewListingPage,
  HostReservationsPage,
  HostReviewPage,
  HostAccountPage,
  // Admin
  AdminUsersPage,
  AdminUserPage,
  AdminEditUserPage,
  AdminListingsPage,
  AdminListingPage,
  AdminEditListingPage,
  AdminAccountPage,
} from './pages';

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

        {/* GUEST - Panel Gościa */}
        <Route element={<ProtectedRoute allowedRoles={['Guest']} />}>
          <Route path="/guest" element={<GuestLayout />}>
            <Route index element={<Navigate to="/guest/account" replace />} />
            <Route path="account" element={<GuestAccountPage />} />
            <Route path="reservations" element={<GuestReservationsPage />} />
            <Route path="review/:reservationId" element={<GuestCreateReviewPage />} />
          </Route>
        </Route>

        {/* HOST */}
        <Route element={<ProtectedRoute allowedRoles={['Host']} />}>
          <Route path="/host" element={<HostLayout />}>
            <Route index element={<Navigate to="/host/account" replace />} />
            <Route path="account" element={<HostAccountPage />} />
            <Route path="listings" element={<HostListingsPage />} />
            <Route path="new-listing" element={<HostNewListingPage />} />
            <Route path="listing/:id" element={<HostListingPage />} />
            <Route path="listing/:id/edit" element={<HostEditListingPage />} />
            <Route path="reservations" element={<HostReservationsPage />} />
            <Route path="review" element={<HostReviewPage />} />
          </Route>
        </Route>

        {/* ADMIN */}
        <Route element={<ProtectedRoute allowedRoles={['Admin']} />}>
          <Route path="/admin" element={<AdminLayout />}>
            <Route index element={<Navigate to="/admin/account" replace />} />
            <Route path="account" element={<AdminAccountPage />} />
            <Route path="users" element={<AdminUsersPage />} />
            <Route path="user/:id" element={<AdminUserPage />} />
            <Route path="user/:id/edit" element={<AdminEditUserPage />} />
            <Route path="listings" element={<AdminListingsPage />} />
            <Route path="listing/:id" element={<AdminListingPage />} />
            <Route path="listing/:id/edit" element={<AdminEditListingPage />} />
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
