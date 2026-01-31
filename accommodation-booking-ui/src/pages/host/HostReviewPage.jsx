import { useState, useEffect } from 'react';
import { Box, CircularProgress, Alert } from '@mui/material';
import { ReviewsSection } from '../../components/host';
import { useAuth, useReviewsApi, useListingsApi } from '../../hooks';

const HostReviewPage = () => {
  const { auth, userData } = useAuth();
  const { getReviews, loading: reviewsLoading, error: reviewsError } = useReviewsApi();
  const { getListings, loading: listingsLoading } = useListingsApi();
  const [reviews, setReviews] = useState([]);

  useEffect(() => {
    const fetchReviews = async () => {
      if (!auth?.token || !userData?.profileId) return;

      const listingsResult = await getListings(userData.profileId, auth.token);

      if (listingsResult.success && listingsResult.data.length > 0) {
        const allReviews = [];

        for (const listing of listingsResult.data) {
          const reviewsResult = await getReviews({ listingId: listing.id }, auth.token);
          if (reviewsResult.success) {
            allReviews.push(...reviewsResult.data);
          }
        }

        setReviews(allReviews);
      } else {
        setReviews([]);
      }
    };

    fetchReviews();
  }, [auth?.token, userData?.profileId]);

  if (reviewsLoading || listingsLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
        <CircularProgress />
      </Box>
    );
  }

  if (reviewsError) {
    return (
      <Box sx={{ p: 3 }}>
        <Alert severity="error">{reviewsError}</Alert>
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
