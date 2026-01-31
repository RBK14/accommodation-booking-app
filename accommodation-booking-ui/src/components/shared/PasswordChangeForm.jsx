import { useState } from 'react';
import { Box, Card, CardContent, TextField, Button, Typography, Stack, Alert } from '@mui/material';
import SaveIcon from '@mui/icons-material/Save';
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';

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
    if (formData.newPassword !== formData.confirmPassword) {
      setError("New passwords don't match");
      return;
    }

    if (formData.newPassword.length < 8) {
      setError('Password must be at least 8 characters');
      return;
    }

    if (onSave) {
      onSave({
        currentPassword: formData.currentPassword,
        newPassword: formData.newPassword,
      });
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
          Change Password
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
              label="Current Password"
              name="currentPassword"
              type="password"
              value={formData.currentPassword}
              onChange={handleChange}
              required
              variant="outlined"
            />

            <TextField
              fullWidth
              label="New Password"
              name="newPassword"
              type="password"
              value={formData.newPassword}
              onChange={handleChange}
              required
              variant="outlined"
              helperText="Minimum 8 characters"
            />

            <TextField
              fullWidth
              label="Confirm New Password"
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
              Change Password
            </Button>
          </Stack>
        </form>
      </CardContent>
    </Card>
  );
};

export default PasswordChangeForm;
