import { useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Stack } from '@mui/material';
import { toast } from 'react-toastify';
import { ContactInfoForm, EmailForm, PasswordChangeForm } from '../../components/shared';
import { AuthContext } from '../../context';
import { useAuthApi, useUsersApi } from '../../hooks';

const GuestAccountPage = () => {
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
      updateUserData({
        firstName: data.firstName,
        lastName: data.lastName,
        phone: data.phone,
      });
      toast.success('Dane kontaktowe zaktualizowane pomy�lnie');
    } else {
      toast.error(result.error || 'B��d aktualizacji danych kontaktowych');
    }
  };

  const handleSaveEmail = async (data) => {
    const result = await updateEmail(auth.id, data.email, auth.token);
    if (result.success) {
      toast.success('Email zaktualizowany pomy�lnie - wylogowywanie...');
      logout();
      navigate('/login');
    } else {
      toast.error(result.error || 'B��d aktualizacji email');
    }
  };

  const handleChangePassword = async (data) => {
    const result = await updatePassword(
      auth.id,
      { password: data.currentPassword, newPassword: data.newPassword },
      auth.token
    );
    if (result.success) {
      toast.success('Has�o zmienione pomy�lnie - wylogowywanie...');
      logout();
      navigate('/login');
    } else {
      toast.error(result.error || 'B��d zmiany has�a');
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

export default GuestAccountPage;
