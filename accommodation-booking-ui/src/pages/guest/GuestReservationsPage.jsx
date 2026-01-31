import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  CircularProgress,
  Alert,
  Button,
  IconButton,
  Menu,
  MenuItem,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogContentText,
  DialogActions,
} from '@mui/material';
import RateReviewIcon from '@mui/icons-material/RateReview';
import EditIcon from '@mui/icons-material/Edit';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import CancelIcon from '@mui/icons-material/Cancel';
import { ReservationsSection } from '../../components/host';
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';
import { useAuth, useReservationsApi, useReviewsApi } from '../../hooks';
import { toast } from 'react-toastify';

const GuestReservationsPage = () => {
  const navigate = useNavigate();
  const { auth, userData } = useAuth();
  const { getReservations, updateReservationStatus, loading, error } = useReservationsApi();
  const { getReviews } = useReviewsApi();
  const [reservations, setReservations] = useState([]);
  const [reviews, setReviews] = useState([]);
  const [anchorEl, setAnchorEl] = useState(null);
  const [selectedReservation, setSelectedReservation] = useState(null);
  const [confirmDialogOpen, setConfirmDialogOpen] = useState(false);

  const fetchData = async () => {
    if (!userData?.profileId || !auth?.token) return;

    const reservationsResult = await getReservations(
      { guestProfileId: userData.profileId },
      auth.token
    );
    if (reservationsResult.success) {
      setReservations(reservationsResult.data);
    }

    const reviewsResult = await getReviews({ guestProfileId: userData.profileId }, auth.token);
    if (reviewsResult.success) {
      setReviews(reviewsResult.data);
    }
  };

  useEffect(() => {
    fetchData();
  }, [userData?.profileId, auth?.token]);

  const isCompleted = (status) => {
    return status?.toLowerCase() === 'completed';
  };

  const canLeaveReview = (reservation) => {
    const status = reservation.status?.toLowerCase();
    return status === 'completed';
  };

  const canCancelReservation = (reservation) => {
    const status = reservation.status?.toLowerCase();
    return status === 'accepted';
  };

  const hasReview = (listingId) => {
    return reviews.some((review) => review.listingId === listingId);
  };

  const getReviewForListing = (listingId) => {
    return reviews.find((review) => review.listingId === listingId);
  };

  const handleCreateOrEditReview = (reservationId, reservation) => {
    const existingReview = getReviewForListing(reservation.listingId);
    navigate(`/guest/review/${reservationId}`, {
      state: {
        reservation,
        existingReview: existingReview || null,
      },
    });
  };

  const handleMenuOpen = (event, reservation) => {
    setAnchorEl(event.currentTarget);
    setSelectedReservation(reservation);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleCancelClick = () => {
    setConfirmDialogOpen(true);
    handleMenuClose();
  };

  const handleConfirmCancel = async () => {
    if (!selectedReservation || !auth?.token) return;

    setConfirmDialogOpen(false);

    const result = await updateReservationStatus(selectedReservation.id, 'Cancelled', auth.token);

    if (result.success) {
      toast.success('Rezerwacja zostala anulowana');
      fetchData();
    } else {
      toast.error(result.error || 'Nie udalo sie anulowac rezerwacji');
    }

    setSelectedReservation(null);
  };

  const handleCancelDialog = () => {
    setConfirmDialogOpen(false);
    setSelectedReservation(null);
  };

  const reservationsWithActions = reservations.map((reservation) => {
    const hasExistingReview = hasReview(reservation.listingId);

    return {
      ...reservation,
      customAction: canLeaveReview(reservation) ? (
        <Button
          fullWidth
          variant={hasExistingReview ? 'outlined' : 'contained'}
          size="small"
          startIcon={hasExistingReview ? <EditIcon /> : <RateReviewIcon />}
          sx={{
            backgroundColor: hasExistingReview ? 'transparent' : PRIMARY_BLUE,
            borderColor: hasExistingReview ? PRIMARY_BLUE : 'transparent',
            color: hasExistingReview ? PRIMARY_BLUE : 'white',
            '&:hover': {
              backgroundColor: hasExistingReview ? 'rgba(13, 110, 253, 0.04)' : '#0a58ca',
              borderColor: PRIMARY_BLUE,
            },
          }}
          onClick={() => handleCreateOrEditReview(reservation.id, reservation)}
        >
          {hasExistingReview ? 'Edytuj opinie' : 'Wystaw opinie'}
        </Button>
      ) : null,
      statusAction: canCancelReservation(reservation) ? (
        <IconButton
          size="small"
          onClick={(e) => handleMenuOpen(e, reservation)}
          sx={{
            color: PRIMARY_BLUE,
            '&:hover': {
              backgroundColor: 'rgba(13, 110, 253, 0.08)',
            },
          }}
        >
          <MoreVertIcon fontSize="small" />
        </IconButton>
      ) : null,
    };
  });

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Box sx={{ p: 3 }}>
        <Alert severity="error">{error}</Alert>
      </Box>
    );
  }

  return (
    <>
      <Box sx={{ p: 3 }}>
        <ReservationsSection
          reservations={reservationsWithActions}
          showListingTitle={true}
          title="Moje rezerwacje"
        />
      </Box>

      {}
      <Menu anchorEl={anchorEl} open={Boolean(anchorEl)} onClose={handleMenuClose}>
        <MenuItem onClick={handleCancelClick}>
          <CancelIcon sx={{ mr: 1, color: '#dc3545' }} />
          Anuluj rezerwacje
        </MenuItem>
      </Menu>

      {}
      <Dialog open={confirmDialogOpen} onClose={handleCancelDialog}>
        <DialogTitle>Anuluj rezerwacje</DialogTitle>
        <DialogContent>
          <DialogContentText>Czy na pewno chcesz anulowac te rezerwacje?</DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCancelDialog} sx={{ color: DARK_GRAY }}>
            Nie
          </Button>
          <Button
            onClick={handleConfirmCancel}
            sx={{
              color: '#dc3545',
              '&:hover': {
                backgroundColor: 'rgba(220, 53, 69, 0.04)',
              },
            }}
            autoFocus
          >
            Tak, anuluj
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};

export default GuestReservationsPage;
