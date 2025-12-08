import { useContext } from 'react';
import { Box, Typography, Stack } from '@mui/material';
import { ContactInfoForm, EmailForm, PasswordChangeForm } from '../../components/shared';
import { DARK_GRAY } from '../../assets/styles/colors';
import AuthContext from '../../context/AuthProvider';
import { useAuthApi } from '../../hooks/useAuthApi';

const AdminAccountPage = () => {
  const { auth, userData } = useContext(AuthContext);
  const { updateEmail, updatePassword, loading, error } = useAuthApi();

  const handleSaveContactInfo = (data) => {
    // TODO: Wysłanie danych do backendu
    console.log('Zapisywanie danych kontaktowych:', data);
  };

  const handleSaveEmail = async (data) => {
    const result = await updateEmail(auth.id, data.email, auth.token);
    if (result.success) {
      console.log('Email zaktualizowany pomyślnie');
    } else {
      console.error('Błąd aktualizacji email:', result.error);
    }
  };

  const handleChangePassword = async (data) => {
    const result = await updatePassword(
      auth.id,
      { password: data.currentPassword, newPassword: data.newPassword },
      auth.token
    );
    if (result.success) {
      console.log('Hasło zmienione pomyślnie');
    } else {
      console.error('Błąd zmiany hasła:', result.error);
    }
  };

  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
      <Stack spacing={3} sx={{ maxWidth: '800px', width: '100%' }}>
        <ContactInfoForm
          initialData={{
            firstName: userData?.firstName || '',
            lastName: userData?.lastName || '',
            phone: userData?.phone || '',
          }}
          onSave={handleSaveContactInfo}
        />

        <EmailForm initialEmail={userData?.email || ''} onSave={handleSaveEmail} />

        <PasswordChangeForm onSave={handleChangePassword} />
      </Stack>
    </Box>
  );
};

export default AdminAccountPage;
