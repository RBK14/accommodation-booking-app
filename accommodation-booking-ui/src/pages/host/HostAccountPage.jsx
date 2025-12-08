import { useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Typography, Stack } from '@mui/material';
import { ContactInfoForm, EmailForm, PasswordChangeForm } from '../../components/shared';
import { DARK_GRAY } from '../../assets/styles/colors';
import AuthContext from '../../context/AuthProvider';
import { useAuthApi, useUsersApi } from '../../hooks';

const HostAccountPage = () => {
  const navigate = useNavigate();
  const { auth, userData, updateUserData, logout } = useContext(AuthContext);
  const { updateEmail, updatePassword } = useAuthApi();
  const { updatePersonalDetails } = useUsersApi();

  const handleSaveContactInfo = async (data) => {
    const result = await updatePersonalDetails(
      auth.id,
      {
        firstName: data.firstName,
        lastName: data.lastName,
        phone: data.phone,
      },
      auth.token
    );
    if (result.success) {
      // Zaktualizuj dane użytkownika w kontekście
      updateUserData({
        firstName: data.firstName,
        lastName: data.lastName,
        phone: data.phone,
      });
      console.log('Dane kontaktowe zaktualizowane pomyślnie');
    } else {
      console.error('Błąd aktualizacji danych kontaktowych:', result.error);
    }
  };

  const handleSaveEmail = async (data) => {
    const result = await updateEmail(auth.id, data.email, auth.token);
    if (result.success) {
      console.log('Email zaktualizowany pomyślnie - wylogowywanie...');
      // Wyloguj użytkownika po zmianie email
      logout();
      navigate('/auth/login');
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
      console.log('Hasło zmienione pomyślnie - wylogowywanie...');
      // Wyloguj użytkownika po zmianie hasła
      logout();
      navigate('/auth/login');
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

export default HostAccountPage;
