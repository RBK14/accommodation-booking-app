import { useSearchParams } from 'react-router-dom';
import { Box, Container, Typography, Card, CardContent } from '@mui/material';
import { DARK_GRAY } from '../../assets/styles/colors';
import { SearchBar } from '../../components/guest';

const ListingsPage = () => {
  const [searchParams] = useSearchParams();

  const location = searchParams.get('location') || '';
  const guests = searchParams.get('guests') || '';

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Box sx={{ mb: 4 }}>
        <Typography variant="h4" sx={{ fontWeight: 'bold', color: DARK_GRAY, mb: 3 }}>
          Dostępne ogłoszenia
        </Typography>

        <Box sx={{ maxWidth: '800px' }}>
          <SearchBar initialLocation={location} initialGuests={guests} compact />
        </Box>
      </Box>

      <Card>
        <CardContent>
          <Typography variant="body1" sx={{ color: 'textSecondary', textAlign: 'center', py: 4 }}>
            Tutaj pojawi się lista ogłoszeń na podstawie parametrów wyszukiwania
          </Typography>
        </CardContent>
      </Card>
    </Container>
  );
};

export default ListingsPage;
