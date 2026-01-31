/**
 * Reservation Page Component
 * Allows guests to create reservations by selecting check-in and check-out dates.
 * Displays listing details, available dates, and calculates total price.
 */
import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import {
  Box,
  Container,
  Card,
  CardContent,
  Typography,
  Button,
  CircularProgress,
  Alert,
  Grid,
} from '@mui/material';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { pl } from 'date-fns/locale';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import { toast } from 'react-toastify';
import { useListingsApi, useReservationsApi, useAuth } from '../../hooks';
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';
import { translateAccommodationType } from '../../utils';

const ReservationPage = () => {
  const navigate = useNavigate();
  const { listingId } = useParams();
  const { auth } = useAuth();
  const { getListing, getAvailableDates } = useListingsApi();
  const { createReservation } = useReservationsApi();

  const [listing, setListing] = useState(null);
  const [checkInDate, setCheckInDate] = useState(null);
  const [checkOutDate, setCheckOutDate] = useState(null);
  const [availableDates, setAvailableDates] = useState([]);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchListing = async () => {
      if (!auth?.token || !listingId) return;

      setLoading(true);
      const result = await getListing(listingId, auth.token);
      if (result.success) {
        setListing(result.data);
      } else {
        setError(result.error);
      }
      setLoading(false);
    };

    fetchListing();
  }, [listingId, auth?.token]);

  useEffect(() => {
    const fetchAvailableDates = async () => {
      if (!auth?.token || !listingId) return;

      const today = new Date();
      const result = await getAvailableDates(
        listingId,
        today.toISOString().split('T')[0],
        90,
        auth.token
      );

      if (result.success) {
        setAvailableDates(result.data.map((date) => new Date(date)));
      }
    };

    fetchAvailableDates();
  }, [listingId, auth?.token]);

  /**
   * Checks if a given date is available for booking.
   * @param {Date} date - Date to check
   * @returns {boolean} True if date is available
   */
  const isDateAvailable = (date) => {
    return availableDates.some(
      (availableDate) => availableDate.toDateString() === date.toDateString()
    );
  };

  /**
   * Determines if a date should be disabled in the DatePicker.
   * Disables past dates and unavailable dates.
   * @param {Date} date - Date to check
   * @returns {boolean} True if date should be disabled
   */
  const shouldDisableDate = (date) => {
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    if (date < today) {
      return true;
    }

    return !isDateAvailable(date);
  };

  /**
   * Calculates the number of nights and total price for the reservation.
   * @returns {object|null} Object with nights count and total price, or null if dates invalid
   */
  const calculateTotalPrice = () => {
    if (!checkInDate || !checkOutDate || !listing) return null;

    const nights = Math.ceil((checkOutDate - checkInDate) / (1000 * 60 * 60 * 24));
    if (nights <= 0) return null;

    return {
      nights,
      total: nights * listing.amountPerDay,
    };
  };

  const totalPrice = calculateTotalPrice();

  /**
   * Handles reservation creation with validation.
   */
  const handleCreateReservation = async () => {
    if (!checkInDate || !checkOutDate) {
      toast.error('Wybierz daty pobytu');
      return;
    }

    if (checkOutDate <= checkInDate) {
      toast.error('Data wyjazdu musi być późniejsza niż data przyjazdu');
      return;
    }

    setSubmitting(true);

    const formatDate = (date) => {
      const year = date.getFullYear();
      const month = String(date.getMonth() + 1).padStart(2, '0');
      const day = String(date.getDate()).padStart(2, '0');
      return `${year}-${month}-${day}`;
    };

    const result = await createReservation(
      {
        listingId: listingId,
        checkIn: formatDate(checkInDate),
        checkOut: formatDate(checkOutDate),
      },
      auth.token
    );

    setSubmitting(false);

    if (result.success) {
      toast.success('Rezerwacja została utworzona pomyślnie!');
      navigate('/listings');
    } else {
      toast.error(result.error || 'Nie udało się utworzyć rezerwacji');
    }
  };

  if (loading) {
    return (
      <Container maxWidth="md" sx={{ py: 4 }}>
        <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', py: 8 }}>
          <CircularProgress />
        </Box>
      </Container>
    );
  }

  if (error) {
    return (
      <Container maxWidth="md" sx={{ py: 4 }}>
        <Alert severity="error" sx={{ mb: 3 }}>
          {error}
        </Alert>
      </Container>
    );
  }

  if (!listing) {
    return (
      <Container maxWidth="md" sx={{ py: 4 }}>
        <Alert severity="info" sx={{ mb: 3 }}>
          Nie znaleziono ogłoszenia
        </Alert>
      </Container>
    );
  }

  return (
    <LocalizationProvider dateAdapter={AdapterDateFns} adapterLocale={pl}>
      <Container maxWidth="md" sx={{ py: 4 }}>
        <Typography variant="h4" sx={{ fontWeight: 'bold', color: DARK_GRAY, mb: 3 }}>
          Rezerwacja
        </Typography>

        <Card sx={{ mb: 3 }}>
          <CardContent>
            <Typography variant="h6" sx={{ fontWeight: 'bold', mb: 2 }}>
              {listing.title}
            </Typography>
            <Typography variant="body2" color="textSecondary" sx={{ mb: 1 }}>
              {translateAccommodationType(listing.accommodationType)}
            </Typography>
            <Typography variant="body2" color="textSecondary" sx={{ mb: 1 }}>
              📍 {listing.street} {listing.buildingNumber}, {listing.city}
            </Typography>
            <Typography variant="h6" sx={{ color: PRIMARY_BLUE, fontWeight: 'bold', mt: 2 }}>
              {listing.amountPerDay} {listing.currency}
              <Typography component="span" variant="body2" sx={{ color: 'textSecondary' }}>
                {' '}
                / noc
              </Typography>
            </Typography>
          </CardContent>
        </Card>

        <Card>
          <CardContent>
            <Typography variant="h6" sx={{ fontWeight: 'bold', mb: 3 }}>
              Wybierz termin pobytu
            </Typography>

            <Grid container spacing={3} sx={{ mb: 3 }}>
              <Grid item xs={12} sm={6}>
                <DatePicker
                  label="Data przyjazdu"
                  value={checkInDate}
                  onChange={(newValue) => setCheckInDate(newValue)}
                  shouldDisableDate={shouldDisableDate}
                  slotProps={{
                    textField: {
                      fullWidth: true,
                      required: true,
                    },
                  }}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <DatePicker
                  label="Data wyjazdu"
                  value={checkOutDate}
                  onChange={(newValue) => setCheckOutDate(newValue)}
                  shouldDisableDate={(date) => {
                    // Check-out date must be after check-in date
                    if (checkInDate && date <= checkInDate) {
                      return true;
                    }
                    return shouldDisableDate(date);
                  }}
                  slotProps={{
                    textField: {
                      fullWidth: true,
                      required: true,
                    },
                  }}
                />
              </Grid>
            </Grid>

            {totalPrice && (
              <Box
                sx={{
                  p: 2,
                  bgcolor: 'rgba(13, 110, 253, 0.05)',
                  borderRadius: 1,
                  mb: 3,
                }}
              >
                <Typography variant="body1" sx={{ mb: 1 }}>
                  Liczba nocy: <strong>{totalPrice.nights}</strong>
                </Typography>
                <Typography variant="h6" sx={{ color: PRIMARY_BLUE, fontWeight: 'bold' }}>
                  Całkowita cena: {totalPrice.total} {listing.currency}
                </Typography>
              </Box>
            )}

            <Box sx={{ display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
              <Button variant="outlined" onClick={() => navigate(-1)}>
                Anuluj
              </Button>
              <Button
                variant="contained"
                startIcon={<CheckCircleIcon />}
                disabled={!checkInDate || !checkOutDate || submitting}
                sx={{
                  backgroundColor: PRIMARY_BLUE,
                  '&:hover': {
                    backgroundColor: '#0a58ca',
                  },
                }}
                onClick={handleCreateReservation}
              >
                {submitting ? 'Tworzenie...' : 'Potwierdź rezerwację'}
              </Button>
            </Box>
          </CardContent>
        </Card>
      </Container>
    </LocalizationProvider>
  );
};

export default ReservationPage;
