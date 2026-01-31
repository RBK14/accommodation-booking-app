import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
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
  IconButton,
  Tooltip,
  CircularProgress,
  Alert,
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import VisibilityIcon from '@mui/icons-material/Visibility';
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';
import { translateAccommodationType } from '../../utils/accommodationTypeMapper';
import { useAuth, useListingsApi } from '../../hooks';
import { toast } from 'react-toastify';

const AdminListingsPage = () => {
  const { auth } = useAuth();
  const { getListings, deleteListing, loading, error } = useListingsApi();
  const [listings, setListings] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchListings = async () => {
      if (!auth?.token) return;

      const result = await getListings(null, auth.token); // null = wszystkie oferty
      if (result.success) {
        setListings(result.data);
      }
    };

    fetchListings();
  }, [auth?.token]);

  const handleView = (id) => {
    navigate(`/admin/listing/${id}`);
  };

  const handleEdit = (id) => {
    navigate(`/admin/listing/${id}/edit`);
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Czy na pewno chcesz usunąć to ogłoszenie?')) {
      return;
    }

    const result = await deleteListing(id, auth.token);
    if (result.success) {
      toast.success('Ogłoszenie zostało usunięte');
      // Refresh listings list
      const refreshResult = await getListings(null, auth.token);
      if (refreshResult.success) {
        setListings(refreshResult.data);
      }
    } else {
      toast.error(result.error || 'Nie udało się usunąć ogłoszenia');
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
    <Box>
      <Typography variant="h4" sx={{ mb: 3, fontWeight: 'bold', color: DARK_GRAY }}>
        Baza Ogłoszeń ({listings.length})
      </Typography>

      <TableContainer component={Paper} sx={{ boxShadow: '0 4px 12px rgba(0,0,0,0.05)' }}>
        <Table sx={{ minWidth: 1000 }} aria-label="tabela ogłoszeń">
          <TableHead sx={{ backgroundColor: '#f8f9fa' }}>
            <TableRow>
              <TableCell sx={{ fontWeight: 'bold', width: '15%', whiteSpace: 'nowrap' }}>
                ID Ogłoszenia
              </TableCell>
              <TableCell sx={{ fontWeight: 'bold', width: '15%', whiteSpace: 'nowrap' }}>
                ID Gospodarza
              </TableCell>
              <TableCell sx={{ fontWeight: 'bold', width: '20%' }}>Tytuł</TableCell>
              <TableCell sx={{ fontWeight: 'bold', width: '7%' }}>Typ</TableCell>
              <TableCell sx={{ fontWeight: 'bold', width: '7%' }}>Kraj</TableCell>
              <TableCell sx={{ fontWeight: 'bold', width: '7%' }}>Miasto</TableCell>
              <TableCell sx={{ fontWeight: 'bold', width: '20%' }}>Adres</TableCell>
              <TableCell align="center" sx={{ fontWeight: 'bold', width: '9%' }}>
                Akcje
              </TableCell>
            </TableRow>
          </TableHead>

          <TableBody>
            {listings.length === 0 ? (
              <TableRow>
                <TableCell colSpan={8} align="center">
                  <Typography variant="body2" color="textSecondary">
                    Brak ogłoszeń w bazie danych
                  </Typography>
                </TableCell>
              </TableRow>
            ) : (
              listings.map((listing) => (
                <TableRow
                  key={listing.id}
                  sx={{
                    '&:last-child td, &:last-child th': { border: 0 },
                    '&:hover': { backgroundColor: '#f1f3f5' },
                  }}
                >
                  <TableCell
                    component="th"
                    scope="row"
                    sx={{ fontFamily: 'monospace', fontSize: '0.8rem', color: '#6c757d' }}
                  >
                    {listing.id}
                  </TableCell>

                  <TableCell sx={{ fontFamily: 'monospace', fontSize: '0.8rem', color: '#6c757d' }}>
                    {listing.hostProfileId}
                  </TableCell>

                  <TableCell sx={{ fontWeight: 'bold' }}>{listing.title}</TableCell>

                  <TableCell>{translateAccommodationType(listing.accommodationType)}</TableCell>

                  <TableCell>{listing.country}</TableCell>
                  <TableCell>{listing.city}</TableCell>

                  <TableCell>
                    {listing.street} {listing.buildingNumber}
                  </TableCell>

                  {/* Actions */}
                  <TableCell align="center">
                    <Box sx={{ display: 'flex', justifyContent: 'center', gap: 0.5 }}>
                      {/* View button */}
                      <Tooltip title="Podgląd">
                        <IconButton
                          size="small"
                          sx={{ color: '#2e7d32' }}
                          onClick={() => handleView(listing.id)}
                        >
                          <VisibilityIcon fontSize="small" />
                        </IconButton>
                      </Tooltip>

                      {/* Edit button */}
                      <Tooltip title="Edytuj">
                        <IconButton
                          size="small"
                          sx={{ color: PRIMARY_BLUE }}
                          onClick={() => handleEdit(listing.id)}
                        >
                          <EditIcon fontSize="small" />
                        </IconButton>
                      </Tooltip>

                      {/* Delete button */}
                      <Tooltip title="Usuń">
                        <IconButton
                          size="small"
                          sx={{ color: '#dc3545' }}
                          onClick={() => handleDelete(listing.id)}
                        >
                          <DeleteIcon fontSize="small" />
                        </IconButton>
                      </Tooltip>
                    </Box>
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

export default AdminListingsPage;
