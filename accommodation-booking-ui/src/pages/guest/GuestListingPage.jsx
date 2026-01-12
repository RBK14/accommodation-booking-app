import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Box, Container, Card, CardContent, Button, CircularProgress, Alert } from '@mui/material';
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';
import { ListingDetailsSection, ListingGallerySection } from '../../components/shared';
import { useListingsApi } from '../../hooks';
import { useAuth } from '../../hooks';
import { PRIMARY_BLUE } from '../../assets/styles/colors';

const GuestListingPage = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const { auth } = useAuth();
  const { getListing, loading, error } = useListingsApi();
  const [listing, setListing] = useState(null);
  const [images] = useState([]);

  useEffect(() => {
    const fetchListing = async () => {
      if (!auth?.token || !id) return;

      const result = await getListing(id, auth.token);
      if (result.success) {
        setListing(result.data);
      }
    };

    fetchListing();
  }, [id, auth?.token]);

  const handleReservation = () => {
    navigate(`/reservation/${id}`);
  };

  if (loading) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', py: 8 }}>
          <CircularProgress />
        </Box>
      </Container>
    );
  }

  if (error) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Alert severity="error" sx={{ mb: 3 }}>
          {error}
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
      <Card>
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
    </Container>
  );
};

export default GuestListingPage;
