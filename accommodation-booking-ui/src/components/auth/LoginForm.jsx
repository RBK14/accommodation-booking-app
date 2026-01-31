import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuthApi } from '../../hooks';
import {
  Box,
  TextField,
  Button,
  Typography,
  Alert,
  Paper,
  IconButton,
  InputAdornment,
} from '@mui/material';
import { Login as LoginIcon, Visibility, VisibilityOff } from '@mui/icons-material';
import { PRIMARY_BLUE } from '../../assets/styles/colors';

const LoginForm = ({ onSuccess }) => {
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const { login, loading, error } = useAuthApi();

  const handleSubmit = async (e) => {
    e.preventDefault();

    const result = await login({ email, password });

    if (result.success) {
      onSuccess(result.data);
    }
  };

  const handleClickShowPassword = () => {
    setShowPassword(!showPassword);
  };

  const handleMouseDownPassword = (event) => {
    event.preventDefault();
  };

  return (
    <Box
      sx={{
        maxWidth: 400,
        margin: '0 auto',
        padding: 3,
      }}
    >
      <Paper elevation={3} sx={{ padding: 4 }}>
        <Typography variant="h4" component="h2" align="center" gutterBottom>
          Logowanie
        </Typography>

        <Box component="form" onSubmit={handleSubmit} noValidate>
          <TextField
            label="Email"
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            fullWidth
            margin="normal"
            autoComplete="email"
            autoFocus
            disabled={loading}
          />

          <TextField
            label="Hasło"
            type={showPassword ? 'text' : 'password'}
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            fullWidth
            margin="normal"
            autoComplete="current-password"
            disabled={loading}
            InputProps={{
              endAdornment: (
                <InputAdornment position="end">
                  <IconButton
                    aria-label="toggle password visibility"
                    onClick={handleClickShowPassword}
                    onMouseDown={handleMouseDownPassword}
                    edge="end"
                    disabled={loading}
                  >
                    {showPassword ? <VisibilityOff /> : <Visibility />}
                  </IconButton>
                </InputAdornment>
              ),
            }}
          />

          {error && (
            <Alert severity="error" sx={{ mt: 2 }}>
              {error}
            </Alert>
          )}

          <Button
            type="submit"
            variant="contained"
            fullWidth
            disabled={loading}
            startIcon={<LoginIcon />}
            sx={{
              mt: 3,
              mb: 2,
              py: 1.5,
              backgroundColor: PRIMARY_BLUE,
              '&:hover': {
                backgroundColor: '#0a58ca',
              },
            }}
          >
            {loading ? 'Logowanie...' : 'Zaloguj się'}
          </Button>

          <Typography variant="body2" sx={{ textAlign: 'center', color: 'textSecondary', mt: 2 }}>
            Nie masz jeszcze konta?{' '}
            <Typography
              component="span"
              sx={{
                color: PRIMARY_BLUE,
                cursor: 'pointer',
                fontWeight: 'bold',
                '&:hover': { textDecoration: 'underline' },
              }}
              onClick={() => navigate('/register')}
            >
              Zarejestruj się
            </Typography>
          </Typography>
        </Box>
      </Paper>
    </Box>
  );
};

export default LoginForm;
