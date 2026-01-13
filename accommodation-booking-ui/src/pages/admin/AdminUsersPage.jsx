import React, { useState, useEffect } from 'react';
import {
    Box,
    Typography,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Chip,
    IconButton,
    Tooltip,
    CircularProgress,
    Alert,
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import VisibilityIcon from '@mui/icons-material/Visibility';
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';
import { useAuth, useUsersApi } from '../../hooks';
import { toast } from 'react-toastify';
import { useNavigate } from 'react-router-dom';

const AdminUsersPage = () => {
    const { auth } = useAuth();
    const navigate = useNavigate(); // DODAJ TO
    const { getUsers, deleteGuest, deleteHost, deleteAdmin, loading, error } = useUsersApi();
    const [users, setUsers] = useState([]);

    useEffect(() => {
        const fetchUsers = async () => {
            if (!auth?.token) {
                console.error('Brak tokenu autoryzacyjnego');
                return;
            }

            console.log('Fetching users with token...'); // Debug
            const result = await getUsers(null, auth.token); // PRZEKAZANIE TOKENU
            console.log('Users result:', result); // Debug
            
            if (result.success) {
                console.log('Users data:', result.data); // Debug
                setUsers(result.data);
            } else {
                console.error('Failed to fetch users:', result.error); // Debug
            }
        };

        fetchUsers();
    }, [auth?.token]);

    // Funkcja pomocnicza do kolorowania ról
    const getRoleColor = (role) => {
        switch (role) {
            case 'Admin':
                return 'error'; // Czerwony
            case 'Host':
                return 'primary'; // Niebieski
            case 'Guest':
                return 'default'; // Szary
            default:
                return 'default';
        }
    };

    const handleDelete = async (userId, userRole) => {
        if (!window.confirm('Czy na pewno chcesz usunąć tego użytkownika?')) {
            return;
        }

        let result;
        if (userRole === 'Guest') {
            result = await deleteGuest(userId, auth.token);
        } else if (userRole === 'Host') {
            result = await deleteHost(userId, auth.token);
        } else if (userRole === 'Admin') {
            result = await deleteAdmin(userId, auth.token);
        }

        if (result?.success) {
            toast.success('Użytkownik został usunięty');
            // Odśwież listę użytkowników
            const refreshResult = await getUsers(null, auth.token);
            if (refreshResult.success) {
                setUsers(refreshResult.data);
            }
        } else {
            toast.error(result?.error || 'Nie udało się usunąć użytkownika');
        }
    };

    const handleView = (userId) => {
        navigate(`/admin/user/${userId}`);
    };

    const handleEdit = (userId) => {
        navigate(`/admin/user/${userId}/edit`);
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
        <Box>
            <Typography variant="h4" sx={{ mb: 3, fontWeight: 'bold', color: DARK_GRAY }}>
                Baza Użytkowników ({users.length})
            </Typography>

            <TableContainer component={Paper} sx={{ boxShadow: '0 4px 12px rgba(0,0,0,0.05)' }}>
                <Table sx={{ minWidth: 650 }} aria-label="tabela użytkowników">
                    <TableHead sx={{ backgroundColor: '#f8f9fa' }}>
                        <TableRow>
                            <TableCell sx={{ fontWeight: 'bold' }}>ID Użytkownika</TableCell>
                            <TableCell sx={{ fontWeight: 'bold' }}>Email</TableCell>
                            <TableCell sx={{ fontWeight: 'bold' }}>Imię</TableCell>
                            <TableCell sx={{ fontWeight: 'bold' }}>Nazwisko</TableCell>
                            <TableCell sx={{ fontWeight: 'bold' }}>Telefon</TableCell>
                            <TableCell sx={{ fontWeight: 'bold' }}>Rola</TableCell>
                            <TableCell align="center" sx={{ fontWeight: 'bold' }}>
                                Akcje
                            </TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {users.length === 0 ? (
                            <TableRow>
                                <TableCell colSpan={7} align="center">
                                    <Typography variant="body2" color="textSecondary">
                                        Brak użytkowników w bazie danych
                                    </Typography>
                                </TableCell>
                            </TableRow>
                        ) : (
                            users.map((user) => (
                                <TableRow
                                    key={user.id}
                                    sx={{
                                        '&:last-child td, &:last-child th': { border: 0 },
                                        '&:hover': { backgroundColor: '#f1f3f5' },
                                    }}
                                >
                                    {/* ID - czcionka monospace dla czytelności */}
                                    <TableCell
                                        component="th"
                                        scope="row"
                                        sx={{ fontFamily: 'monospace', fontSize: '0.8rem', color: '#6c757d' }}
                                    >
                                        {user.id}
                                    </TableCell>

                                    <TableCell>{user.email}</TableCell>
                                    <TableCell>{user.firstName}</TableCell>
                                    <TableCell>{user.lastName}</TableCell>
                                    <TableCell>{user.phone || '-'}</TableCell>

                                    {/* Rola jako kolorowy Chip */}
                                    <TableCell>
                                        <Chip
                                            label={user.userRole}
                                            color={getRoleColor(user.userRole)}
                                            size="small"
                                            variant="outlined"
                                            sx={{ fontWeight: 'bold' }}
                                        />
                                    </TableCell>

                                    {/* Przyciski akcji - ZMIEŃ TO */}
                                    <TableCell align="center">
                                        <Tooltip title="Podgląd">
                                            <IconButton 
                                                size="small" 
                                                sx={{ color: '#2e7d32' }}
                                                onClick={() => handleView(user.id)}
                                            >
                                                <VisibilityIcon fontSize="small" />
                                            </IconButton>
                                        </Tooltip>
                                        <Tooltip title="Edytuj">
                                            <IconButton 
                                                size="small" 
                                                sx={{ color: PRIMARY_BLUE }}
                                                onClick={() => handleEdit(user.id)}
                                            >
                                                <EditIcon fontSize="small" />
                                            </IconButton>
                                        </Tooltip>
                                        <Tooltip title="Usuń">
                                            <IconButton
                                                size="small"
                                                sx={{ color: '#dc3545' }}
                                                onClick={() => handleDelete(user.id, user.userRole)}
                                            >
                                                <DeleteIcon fontSize="small" />
                                            </IconButton>
                                        </Tooltip>
                                    </TableCell>
                                </TableRow>
                            ))
                        )}
                    </TableBody>
                </Table>
            </TableContainer>
        </Box>
    );
};

export default AdminUsersPage;