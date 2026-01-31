import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import {
  Box,
  Card,
  CardContent,
  Typography,
  CircularProgress,
  Alert,
  Button,
  Stack,
  Chip,
  Divider,
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';
import { useAuth, useUsersApi } from '../../hooks';
import { toast } from 'react-toastify';

const AdminUserPage = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const { auth } = useAuth();
  const { getUser, deleteGuest, deleteHost, deleteAdmin, loading, error } = useUsersApi();

  const [user, setUser] = useState(null);

  useEffect(() => {
    const fetchUser = async () => {
      if (!auth?.token || !id) return;

      const result = await getUser(id, auth.token);
      if (result.success) {
        setUser(result.data);
      }
    };

    fetchUser();
  }, [id, auth?.token]);

  const handleEdit = () => {
    navigate(`/admin/user/${id}/edit`);
  };

  const handleDelete = async () => {
    if (
      !window.confirm(`Czy na pewno chcesz usunąć użytkownika ${user.firstName} ${user.lastName}?`)
    ) {
      return;
    }

    let result;
    if (user.userRole === 'Guest') {
      result = await deleteGuest(id, auth.token);
    } else if (user.userRole === 'Host') {
      result = await deleteHost(id, auth.token);
    } else if (user.userRole === 'Admin') {
      result = await deleteAdmin(id, auth.token);
    }

    if (result?.success) {
      toast.success('Użytkownik został usunięty');
      navigate('/admin/users');
    } else {
      toast.error(result?.error || 'Nie udało się usunąć użytkownika');
    }
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

  if (!user) {
    return (
      <Box sx={{ p: 3 }}>
        <Alert severity="info">Ładowanie danych...</Alert>
      </Box>
    );
  }

  return (
    <Box sx={{ p: 3 }}>
      <Card>
        <CardContent>
          <Box
            sx={{
              display: 'flex',
              justifyContent: 'space-between',
              alignItems: 'flex-start',
              mb: 3,
            }}
          >
            <Box>
              <Typography variant="h4" sx={{ fontWeight: 'bold', color: DARK_GRAY, mb: 1 }}>
                {user.firstName} {user.lastName}
              </Typography>
              <Chip
                label={user.userRole}
                color={getRoleColor(user.userRole)}
                size="small"
                sx={{ fontWeight: 'bold' }}
              />
            </Box>
            <Stack direction="row" spacing={2}>
              <Button
                variant="contained"
                startIcon={<EditIcon />}
                sx={{
                  backgroundColor: PRIMARY_BLUE,
                  '&:hover': {
                    backgroundColor: '#0a58ca',
                  },
                }}
                onClick={handleEdit}
              >
                Edytuj
              </Button>
              <Button
                variant="outlined"
                startIcon={<DeleteIcon />}
                sx={{
                  borderColor: '#dc3545',
                  color: '#dc3545',
                  '&:hover': {
                    borderColor: '#bb2d3b',
                    backgroundColor: 'rgba(220, 53, 69, 0.04)',
                  },
                }}
                onClick={handleDelete}
              >
                Usuń
              </Button>
            </Stack>
          </Box>

          <Divider sx={{ my: 3 }} />

          <Stack spacing={3}>
            {/* User ID */}
            <Box>
              <Typography
                variant="body2"
                sx={{ color: 'textSecondary', fontWeight: 'bold', mb: 0.5 }}
              >
                ID Użytkownika
              </Typography>
              <Typography
                variant="body1"
                sx={{ fontFamily: 'monospace', fontSize: '0.9rem', color: '#6c757d' }}
              >
                {user.id}
              </Typography>
            </Box>

            {/* Email */}
            <Box>
              <Typography
                variant="body2"
                sx={{ color: 'textSecondary', fontWeight: 'bold', mb: 0.5 }}
              >
                Adres email
              </Typography>
              <Typography variant="body1">{user.email}</Typography>
            </Box>

            {/* First name */}
            <Box>
              <Typography
                variant="body2"
                sx={{ color: 'textSecondary', fontWeight: 'bold', mb: 0.5 }}
              >
                Imię
              </Typography>
              <Typography variant="body1">{user.firstName}</Typography>
            </Box>

            {/* Nazwisko */}
            <Box>
              <Typography
                variant="body2"
                sx={{ color: 'textSecondary', fontWeight: 'bold', mb: 0.5 }}
              >
                Nazwisko
              </Typography>
              <Typography variant="body1">{user.lastName}</Typography>
            </Box>

            {/* Telefon */}
            <Box>
              <Typography
                variant="body2"
                sx={{ color: 'textSecondary', fontWeight: 'bold', mb: 0.5 }}
              >
                Numer telefonu
              </Typography>
              <Typography variant="body1">{user.phone || 'Brak numeru telefonu'}</Typography>
            </Box>

            {/* Rola */}
            <Box>
              <Typography
                variant="body2"
                sx={{ color: 'textSecondary', fontWeight: 'bold', mb: 0.5 }}
              >
                Rola w systemie
              </Typography>
              <Chip
                label={user.userRole}
                color={getRoleColor(user.userRole)}
                size="medium"
                variant="outlined"
                sx={{ fontWeight: 'bold' }}
              />
            </Box>
          </Stack>
        </CardContent>
      </Card>
    </Box>
  );
};

export default AdminUserPage;
