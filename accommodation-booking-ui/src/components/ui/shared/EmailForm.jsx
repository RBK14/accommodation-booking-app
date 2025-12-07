import { useState } from 'react';
import { Box, Card, CardContent, TextField, Button, Typography, Stack } from '@mui/material';
import SaveIcon from '@mui/icons-material/Save';
import { PRIMARY_BLUE, DARK_GRAY } from '../../../assets/styles/colors';

const EmailForm = ({ initialEmail = '', onSave }) => {
  const [email, setEmail] = useState(initialEmail);

  const handleSubmit = (e) => {
    e.preventDefault();
    if (onSave) {
      onSave({ email });
    }
  };

  return (
    <Card>
      <CardContent>
        <Typography variant="h6" sx={{ mb: 3, fontWeight: 'bold', color: DARK_GRAY }}>
          Adres e-mail
        </Typography>
        <form onSubmit={handleSubmit}>
          <Stack spacing={3}>
            <TextField
              fullWidth
              label="E-mail"
              name="email"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
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
              Zapisz zmiany
            </Button>
          </Stack>
        </form>
      </CardContent>
    </Card>
  );
};

export default EmailForm;
