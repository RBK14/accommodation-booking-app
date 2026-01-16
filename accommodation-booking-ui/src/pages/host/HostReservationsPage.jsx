import { useState, useEffect } from 'react';
import { Box, CircularProgress, Alert, Button, Menu, MenuItem, Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions, IconButton } from '@mui/material';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import CancelIcon from '@mui/icons-material/Cancel';
import EventBusyIcon from '@mui/icons-material/EventBusy';
import { ReservationsSection } from '../../components/host';
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';
import { useAuth, useReservationsApi } from '../../hooks';
import { toast } from 'react-toastify';

const HostReservationsPage = () => {
  const { auth, userData } = useAuth();
  const { getReservations, updateReservationStatus, loading, error } = useReservationsApi();
  const [reservations, setReservations] = useState([]);
  const [anchorEl, setAnchorEl] = useState(null);
  const [selectedReservation, setSelectedReservation] = useState(null);
  const [confirmDialogOpen, setConfirmDialogOpen] = useState(false);
  const [newStatus, setNewStatus] = useState(null);

  const fetchReservations = async () => {
    if (!userData?.profileId || !auth?.token) return;

    const result = await getReservations({ hostProfileId: userData.profileId }, auth.token);
    if (result.success) {
      setReservations(result.data);
    }
  };

  useEffect(() => {
    fetchReservations();
  }, [userData?.profileId, auth?.token]);

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
    if (!selectedReservation || !newStatus || !auth?.token) {
          return;
    }

    if (!selectedReservation.id) {
        toast.error('Błąd: Brak ID rezerwacji');
        return;
    }

    setConfirmDialogOpen(false);

    const result = await updateReservationStatus(selectedReservation.id, newStatus, auth.token);

    if (result.success) {
      toast.success('Status rezerwacji został zaktualizowany');
      fetchReservations();
    } else {
      toast.error(result.error || 'Nie udało się zaktualizować statusu');
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
          title="Wszystkie rezerwacje"
        />
      </Box>

      {/* Menu zmiany statusu */}
      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={handleMenuClose}
      >
        <MenuItem onClick={() => handleStatusChangeClick('Cancelled')}>
          <CancelIcon sx={{ mr: 1, color: '#dc3545' }} />
          Anuluj rezerwację
        </MenuItem>
        {/* NoShow tylko gdy rezerwacja jest InProgress */}
        {selectedReservation && canMarkAsNoShow(selectedReservation) && (
          <MenuItem onClick={() => handleStatusChangeClick('NoShow')}>
            <EventBusyIcon sx={{ mr: 1, color: '#ffc107' }} />
            Oznacz jako nieodbyta
          </MenuItem>
        )}
      </Menu>

      {/* Dialog potwierdzenia */}
      <Dialog
        open={confirmDialogOpen}
        onClose={handleCancelDialog}
      >
        <DialogTitle>Potwierdź zmianę statusu</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Czy na pewno chcesz zmienić status rezerwacji na "{getNewStatusLabel(newStatus)}"?
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCancelDialog} sx={{ color: DARK_GRAY }}>
            Anuluj
          </Button>
          <Button
            onClick={handleConfirmStatusChange}
            sx={{
              color: newStatus === 'Cancelled' ? '#dc3545' : '#ffc107',
              '&:hover': {
                backgroundColor: newStatus === 'Cancelled' 
                  ? 'rgba(220, 53, 69, 0.04)' 
                  : 'rgba(255, 193, 7, 0.04)',
              },
            }}
            autoFocus
          >
            Potwierdź
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};

export default HostReservationsPage;