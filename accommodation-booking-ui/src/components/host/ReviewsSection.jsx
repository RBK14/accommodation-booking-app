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
} from '@mui/material';
import { DARK_GRAY, PRIMARY_BLUE } from '../../assets/styles/colors';

const ReviewsSection = ({ reviews = [], showListingTitle = false, title = 'Opinie' }) => {
  const [page, setPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(5);

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

  return (
    <Card>
      <CardContent>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
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
                      <Box>
                        <Typography
                          variant="subtitle1"
                          sx={{ fontWeight: 'bold', color: DARK_GRAY }}
                        >
                          {review.guestName}
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
                      <Typography variant="caption" sx={{ color: 'textSecondary' }}>
                        {new Date(review.date).toLocaleDateString('pl-PL')}
                      </Typography>
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
  );
};

export default ReviewsSection;
