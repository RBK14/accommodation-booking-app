import {
  Box,
  Typography,
  TextField,
  Button,
  Card,
  CardContent,
  Stack,
  Container,
} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';

const RegisterPage = () => {
  const navigate = useNavigate();

  const handleRegister = () => {
    // TODO: Implementacja rejestracji
    console.log('Rejestracja');
  };

  return (
    <Container maxWidth="sm" sx={{ py: 5 }}>
      <Card>
        <CardContent>
          <Typography variant="h4" sx={{ mb: 3, fontWeight: 'bold', textAlign: 'center' }}>
            Rejestracja
          </Typography>

          <Stack spacing={2}>
            <TextField fullWidth label="Imię" variant="outlined" />

            <TextField fullWidth label="Nazwisko" variant="outlined" />

            <TextField fullWidth label="Email" type="email" variant="outlined" />

            <TextField fullWidth label="Hasło" type="password" variant="outlined" />

            <TextField fullWidth label="Powtórz hasło" type="password" variant="outlined" />

            <Stack direction="row" spacing={2} sx={{ mt: 2 }}>
              <Button
                fullWidth
                variant="contained"
                sx={{
                  backgroundColor: PRIMARY_BLUE,
                  '&:hover': {
                    backgroundColor: '#0a58ca',
                  },
                }}
                onClick={handleRegister}
              >
                Zarejestruj się
              </Button>
              <Button
                fullWidth
                variant="outlined"
                sx={{
                  borderColor: DARK_GRAY,
                  color: DARK_GRAY,
                  '&:hover': {
                    borderColor: DARK_GRAY,
                    backgroundColor: 'rgba(33, 37, 41, 0.04)',
                  },
                }}
                onClick={() => navigate('/login')}
              >
                Logowanie
              </Button>
            </Stack>

            <Typography variant="body2" sx={{ textAlign: 'center', color: 'textSecondary', mt: 2 }}>
              Masz już konto?{' '}
              <Typography
                component="span"
                sx={{
                  color: PRIMARY_BLUE,
                  cursor: 'pointer',
                  '&:hover': { textDecoration: 'underline' },
                }}
                onClick={() => navigate('/login')}
              >
                Zaloguj się
              </Typography>
            </Typography>
          </Stack>
        </CardContent>
      </Card>
    </Container>
  );
};

export default RegisterPage;
