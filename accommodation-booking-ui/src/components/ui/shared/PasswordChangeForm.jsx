import { useState } from 'react';
import { Box, Card, CardContent, TextField, Button, Typography, Stack, Alert } from '@mui/material';
import SaveIcon from '@mui/icons-material/Save';
import { PRIMARY_BLUE, DARK_GRAY } from '../../../assets/styles/colors';

const PasswordChangeForm = ({ onSave }) => {
  const [formData, setFormData] = useState({
    currentPassword: '',
    newPassword: '',
    confirmPassword: '',
  });
  const [error, setError] = useState('');

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
    setError('');
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    // Walidacja
    if (formData.newPassword !== formData.confirmPassword) {
      setError('Nowe hasła nie są identyczne');
      return;
    }

    if (formData.newPassword.length < 8) {
      setError('Hasło musi zawierać co najmniej 8 znaków');
      return;
    }

    if (onSave) {
      onSave({
        currentPassword: formData.currentPassword,
        newPassword: formData.newPassword,
      });

      // Wyczyść formularz po zapisie
      setFormData({
        currentPassword: '',
        newPassword: '',
        confirmPassword: '',
      });
    }
  };

  return (
    <Card>
      <CardContent>
        <Typography variant="h6" sx={{ mb: 3, fontWeight: 'bold', color: DARK_GRAY }}>
          Zmiana hasła
        </Typography>
        <form onSubmit={handleSubmit}>
          <Stack spacing={3}>
            {error && (
              <Alert severity="error" onClose={() => setError('')}>
                {error}
              </Alert>
            )}

            <TextField
              fullWidth
              label="Obecne hasło"
              name="currentPassword"
              type="password"
              value={formData.currentPassword}
              onChange={handleChange}
              required
              variant="outlined"
            />

            <TextField
              fullWidth
              label="Nowe hasło"
              name="newPassword"
              type="password"
              value={formData.newPassword}
              onChange={handleChange}
              required
              variant="outlined"
              helperText="Minimum 8 znaków"
            />

            <TextField
              fullWidth
              label="Powtórz nowe hasło"
              name="confirmPassword"
              type="password"
              value={formData.confirmPassword}
              onChange={handleChange}
              required
              variant="outlined"
            />

            <Button
              type="submit"
              variant="contained"
              startIcon={<SaveIcon />}
              sx={{
                backgroundColor: PRIMARY_BLUE,
                alignSelf: 'flex-start',
                '&:hover': {
                  backgroundColor: '#0a58ca',
                },
              }}
            >
              Zmień hasło
            </Button>
          </Stack>
        </form>
      </CardContent>
    </Card>
  );
};

export default PasswordChangeForm;
