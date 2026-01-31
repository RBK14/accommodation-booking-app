/**
 * Reviews Section Component
 * Displays a paginated list of reviews with optional edit/delete functionality.
 * @param {Object[]} reviews - Array of review objects to display
 * @param {boolean} showListingTitle - Whether to show listing title in review cards
 * @param {string} title - Section title text
 * @param {boolean} allowEdit - Whether to allow editing and deleting reviews
 * @param {Function} onReviewUpdated - Callback triggered after review update
 * @param {Function} onReviewDeleted - Callback triggered after review deletion
 */
import { useState } from 'react';
import {
  Card,
  CardContent,
  Typography,
  Box,
  Rating,
  Stack,
  Pagination,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  Tooltip,
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import SaveIcon from '@mui/icons-material/Save';
import CancelIcon from '@mui/icons-material/Cancel';
import { DARK_GRAY, PRIMARY_BLUE } from '../../assets/styles/colors';
import { useAuth, useReviewsApi } from '../../hooks';
import { toast } from 'react-toastify';

const ReviewsSection = ({
  reviews = [],
  showListingTitle = false,
  title = 'Opinie',
  allowEdit = false,
  onReviewUpdated = null,
  onReviewDeleted = null,
}) => {
  const { auth } = useAuth();
  const { updateReview, deleteReview } = useReviewsApi();
  const [page, setPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(5);
  const [editDialogOpen, setEditDialogOpen] = useState(false);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [selectedReview, setSelectedReview] = useState(null);
  const [editFormData, setEditFormData] = useState({
    rating: 0,
    comment: '',
  });
  const [isSaving, setIsSaving] = useState(false);

  const handlePageChange = (event, value) => {
    setPage(value);
  };

  const handleItemsPerPageChange = (event) => {
    setItemsPerPage(event.target.value);
    setPage(1);
  };

  const totalPages = Math.ceil(reviews.length / itemsPerPage);
  const startIndex = (page - 1) * itemsPerPage;
  const endIndex = startIndex + itemsPerPage;
  const currentReviews = reviews.slice(startIndex, endIndex);

  const formatDate = (dateString) => {
    if (!dateString) return 'Brak daty';
    try {
      const date = new Date(dateString);
      if (isNaN(date.getTime())) {
        return 'Nieprawidlowa data';
      }
      return date.toLocaleDateString('pl-PL', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
      });
    } catch (error) {
      return 'Blad daty';
    }
  };

  const getGuestName = (review) => {
    if (review.guestFirstName && review.guestLastName) {
      return `${review.guestFirstName} ${review.guestLastName}`;
    }
    if (review.guestName) {
      return review.guestName;
    }
    return 'Gosc';
  };

  const handleEditClick = (review) => {
    setSelectedReview(review);
    setEditFormData({
      rating: review.rating || 0,
      comment: review.comment || '',
    });
    setEditDialogOpen(true);
  };

  const handleDeleteClick = (review) => {
    setSelectedReview(review);
    setDeleteDialogOpen(true);
  };

  const handleEditClose = () => {
    setEditDialogOpen(false);
    setSelectedReview(null);
    setEditFormData({ rating: 0, comment: '' });
  };

  const handleDeleteClose = () => {
    setDeleteDialogOpen(false);
    setSelectedReview(null);
  };

  const handleRatingChange = (event, newValue) => {
    setEditFormData((prev) => ({
      ...prev,
      rating: newValue || 0,
    }));
  };

  const handleCommentChange = (e) => {
    setEditFormData((prev) => ({
      ...prev,
      comment: e.target.value,
    }));
  };

  const handleSaveEdit = async () => {
    if (!selectedReview || !auth?.token) return;

    if (editFormData.rating === 0) {
      toast.error('Prosze wybrac ocene');
      return;
    }

    if (!editFormData.comment.trim()) {
      toast.error('Prosze dodac komentarz');
      return;
    }

    setIsSaving(true);

    const dataToUpdate = {
      rating: editFormData.rating,
      comment: editFormData.comment.trim(),
    };

    const result = await updateReview(selectedReview.id, dataToUpdate, auth.token);

    setIsSaving(false);

    if (result.success) {
      toast.success('Opinia zostala zaktualizowana');
      handleEditClose();
      if (onReviewUpdated) {
        onReviewUpdated();
      }
    } else {
      toast.error(result.error || 'Nie udalo sie zaktualizowac opinii');
    }
  };

  const handleConfirmDelete = async () => {
    if (!selectedReview || !auth?.token) return;

    setIsSaving(true);

    const result = await deleteReview(selectedReview.id, auth.token);

    setIsSaving(false);

    if (result.success) {
      toast.success('Opinia zostala usunieta');
      handleDeleteClose();
      if (onReviewDeleted) {
        onReviewDeleted();
      }
    } else {
      toast.error(result.error || 'Nie udalo sie usunac opinii');
    }
  };

  return (
    <>
      <Card>
        <CardContent>
          <Box
            sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}
          >
            <Typography variant="h6" sx={{ fontWeight: 'bold' }}>
              {title} ({reviews.length})
            </Typography>

            {reviews.length > 0 && (
              <FormControl size="small" sx={{ minWidth: 120 }}>
                <InputLabel>Na stronie</InputLabel>
                <Select value={itemsPerPage} onChange={handleItemsPerPageChange} label="Na stronie">
                  <MenuItem value={5}>5</MenuItem>
                  <MenuItem value={10}>10</MenuItem>
                  <MenuItem value={15}>15</MenuItem>
                  <MenuItem value={20}>20</MenuItem>
                </Select>
              </FormControl>
            )}
          </Box>

          {reviews.length === 0 ? (
            <Typography variant="body2" sx={{ color: 'textSecondary' }}>
              Brak opinii
            </Typography>
          ) : (
            <>
              <Stack spacing={2}>
                {currentReviews.map((review) => (
                  <Card
                    key={review.id}
                    variant="outlined"
                    sx={{
                      backgroundColor: '#f8f9fa',
                      '&:hover': {
                        boxShadow: '0 2px 8px rgba(0, 0, 0, 0.1)',
                      },
                    }}
                  >
                    <CardContent>
                      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 1 }}>
                        <Box sx={{ flex: 1 }}>
                          <Typography
                            variant="subtitle1"
                            sx={{ fontWeight: 'bold', color: DARK_GRAY }}
                          >
                            {getGuestName(review)}
                          </Typography>
                          {showListingTitle && review.listingTitle && (
                            <Typography
                              variant="caption"
                              sx={{ color: PRIMARY_BLUE, fontWeight: 'bold' }}
                            >
                              {review.listingTitle}
                            </Typography>
                          )}
                        </Box>

                        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                          <Typography variant="caption" sx={{ color: 'textSecondary' }}>
                            {formatDate(review.createdAt || review.updatedAt || review.date)}
                          </Typography>

                          {}
                          {allowEdit && (
                            <Box sx={{ display: 'flex', gap: 0.5 }}>
                              <Tooltip title="Edytuj opinie">
                                <IconButton
                                  size="small"
                                  sx={{ color: PRIMARY_BLUE }}
                                  onClick={() => handleEditClick(review)}
                                >
                                  <EditIcon fontSize="small" />
                                </IconButton>
                              </Tooltip>
                              <Tooltip title="Usun opinie">
                                <IconButton
                                  size="small"
                                  sx={{ color: '#dc3545' }}
                                  onClick={() => handleDeleteClick(review)}
                                >
                                  <DeleteIcon fontSize="small" />
                                </IconButton>
                              </Tooltip>
                            </Box>
                          )}
                        </Box>
                      </Box>

                      <Rating value={review.rating} readOnly size="small" sx={{ mb: 1 }} />

                      <Typography variant="body2" sx={{ color: DARK_GRAY }}>
                        {review.comment}
                      </Typography>
                    </CardContent>
                  </Card>
                ))}
              </Stack>

              {totalPages > 1 && (
                <Box sx={{ display: 'flex', justifyContent: 'center', mt: 3 }}>
                  <Pagination
                    count={totalPages}
                    page={page}
                    onChange={handlePageChange}
                    color="primary"
                    size="medium"
                  />
                </Box>
              )}
            </>
          )}
        </CardContent>
      </Card>

      {}
      <Dialog open={editDialogOpen} onClose={handleEditClose} maxWidth="sm" fullWidth>
        <DialogTitle>Edytuj opinie</DialogTitle>
        <DialogContent>
          <Stack spacing={3} sx={{ mt: 2 }}>
            {}
            <Box>
              <Typography variant="body2" sx={{ color: DARK_GRAY, fontWeight: 'bold', mb: 1 }}>
                Ocena *
              </Typography>
              <Rating value={editFormData.rating} onChange={handleRatingChange} size="large" />
            </Box>

            {}
            <Box>
              <Typography variant="body2" sx={{ color: DARK_GRAY, fontWeight: 'bold', mb: 1 }}>
                Komentarz *
              </Typography>
              <TextField
                fullWidth
                multiline
                rows={6}
                value={editFormData.comment}
                onChange={handleCommentChange}
                variant="outlined"
                helperText={`${editFormData.comment.length} znak�w`}
              />
            </Box>
          </Stack>
        </DialogContent>
        <DialogActions>
          <Button
            onClick={handleEditClose}
            disabled={isSaving}
            startIcon={<CancelIcon />}
            sx={{ color: DARK_GRAY }}
          >
            Anuluj
          </Button>
          <Button
            onClick={handleSaveEdit}
            disabled={isSaving}
            variant="contained"
            startIcon={<SaveIcon />}
            sx={{
              backgroundColor: PRIMARY_BLUE,
              '&:hover': {
                backgroundColor: '#0a58ca',
              },
            }}
          >
            {isSaving ? 'Zapisywanie...' : 'Zapisz'}
          </Button>
        </DialogActions>
      </Dialog>

      {}
      <Dialog open={deleteDialogOpen} onClose={handleDeleteClose}>
        <DialogTitle>Usun opinie</DialogTitle>
        <DialogContent>
          <Typography>
            Czy na pewno chcesz usunac te opinie? Ta operacja jest nieodwracalna.
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleDeleteClose} disabled={isSaving} sx={{ color: DARK_GRAY }}>
            Anuluj
          </Button>
          <Button
            onClick={handleConfirmDelete}
            disabled={isSaving}
            sx={{
              color: '#dc3545',
              '&:hover': {
                backgroundColor: 'rgba(220, 53, 69, 0.04)',
              },
            }}
          >
            {isSaving ? 'Usuwanie...' : 'Usun'}
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};

export default ReviewsSection;
