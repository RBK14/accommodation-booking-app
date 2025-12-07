import { useState } from 'react';
import { useAuthApi } from '../hooks';
import { Box, TextField, Button, Typography, Alert, Paper } from '@mui/material';
import { Login as LoginIcon } from '@mui/icons-material';

const LoginForm = ({ onSuccess }) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const { login, loading, error } = useAuthApi();

  const handleSubmit = async (e) => {
    e.preventDefault();

    const result = await login({ email, password });

    if (result.success) {
      onSuccess(result.data);
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
          />

          <TextField
            label="Hasło"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            fullWidth
            margin="normal"
            autoComplete="current-password"
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
            sx={{ mt: 3, mb: 2, py: 1.5 }}
          >
            {loading ? 'Logowanie...' : 'Zaloguj się'}
          </Button>
        </Box>
      </Paper>
    </Box>
  );
};

export default LoginForm;
