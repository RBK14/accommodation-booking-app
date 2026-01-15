import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Box, Card, CardContent, CircularProgress, Alert } from '@mui/material';
import { ReservationsSection, ReviewsSection } from '../../components/host';
import { ListingDetailsSection, ListingGallerySection } from '../../components/shared';
import { useAuth, useListingsApi, useReservationsApi, useReviewsApi } from '../../hooks';

const HostListingPage = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const { auth } = useAuth();
  const {
    getListing,
    deleteListing,
    loading: listingLoading,
    error: listingError,
  } = useListingsApi();
  const { getReservations, loading: reservationsLoading } = useReservationsApi();
  const { getReviews, loading: reviewsLoading } = useReviewsApi();

  const [listing, setListing] = useState(null);
  const [images, setImages] = useState([]);
  const [reviews, setReviews] = useState([]);
  const [reservations, setReservations] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      if (!auth?.token) return;

      const listingResult = await getListing(id, auth.token);
      if (listingResult.success) {
        setListing(listingResult.data);

        if (listingResult.data.photos && listingResult.data.photos.length > 0) {
          setImages(listingResult.data.photos);
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

    fetchData();
  }, [id, auth?.token]);

  const handleEdit = () => {
    navigate(`/host/listing/${id}/edit`, { state: { listing, images } });
  };

  const handleDelete = async () => {
    if (!window.confirm('Czy na pewno chcesz usunąć tę ofertę?')) return;

    const result = await deleteListing(id, auth.token);
    if (result.success) {
      navigate('/host/listings');
    }
  };

  if (listingLoading || reservationsLoading || reviewsLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
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
        <Alert severity="info">Nie znaleziono oferty</Alert>
      </Box>
    );
  }

  return (
    <Box sx={{ p: 3 }}>
      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Box sx={{ display: 'flex', gap: 3 }}>
            <ListingDetailsSection listing={listing} onEdit={handleEdit} onDelete={handleDelete} />
            <ListingGallerySection images={images} />
          </Box>
        </CardContent>
      </Card>

      <ReservationsSection reservations={reservations} />
      <ReviewsSection reviews={reviews} />
    </Box>
  );
};

export default HostListingPage;
