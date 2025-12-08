import { useState } from 'react';
import {
  Card,
  CardContent,
  Typography,
  Box,
  Stack,
  Pagination,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  Chip,
} from '@mui/material';
import { DARK_GRAY, PRIMARY_BLUE } from '../../assets/styles/colors';

const ReservationsSection = ({
  reservations = [],
  showListingTitle = false,
  title = 'Rezerwacje',
}) => {
  const [page, setPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(5);

  const handlePageChange = (event, value) => {
    setPage(value);
  };

  const handleItemsPerPageChange = (event) => {
    setItemsPerPage(event.target.value);
    setPage(1);
  };

  const getStatusColor = (status) => {
    switch (status.toLowerCase()) {
      case 'confirmed':
      case 'potwierdzona':
        return 'success';
      case 'pending':
      case 'oczekuj�ca':
        return 'warning';
      case 'cancelled':
      case 'anulowana':
        return 'error';
      case 'completed':
      case 'zako�czona':
        return 'info';
      default:
        return 'default';
    }
  };

  const totalPages = Math.ceil(reservations.length / itemsPerPage);
  const startIndex = (page - 1) * itemsPerPage;
  const endIndex = startIndex + itemsPerPage;
  const currentReservations = reservations.slice(startIndex, endIndex);

  return (
    <Card sx={{ mb: 3 }}>
      <CardContent>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
          <Typography variant="h6" sx={{ fontWeight: 'bold' }}>
            {title} ({reservations.length})
          </Typography>

          {reservations.length > 0 && (
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

        {reservations.length === 0 ? (
          <Typography variant="body2" sx={{ color: 'textSecondary' }}>
            Brak rezerwacji
          </Typography>
        ) : (
          <>
            <Stack spacing={2}>
              {currentReservations.map((reservation) => (
                <Card
                  key={reservation.id}
                  variant="outlined"
                  sx={{
                    backgroundColor: '#f8f9fa',
                    '&:hover': {
                      boxShadow: '0 2px 8px rgba(0, 0, 0, 0.1)',
                    },
                  }}
                >
                  <CardContent>
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
                      <Box>
                        {showListingTitle && reservation.title && (
                          <Typography
                            variant="subtitle1"
                            sx={{ fontWeight: 'bold', color: PRIMARY_BLUE, mb: 0.5 }}
                          >
                            {reservation.title}
                          </Typography>
                        )}
                        <Typography variant="body2" sx={{ color: DARK_GRAY }}>
                          {reservation.street} {reservation.buildingNumber}
                        </Typography>
                        <Typography variant="body2" sx={{ color: DARK_GRAY }}>
                          {reservation.postalCode} {reservation.city}, {reservation.country}
                        </Typography>
                      </Box>
                      <Chip
                        label={reservation.status}
                        color={getStatusColor(reservation.status)}
                        size="small"
                      />
                    </Box>

                    <Box sx={{ display: 'flex', gap: 4, mb: 1 }}>
                      <Box>
                        <Typography
                          variant="caption"
                          sx={{ color: 'textSecondary', fontWeight: 'bold' }}
                        >
                          Zameldowanie
                        </Typography>
                        <Typography variant="body2" sx={{ color: DARK_GRAY }}>
                          {new Date(reservation.checkIn).toLocaleDateString('pl-PL')}
                        </Typography>
                      </Box>
                      <Box>
                        <Typography
                          variant="caption"
                          sx={{ color: 'textSecondary', fontWeight: 'bold' }}
                        >
                          Wymeldowanie
                        </Typography>
                        <Typography variant="body2" sx={{ color: DARK_GRAY }}>
                          {new Date(reservation.checkOut).toLocaleDateString('pl-PL')}
                        </Typography>
                      </Box>
                    </Box>

                    <Box
                      sx={{
                        display: 'flex',
                        justifyContent: 'space-between',
                        mt: 2,
                        pt: 2,
                        borderTop: '1px solid #dee2e6',
                      }}
                    >
                      <Typography variant="body2" sx={{ color: 'textSecondary' }}>
                        Cena za noc:{' '}
                        <strong>
                          {reservation.pricePerDay} {reservation.currency}
                        </strong>
                      </Typography>
                      <Typography variant="body2" sx={{ color: PRIMARY_BLUE, fontWeight: 'bold' }}>
                        Łącznie: {reservation.totalPrice} {reservation.currency}
                      </Typography>
                    </Box>
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

export default ReservationsSection;
