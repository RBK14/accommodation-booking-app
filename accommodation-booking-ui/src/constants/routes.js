// Definicje wszystkich ścieżek w aplikacji
// Ułatwia utrzymanie spójności i refaktoryzację

export const ROUTES = {
  // Public routes
  HOME: '/',
  LISTINGS: '/listings',
  LISTING_DETAILS: '/listing/:id',
  RESERVATION: '/reservation/:listingId',

  // Auth routes
  LOGIN: '/login',
  REGISTER: '/register',

  // Guest routes
  GUEST: {
    ROOT: '/guest',
    ACCOUNT: '/guest/account',
    RESERVATIONS: '/guest/reservations',
    CREATE_REVIEW: '/guest/review/:reservationId',
  },

  // Host routes
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

  // Admin routes
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

  // Error routes
  UNAUTHORIZED: '/unauthorized',
};

// Helper functions do generowania ścieżek z parametrami
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
