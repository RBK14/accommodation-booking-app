import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import {
  Box,
  Typography,
  TextField,
  Button,
  Card,
  CardContent,
  Stack,
  CircularProgress,
  Alert,
  Chip,
} from '@mui/material';
import SaveIcon from '@mui/icons-material/Save';
import CancelIcon from '@mui/icons-material/Cancel';
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';
import { useAuth, useUsersApi } from '../../hooks';
import { toast } from 'react-toastify';

const AdminEditUserPage = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const { auth } = useAuth();
  const { getUser, updatePersonalDetails, loading, error } = useUsersApi();

  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    phone: '',
    email: '',
    userRole: '',
  });
  const [isSaving, setIsSaving] = useState(false);

  useEffect(() => {
    const fetchUser = async () => {
      if (!auth?.token || !id) return;

      const result = await getUser(id, auth.token);
      if (result.success) {
        const user = result.data;
        setFormData({
          firstName: user.firstName || '',
          lastName: user.lastName || '',
          phone: user.phone || '',
          email: user.email || '',
          userRole: user.userRole || '',
        });
      }
    };

    fetchUser();
  }, [id, auth?.token]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSave = async () => {
    setIsSaving(true);

    const dataToSend = {
      firstName: formData.firstName,
      lastName: formData.lastName,
      phone: formData.phone,
    };

    const result = await updatePersonalDetails(id, dataToSend, auth.token);

    setIsSaving(false);

    if (result.success) {
      toast.success('Dane użytkownika zostały zaktualizowane');
      navigate(`/admin/user/${id}`);
    } else {
      toast.error(result.error || 'Nie udało się zaktualizować danych użytkownika');
    }
  };

  const handleCancel = () => {
    navigate(-1);
  };

  const getRoleColor = (role) => {
    switch (role) {
      case 'Admin':
        return 'error';
      case 'Host':
        return 'primary';
      case 'Guest':
        return 'default';
      default:
        return 'default';
    }
  };

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', py: 10 }}>
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Box sx={{ p: 3 }}>
        <Alert severity="error">{error}</Alert>
      </Box>
    );
  }

  return (
    <Box sx={{ p: 3 }}>
      <Card>
        <CardContent>
          <Typography variant="h5" sx={{ fontWeight: 'bold', color: DARK_GRAY, mb: 3 }}>
            Edycja danych użytkownika
          </Typography>

          <Stack spacing={3} sx={{ maxWidth: '600px' }}>
            {/* Email - tylko do odczytu */}
            <Box>
              <Typography variant="body2" sx={{ color: 'textSecondary', fontWeight: 'bold', mb: 1 }}>
                Adres email
              </Typography>
              <TextField
                fullWidth
                name="email"
                value={formData.email}
                variant="outlined"
                disabled
                sx={{
                  '& .MuiInputBase-input.Mui-disabled': {
                    WebkitTextFillColor: 'rgba(0, 0, 0, 0.6)',
                  },
                }}
              />
            </Box>

            {/* Rola - tylko do odczytu */}
            <Box>
              <Typography variant="body2" sx={{ color: 'textSecondary', fontWeight: 'bold', mb: 1 }}>
                Rola w systemie
              </Typography>
              <Chip
                label={formData.userRole}
                color={getRoleColor(formData.userRole)}
                size="medium"
                variant="outlined"
                sx={{ fontWeight: 'bold' }}
              />
            </Box>

            {/* Imię */}
            <TextField
              fullWidth
              label="Imię"
              name="firstName"
              value={formData.firstName}
              onChange={handleChange}
              variant="outlined"
              required
            />

            {/* Nazwisko */}
            <TextField
              fullWidth
              label="Nazwisko"
              name="lastName"
              value={formData.lastName}
              onChange={handleChange}
              variant="outlined"
              required
            />

            {/* Telefon */}
            <TextField
              fullWidth
              label="Numer telefonu"
              name="phone"
              value={formData.phone}
              onChange={handleChange}
              variant="outlined"
              placeholder="np. 123-456-789"
            />

            {/* Przyciski */}
            <Stack direction="row" spacing={2} sx={{ mt: 4 }}>
              <Button
                variant="contained"
                startIcon={<SaveIcon />}
                disabled={isSaving}
                sx={{
                  backgroundColor: PRIMARY_BLUE,
                  '&:hover': {
                    backgroundColor: '#0a58ca',
                  },
                }}
                onClick={handleSave}
              >
                {isSaving ? 'Zapisywanie...' : 'Zapisz zmiany'}
              </Button>
              <Button
                variant="outlined"
                startIcon={<CancelIcon />}
                disabled={isSaving}
                sx={{
                  borderColor: DARK_GRAY,
                  color: DARK_GRAY,
                  '&:hover': {
                    borderColor: DARK_GRAY,
                    backgroundColor: 'rgba(33, 37, 41, 0.04)',
                  },
                }}
                onClick={handleCancel}
              >
                Anuluj
              </Button>
            </Stack>
          </Stack>
        </CardContent>
      </Card>
    </Box>
  );
};

export default AdminEditUserPage;