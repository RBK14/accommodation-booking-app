import { Box, Container, Typography } from '@mui/material';
import { SearchBar } from '../../components/guest';
import { PRIMARY_BLUE } from '../../assets/styles/colors';

const HomePage = () => {
  return (
    <Box
      sx={{
        minHeight: '70vh',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
        padding: 3,
      }}
    >
      <Container maxWidth="lg">
        <Box sx={{ textAlign: 'center', mb: 6 }}>
          <Typography
            variant="h2"
            sx={{
              fontWeight: 'bold',
              color: 'white',
              mb: 2,
              textShadow: '2px 2px 4px rgba(0,0,0,0.2)',
            }}
          >
            Znajdź swoje idealne miejsce
          </Typography>
          <Typography
            variant="h5"
            sx={{
              color: 'rgba(255, 255, 255, 0.95)',
              fontWeight: 300,
              mb: 1,
            }}
          >
            Odkryj wyjątkowe noclegi w najpiękniejszych zakątkach świata
          </Typography>
          <Typography
            variant="body1"
            sx={{
              color: 'rgba(255, 255, 255, 0.85)',
              maxWidth: '600px',
              margin: '0 auto',
            }}
          >
            Tysiące zweryfikowanych ofert, sprawdzonych gospodarzy i niezapomnianych wrażeń
          </Typography>
        </Box>

        <SearchBar />
      </Container>
    </Box>
  );
};

export default HomePage;
