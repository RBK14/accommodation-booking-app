import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import {
  Box,
  Card,
  CardContent,
  CircularProgress,
  Alert,
  IconButton,
  Menu,
  MenuItem,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogContentText,
  DialogActions,
  Button,
} from '@mui/material';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import CancelIcon from '@mui/icons-material/Cancel';
import EventBusyIcon from '@mui/icons-material/EventBusy';
import { ReservationsSection, ReviewsSection } from '../../components/host';
import { ListingDetailsSection, ListingGallerySection } from '../../components/shared';
import { useAuth, useListingsApi, useReservationsApi, useReviewsApi } from '../../hooks';
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';
import { toast } from 'react-toastify';

const AdminListingPage = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const { auth } = useAuth();
  const {
    getListing,
    deleteListing,
    loading: listingLoading,
    error: listingError,
  } = useListingsApi();
  const {
    getReservations,
    updateReservationStatus,
    loading: reservationsLoading,
  } = useReservationsApi();
  const { getReviews, loading: reviewsLoading } = useReviewsApi();

  const [listing, setListing] = useState(null);
  const [images, setImages] = useState([]);
  const [reviews, setReviews] = useState([]);
  const [reservations, setReservations] = useState([]);
  const [anchorEl, setAnchorEl] = useState(null);
  const [selectedReservation, setSelectedReservation] = useState(null);
  const [confirmDialogOpen, setConfirmDialogOpen] = useState(false);
  const [newStatus, setNewStatus] = useState(null);

  const fetchData = async () => {
    if (!auth?.token || !id) return;

    const listingResult = await getListing(id, auth.token);
    if (listingResult.success) {
      setListing(listingResult.data);
      if (listingResult.data.photos && listingResult.data.photos.length > 0) {
        setImages(listingResult.data.photos);
      } else {
        setImages([]);
      }
    }

    const reservationsResult = await getReservations({ listingId: id }, auth.token);
    if (reservationsResult.success) {
      setReservations(reservationsResult.data);
    }

    const reviewsResult = await getReviews({ listingId: id }, auth.token);
    if (reviewsResult.success) {
      setReviews(reviewsResult.data);
    }
  };

  useEffect(() => {
    fetchData();
  }, [id, auth?.token]);

  const handleEdit = () => {
    navigate(`/admin/listing/${id}/edit`);
  };

  const handleDelete = async () => {
    if (!window.confirm('Czy na pewno chcesz usunac to ogloszenie?')) {
      return;
    }

    const result = await deleteListing(id, auth.token);
    if (result.success) {
      toast.success('Ogloszenie zostalo usuniete');
      navigate('/admin/listings');
    } else {
      toast.error(result.error || 'Nie udalo sie usunac ogloszenia');
    }
  };

  const handleReviewUpdated = () => {
    fetchData();
  };

  const handleReviewDeleted = () => {
    fetchData();
  };

  const canChangeStatus = (reservation) => {
    const status = reservation.status?.toLowerCase();
    return status !== 'completed' && status !== 'cancelled' && status !== 'noshow';
  };

  const canMarkAsNoShow = (reservation) => {
    return reservation.status?.toLowerCase() === 'inprogress';
  };

  const handleMenuOpen = (event, reservation) => {
    setAnchorEl(event.currentTarget);
    setSelectedReservation(reservation);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleStatusChangeClick = (status) => {
    setNewStatus(status);
    setConfirmDialogOpen(true);
    handleMenuClose();
  };

  const handleConfirmStatusChange = async () => {
    if (!selectedReservation || !newStatus || !auth?.token) return;

    setConfirmDialogOpen(false);

    const result = await updateReservationStatus(selectedReservation.id, newStatus, auth.token);

    if (result.success) {
      toast.success('Status rezerwacji zostal zaktualizowany');
      fetchData();
    } else {
      toast.error(result.error || 'Nie udalo sie zaktualizowac statusu');
    }

    setSelectedReservation(null);
    setNewStatus(null);
  };

  const handleCancelDialog = () => {
    setConfirmDialogOpen(false);
    setSelectedReservation(null);
    setNewStatus(null);
  };

  const getNewStatusLabel = (status) => {
    switch (status) {
      case 'Cancelled':
        return 'Anulowana';
      case 'NoShow':
        return 'Nieodbyta';
      default:
        return status;
    }
  };

  const reservationsWithActions = reservations.map((reservation) => {
    return {
      ...reservation,
      statusAction: canChangeStatus(reservation) ? (
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

  if (listingLoading || reservationsLoading || reviewsLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', py: 10 }}>
        <CircularProgress />
      </Box>
    );
  }

  if (listingError) {
    return (
      <Box sx={{ p: 3 }}>
        <Alert severity="error">{listingError}</Alert>
      </Box>
    );
  }

  if (!listing) {
    return (
      <Box sx={{ p: 3 }}>
        <Alert severity="info">Nie znaleziono ogloszenia</Alert>
      </Box>
    );
  }

  return (
    <>
      <Box sx={{ p: 3 }}>
        <Card sx={{ mb: 3 }}>
          <CardContent>
            <Box sx={{ display: 'flex', gap: 3 }}>
              <ListingDetailsSection
                listing={listing}
                onEdit={handleEdit}
                onDelete={handleDelete}
              />
              <ListingGallerySection images={images} />
            </Box>
          </CardContent>
        </Card>

        <ReservationsSection
          reservations={reservationsWithActions}
          title="Rezerwacje tego ogloszenia"
          showListingTitle={false}
          showAdminDetails={true}
        />
        <ReviewsSection
          reviews={reviews}
          title="Opinie"
          showListingTitle={false}
          allowEdit={true}
          onReviewUpdated={handleReviewUpdated}
          onReviewDeleted={handleReviewDeleted}
        />
      </Box>

      {}
      <Menu anchorEl={anchorEl} open={Boolean(anchorEl)} onClose={handleMenuClose}>
        <MenuItem onClick={() => handleStatusChangeClick('Cancelled')}>
          <CancelIcon sx={{ mr: 1, color: '#dc3545' }} />
          Anuluj rezerwacje
        </MenuItem>
        {}
        {selectedReservation && canMarkAsNoShow(selectedReservation) && (
          <MenuItem onClick={() => handleStatusChangeClick('NoShow')}>
            <EventBusyIcon sx={{ mr: 1, color: '#ffc107' }} />
            Oznacz jako nieodbyta
          </MenuItem>
        )}
      </Menu>

      {}
      <Dialog open={confirmDialogOpen} onClose={handleCancelDialog}>
        <DialogTitle>Potwierdz zmiane statusu</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Czy na pewno chcesz zmienic status rezerwacji na "{getNewStatusLabel(newStatus)}"?
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCancelDialog} sx={{ color: DARK_GRAY }}>
            Anuluj
          </Button>
          <Button
            onClick={handleConfirmStatusChange}
            sx={{
              color: PRIMARY_BLUE,
              '&:hover': {
                backgroundColor: 'rgba(13, 110, 253, 0.04)',
              },
            }}
            autoFocus
          >
            Potwierdz
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};

export default AdminListingPage;
