import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Box, Container, Card, CardContent, Button, CircularProgress, Alert } from '@mui/material';
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';
import { ListingDetailsSection, ListingGallerySection } from '../../components/shared';
import { ReviewsSection } from '../../components/host';
import { useListingsApi, useReviewsApi } from '../../hooks';
import { useAuth } from '../../hooks';
import { PRIMARY_BLUE } from '../../assets/styles/colors';

const GuestListingPage = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const { auth } = useAuth();
  const { getListing, loading: listingLoading, error: listingError } = useListingsApi();
  const { getReviews, loading: reviewsLoading } = useReviewsApi();
  const [listing, setListing] = useState(null);
  const [images, setImages] = useState([]);
  const [reviews, setReviews] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      if (!auth?.token || !id) return;

      // Fetch listing data
      const listingResult = await getListing(id, auth.token);
      if (listingResult.success) {
        setListing(listingResult.data);
        if (listingResult.data.photos && listingResult.data.photos.length > 0) {
          setImages(listingResult.data.photos);
        }
      }

      // Fetch reviews for this listing
      const reviewsResult = await getReviews({ listingId: id }, auth.token);
      if (reviewsResult.success) {
        setReviews(reviewsResult.data);
      }
    };

    fetchData();
  }, [id, auth?.token]);

  const handleReservation = () => {
    navigate(`/reservation/${id}`);
  };

  if (listingLoading || reviewsLoading) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', py: 8 }}>
          <CircularProgress />
        </Box>
      </Container>
    );
  }

  if (listingError) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Alert severity="error" sx={{ mb: 3 }}>
          {listingError}
        </Alert>
      </Container>
    );
  }

  if (!listing) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Alert severity="info" sx={{ mb: 3 }}>
          Nie znaleziono ogłoszenia
        </Alert>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Box sx={{ display: 'flex', gap: 3, flexDirection: { xs: 'column', md: 'row' } }}>
            <ListingDetailsSection listing={listing} onEdit={null} onDelete={null} />
            <ListingGallerySection images={images} />
          </Box>

          {/* Przycisk rezerwacji */}
          <Box sx={{ mt: 3, display: 'flex', justifyContent: 'center' }}>
            <Button
              variant="contained"
              size="large"
              startIcon={<CalendarMonthIcon />}
              sx={{
                backgroundColor: PRIMARY_BLUE,
                px: 6,
                py: 1.5,
                fontSize: '1.1rem',
                '&:hover': {
                  backgroundColor: '#0a58ca',
                },
              }}
              onClick={handleReservation}
            >
              Zarezerwuj termin
            </Button>
          </Box>
        </CardContent>
      </Card>

      {/* Reviews section */}
      <ReviewsSection
        reviews={reviews}
        title="Opinie gości"
        showListingTitle={false}
        allowEdit={false} // Guest cannot edit other users' reviews
      />
    </Container>
  );
};

export default GuestListingPage;
