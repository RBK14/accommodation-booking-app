import React, { useState } from 'react';
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
    Tooltip
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import VisibilityIcon from '@mui/icons-material/Visibility';
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';

// Przykładowe dane (później zastąpisz je danymi z API)
const mockUsers = [
    {
        id: 'f47ac10b-58cc-4372-a567-0e02b2c3d479',
        email: 'jan.kowalski@example.com',
        firstName: 'Jan',
        lastName: 'Kowalski',
        phone: '123-456-789',
        userRole: 'Guest'
    },
    {
        id: 'c2d29867-3d0b-4497-9e96-66f568656209',
        email: 'anna.nowak@hostly.com',
        firstName: 'Anna',
        lastName: 'Nowak',
        phone: '987-654-321',
        userRole: 'Host'
    },
    {
        id: 'a1b2c3d4-e5f6-7890-1234-56789abcdef0',
        email: 'admin@hostly.com',
        firstName: 'Piotr',
        lastName: 'Adminowski',
        phone: '555-666-777',
        userRole: 'Admin'
    }
];

const AdminUsersPage = () => {
    const [users, setUsers] = useState(mockUsers);

    // Funkcja pomocnicza do kolorowania ról
    const getRoleColor = (role) => {
        switch (role) {
            case 'Admin': return 'error';   // Czerwony
            case 'Host': return 'primary';  // Niebieski
            case 'Guest': return 'default'; // Szary
            default: return 'default';
        }
    };

    return (
        <Box>
            <Typography variant="h4" sx={{ mb: 3, fontWeight: 'bold', color: DARK_GRAY }}>
                Baza Użytkowników
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
                            <TableCell align="center" sx={{ fontWeight: 'bold' }}>Akcje</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {users.map((user) => (
                            <TableRow
                                key={user.id}
                                sx={{ '&:last-child td, &:last-child th': { border: 0 }, '&:hover': { backgroundColor: '#f1f3f5' } }}
                            >
                                {/* ID - czcionka monospace dla czytelności */}
                                <TableCell component="th" scope="row" sx={{ fontFamily: 'monospace', fontSize: '0.8rem', color: '#6c757d' }}>
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

                                {/* Przyciski akcji */}
                                <TableCell align="center">
                                    <Tooltip title="Podgląd">
                                        <IconButton size="small" sx={{ color: '#2e7d32' }} >
                                            <VisibilityIcon fontSize="small" />
                                        </IconButton>
                                    </Tooltip>
                                    <Tooltip title="Edytuj">
                                        <IconButton size="small" sx={{ color: PRIMARY_BLUE }}>
                                            <EditIcon fontSize="small" />
                                        </IconButton>
                                    </Tooltip>
                                    <Tooltip title="Usuń">
                                        <IconButton size="small" sx={{ color: '#dc3545' }}>
                                            <DeleteIcon fontSize="small" />
                                        </IconButton>
                                    </Tooltip>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </Box>
    );
};

export default AdminUsersPage;