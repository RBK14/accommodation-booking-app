import { useState, useEffect } from 'react';
import { Box, CircularProgress, Alert } from '@mui/material';
import { ReviewsSection } from '../../components/host';
import { useAuth, useReviewsApi } from '../../hooks';

const HostReviewPage = () => {
  const { auth } = useAuth();
  const { getReviews, loading, error } = useReviewsApi();
  const [reviews, setReviews] = useState([]);

  useEffect(() => {
    const fetchReviews = async () => {
      if (!auth?.token) return;

      // Pobierz wszystkie opinie (bez filtrowania - backend zwróci opinie dla hosta)
      const result = await getReviews({}, auth.token);
      if (result.success) {
        setReviews(result.data);
      }
    };

    fetchReviews();
  }, [auth?.token]);

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
    <Box sx={{ p: 3 }}>
      <ReviewsSection reviews={reviews} showListingTitle={true} title="Wszystkie opinie" />
    </Box>
  );
};

export default HostReviewPage;
