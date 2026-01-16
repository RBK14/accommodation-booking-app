import { useState } from 'react';
import {
  Box,
  Typography,
  TextField,
  Button,
  Card,
  CardContent,
  Stack,
  Alert,
  CircularProgress,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  IconButton,
  InputAdornment,
  Paper,
} from '@mui/material';
import { Visibility, VisibilityOff } from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';
import { useAuthApi } from '../../hooks';
import { toast } from 'react-toastify';

const RegisterPage = () => {
  const navigate = useNavigate();
  const { registerGuest, registerHost, loading, error } = useAuthApi();
  
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
    phone: '',
    userRole: 'Guest',
  });

  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleClickShowPassword = () => {
    setShowPassword(!showPassword);
  };

  const handleClickShowConfirmPassword = () => {
    setShowConfirmPassword(!showConfirmPassword);
  };

  const handleMouseDownPassword = (event) => {
    event.preventDefault();
  };

  const handleRegister = async () => {
    if (formData.password !== formData.confirmPassword) {
      toast.error('Hasła nie są identyczne');
      return;
    }

    const registrationData = {
      email: formData.email.trim(),
      password: formData.password,
      firstName: formData.firstName.trim(),
      lastName: formData.lastName.trim(),
      phone: formData.phone.trim(),
    };

    let result;
    if (formData.userRole === 'Host') {
      result = await registerHost(registrationData);
    } else {
      result = await registerGuest(registrationData);
    }

    if (result.success) {
      toast.success('Rejestracja zakończona sukcesem! Możesz się teraz zalogować.');
      navigate('/login');
    } else {
      toast.error(result.error || 'Nie udało się zarejestrować');
    }
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
          Rejestracja
        </Typography>

        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}

        <Stack spacing={2}>
          <FormControl fullWidth>
            <InputLabel>Typ konta</InputLabel>
            <Select
              name="userRole"
              value={formData.userRole}
              onChange={handleChange}
              label="Typ konta"
              disabled={loading}
            >
              <MenuItem value="Guest">Gość</MenuItem>
              <MenuItem value="Host">Gospodarz</MenuItem>
            </Select>
          </FormControl>

          <TextField
            fullWidth
            label="Imię"
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
            variant="outlined"
            disabled={loading}
          />

          <TextField
            fullWidth
            label="Nazwisko"
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            variant="outlined"
            disabled={loading}
          />

          <TextField
            fullWidth
            label="Email"
            name="email"
            type="email"
            value={formData.email}
            onChange={handleChange}
            variant="outlined"
            disabled={loading}
          />

          <TextField
            fullWidth
            label="Telefon"
            name="phone"
            value={formData.phone}
            onChange={handleChange}
            variant="outlined"
            disabled={loading}
          />

          <TextField
            fullWidth
            label="Hasło"
            name="password"
            type={showPassword ? 'text' : 'password'}
            value={formData.password}
            onChange={handleChange}
            variant="outlined"
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

          <TextField
            fullWidth
            label="Powtórz hasło"
            name="confirmPassword"
            type={showConfirmPassword ? 'text' : 'password'}
            value={formData.confirmPassword}
            onChange={handleChange}
            variant="outlined"
            disabled={loading}
            InputProps={{
              endAdornment: (
                <InputAdornment position="end">
                  <IconButton
                    aria-label="toggle confirm password visibility"
                    onClick={handleClickShowConfirmPassword}
                    onMouseDown={handleMouseDownPassword}
                    edge="end"
                    disabled={loading}
                  >
                    {showConfirmPassword ? <VisibilityOff /> : <Visibility />}
                  </IconButton>
                </InputAdornment>
              ),
            }}
          />

          <Button
            fullWidth
            variant="contained"
            sx={{
              mt: 3,
              mb: 2,
              py: 1.5,
              backgroundColor: PRIMARY_BLUE,
              '&:hover': {
                backgroundColor: '#0a58ca',
              },
            }}
            onClick={handleRegister}
            disabled={loading}
            startIcon={loading ? <CircularProgress size={20} color="inherit" /> : null}
          >
            {loading ? 'Rejestracja...' : 'Zarejestruj się'}
          </Button>

          <Typography variant="body2" sx={{ textAlign: 'center', color: 'textSecondary', mt: 2 }}>
            Masz już konto?{' '}
            <Typography
              component="span"
              sx={{
                color: PRIMARY_BLUE,
                cursor: 'pointer',
                fontWeight: 'bold',
                '&:hover': { textDecoration: 'underline' },
              }}
              onClick={() => navigate('/login')}
            >
              Zaloguj się
            </Typography>
          </Typography>
        </Stack>
      </Paper>
    </Box>
  );
};

export default RegisterPage;
