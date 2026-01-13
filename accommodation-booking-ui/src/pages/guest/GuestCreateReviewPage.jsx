import { useState, useEffect } from 'react';
import { useNavigate, useParams, useLocation } from 'react-router-dom';
import {
  Box,
  Card,
  CardContent,
  Typography,
  TextField,
  Button,
  Stack,
  Rating,
  Divider,
  Alert,
  CircularProgress,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogContentText,
  DialogActions,
} from '@mui/material';
import StarIcon from '@mui/icons-material/Star';
import SaveIcon from '@mui/icons-material/Save';
import CancelIcon from '@mui/icons-material/Cancel';
import DeleteIcon from '@mui/icons-material/Delete';
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';
import { useAuth, useReviewsApi } from '../../hooks';
import { toast } from 'react-toastify';

const GuestCreateReviewPage = () => {
  const navigate = useNavigate();
  const { reservationId } = useParams();
  const location = useLocation();
  const { auth } = useAuth();
  const { createReview, updateReview, deleteReview, loading } = useReviewsApi();

  const [reservation, setReservation] = useState(null);
  const [existingReview, setExistingReview] = useState(null);
  const [isEditMode, setIsEditMode] = useState(false);
  const [formData, setFormData] = useState({
    rating: 0,
    comment: '',
  });
  const [isSaving, setIsSaving] = useState(false);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);

  useEffect(() => {
    // Pobierz dane rezerwacji i ewentualnie istniejącą opinię z location.state
    if (location.state?.reservation) {
      setReservation(location.state.reservation);
    }

    if (location.state?.existingReview) {
      const review = location.state.existingReview;
      setExistingReview(review);
      setIsEditMode(true);
      setFormData({
        rating: review.rating || 0,
        comment: review.comment || '',
      });
    }
  }, [location.state]);

  const handleRatingChange = (event, newValue) => {
    setFormData((prev) => ({
      ...prev,
      rating: newValue || 0,
    }));
  };

  const handleCommentChange = (e) => {
    setFormData((prev) => ({
      ...prev,
      comment: e.target.value,
    }));
  };

  const handleSubmit = async () => {
    // Walidacja
    if (formData.rating === 0) {
      toast.error('Proszę wybrać ocenę');
      return;
    }

    if (!formData.comment.trim()) {
      toast.error('Proszę dodać komentarz');
      return;
    }

    if (!reservation?.listingId) {
      toast.error('Brak informacji o ogłoszeniu');
      return;
    }

    setIsSaving(true);

    let result;

    if (isEditMode && existingReview) {
      // Aktualizuj istniejącą opinię
      const dataToUpdate = {
        rating: formData.rating,
        comment: formData.comment.trim(),
      };

      result = await updateReview(existingReview.id, dataToUpdate, auth.token);
    } else {
      // Utwórz nową opinię
      const dataToSend = {
        listingId: reservation.listingId,
        rating: formData.rating,
        comment: formData.comment.trim(),
      };

      result = await createReview(dataToSend, auth.token);
    }

    setIsSaving(false);

    if (result.success) {
      toast.success(isEditMode ? 'Opinia została zaktualizowana' : 'Opinia została dodana');
      navigate('/guest/reservations');
    } else {
      toast.error(result.error || `Nie udało się ${isEditMode ? 'zaktualizować' : 'dodać'} opinii`);
    }
  };

  const handleDeleteClick = () => {
    setDeleteDialogOpen(true);
  };

  const handleDeleteConfirm = async () => {
    if (!existingReview) return;

    setDeleteDialogOpen(false);
    setIsSaving(true);

    const result = await deleteReview(existingReview.id, auth.token);

    setIsSaving(false);

    if (result.success) {
      toast.success('Opinia została usunięta');
      navigate('/guest/reservations');
    } else {
      toast.error(result.error || 'Nie udało się usunąć opinii');
    }
  };

  const handleDeleteCancel = () => {
    setDeleteDialogOpen(false);
  };

  const handleCancel = () => {
    navigate('/guest/reservations');
  };

  if (!reservation) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', py: 10 }}>
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Box sx={{ p: 3 }}>
      <Card>
        <CardContent>
          <Typography variant="h5" sx={{ fontWeight: 'bold', color: DARK_GRAY, mb: 3 }}>
            {isEditMode ? 'Edytuj opinię' : 'Wystaw opinię'}
          </Typography>

          {/* Informacje o rezerwacji */}
          <Box sx={{ mb: 4, p: 2, backgroundColor: '#f8f9fa', borderRadius: 2 }}>
            <Typography variant="subtitle1" sx={{ fontWeight: 'bold', mb: 1 }}>
              {reservation.title}
            </Typography>
            <Typography variant="body2" color="textSecondary">
              📍 {reservation.street} {reservation.buildingNumber}, {reservation.postalCode}{' '}
              {reservation.city}
            </Typography>
            <Typography variant="body2" color="textSecondary" sx={{ mt: 1 }}>
              📅 {new Date(reservation.checkIn).toLocaleDateString('pl-PL')} -{' '}
              {new Date(reservation.checkOut).toLocaleDateString('pl-PL')}
            </Typography>
          </Box>

          <Divider sx={{ my: 3 }} />

          <Stack spacing={3} sx={{ maxWidth: '700px' }}>
            {/* Ocena gwiazdkowa */}
            <Box>
              <Typography variant="body2" sx={{ color: DARK_GRAY, fontWeight: 'bold', mb: 2 }}>
                Twoja ocena *
              </Typography>
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                <Rating
                  name="rating"
                  value={formData.rating}
                  onChange={handleRatingChange}
                  size="large"
                  emptyIcon={<StarIcon style={{ opacity: 0.3 }} fontSize="inherit" />}
                />
                <Typography variant="body1" sx={{ color: DARK_GRAY, fontWeight: 'bold' }}>
                  {formData.rating > 0 ? `${formData.rating}/5` : 'Wybierz ocenę'}
                </Typography>
              </Box>
            </Box>

            {/* Komentarz */}
            <Box>
              <Typography variant="body2" sx={{ color: DARK_GRAY, fontWeight: 'bold', mb: 2 }}>
                Twoja opinia *
              </Typography>
              <TextField
                fullWidth
                multiline
                rows={6}
                placeholder="Podziel się swoimi wrażeniami z pobytu..."
                value={formData.comment}
                onChange={handleCommentChange}
                variant="outlined"
                helperText={`${formData.comment.length} znaków`}
              />
            </Box>

            {/* Informacja */}
            <Alert severity="info" sx={{ mt: 2 }}>
              Twoja opinia pomoże innym gościom w podjęciu decyzji oraz pomoże właścicielowi
              poprawić jakość usług.
            </Alert>

            {/* Przyciski */}
            <Stack direction="row" spacing={2} sx={{ mt: 4 }}>
              <Button
                variant="contained"
                startIcon={<SaveIcon />}
                disabled={isSaving || loading}
                sx={{
                  backgroundColor: PRIMARY_BLUE,
                  '&:hover': {
                    backgroundColor: '#0a58ca',
                  },
                }}
                onClick={handleSubmit}
              >
                {isSaving ? 'Zapisywanie...' : isEditMode ? 'Zaktualizuj opinię' : 'Wystaw opinię'}
              </Button>
              
              {isEditMode && (
                <Button
                  variant="outlined"
                  startIcon={<DeleteIcon />}
                  disabled={isSaving || loading}
                  sx={{
                    borderColor: '#dc3545',
                    color: '#dc3545',
                    '&:hover': {
                      borderColor: '#bb2d3b',
                      backgroundColor: 'rgba(220, 53, 69, 0.04)',
                    },
                  }}
                  onClick={handleDeleteClick}
                >
                  Usuń opinię
                </Button>
              )}

              <Button
                variant="outlined"
                startIcon={<CancelIcon />}
                disabled={isSaving || loading}
                sx={{
                  borderColor: DARK_GRAY,
                  color: DARK_GRAY,
                  '&:hover': {
                    borderColor: DARK_GRAY,
                    backgroundColor: 'rgba(33, 37, 41, 0.04)',
                  },
                }}
                onClick={handleCancel}
              >
                Anuluj
              </Button>
            </Stack>
          </Stack>
        </CardContent>
      </Card>

      {/* Dialog potwierdzenia usunięcia */}
      <Dialog
        open={deleteDialogOpen}
        onClose={handleDeleteCancel}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
      >
        <DialogTitle id="alert-dialog-title">Usuń opinię</DialogTitle>
        <DialogContent>
          <DialogContentText id="alert-dialog-description">
            Czy na pewno chcesz usunąć tę opinię? Ta operacja jest nieodwracalna.
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleDeleteCancel} sx={{ color: DARK_GRAY }}>
            Anuluj
          </Button>
          <Button
            onClick={handleDeleteConfirm}
            sx={{
              color: '#dc3545',
              '&:hover': {
                backgroundColor: 'rgba(220, 53, 69, 0.04)',
              },
            }}
            autoFocus
          >
            Usuń
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default GuestCreateReviewPage;