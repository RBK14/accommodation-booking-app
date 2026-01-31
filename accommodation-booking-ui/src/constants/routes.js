/**
 * Application Route Definitions
 * Centralized route constants for consistent navigation throughout the application.
 * Use these constants instead of hardcoded strings to prevent typos and enable refactoring.
 */

export const ROUTES = {
  HOME: '/',
  LISTINGS: '/listings',
  LISTING_DETAILS: '/listing/:id',
  RESERVATION: '/reservation/:listingId',

  LOGIN: '/login',
  REGISTER: '/register',

  GUEST: {
    ROOT: '/guest',
    ACCOUNT: '/guest/account',
    RESERVATIONS: '/guest/reservations',
    CREATE_REVIEW: '/guest/review/:reservationId',
  },

  HOST: {
    ROOT: '/host',
    ACCOUNT: '/host/account',
    LISTINGS: '/host/listings',
    NEW_LISTING: '/host/new-listing',
    LISTING: '/host/listing/:id',
    EDIT_LISTING: '/host/listing/:id/edit',
    RESERVATIONS: '/host/reservations',
    REVIEW: '/host/review',
  },

  ADMIN: {
    ROOT: '/admin',
    ACCOUNT: '/admin/account',
    USERS: '/admin/users',
    USER: '/admin/user/:id',
    EDIT_USER: '/admin/user/:id/edit',
    LISTINGS: '/admin/listings',
    LISTING: '/admin/listing/:id',
    EDIT_LISTING: '/admin/listing/:id/edit',
  },

  UNAUTHORIZED: '/unauthorized',
};

/**
 * Path generator functions for routes with dynamic parameters.
 * Use these helpers to generate URLs with actual values instead of placeholders.
 */
export const generatePath = {
  listingDetails: (id) => `/listing/${id}`,
  reservation: (listingId) => `/reservation/${listingId}`,

  guest: {
    createReview: (reservationId) => `/guest/review/${reservationId}`,
  },

  host: {
    listing: (id) => `/host/listing/${id}`,
    editListing: (id) => `/host/listing/${id}/edit`,
  },

  admin: {
    user: (id) => `/admin/user/${id}`,
    editUser: (id) => `/admin/user/${id}/edit`,
    listing: (id) => `/admin/listing/${id}`,
    editListing: (id) => `/admin/listing/${id}/edit`,
  },
};
