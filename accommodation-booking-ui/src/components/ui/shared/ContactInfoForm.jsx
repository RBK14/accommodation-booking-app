import { useState } from 'react';
import { Box, Card, CardContent, TextField, Button, Typography, Stack } from '@mui/material';
import SaveIcon from '@mui/icons-material/Save';
import { PRIMARY_BLUE, DARK_GRAY } from '../../../assets/styles/colors';

const ContactInfoForm = ({ initialData = {}, onSave }) => {
  const [formData, setFormData] = useState({
    firstName: initialData.firstName || '',
    lastName: initialData.lastName || '',
    phone: initialData.phone || '',
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (onSave) {
      onSave(formData);
    }
  };

  return (
    <Card>
      <CardContent>
        <Typography variant="h6" sx={{ mb: 3, fontWeight: 'bold', color: DARK_GRAY }}>
          Dane kontaktowe
        </Typography>
        <form onSubmit={handleSubmit}>
          <Stack spacing={3}>
            <TextField
              fullWidth
              label="Imię"
              name="firstName"
              value={formData.firstName}
              onChange={handleChange}
              required
              variant="outlined"
            />

            <TextField
              fullWidth
              label="Nazwisko"
              name="lastName"
              value={formData.lastName}
              onChange={handleChange}
              required
              variant="outlined"
            />

            <TextField
              fullWidth
              label="Numer telefonu"
              name="phone"
              value={formData.phone}
              onChange={handleChange}
              variant="outlined"
              placeholder="+48 123 456 789"
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
              Zapisz zmiany
            </Button>
          </Stack>
        </form>
      </CardContent>
    </Card>
  );
};

export default ContactInfoForm;
