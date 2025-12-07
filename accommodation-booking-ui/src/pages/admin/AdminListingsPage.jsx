import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom'; // <--- WAŻNE: Dodany import
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
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import VisibilityIcon from '@mui/icons-material/Visibility'; // <--- WAŻNE: Dodany import ikony
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';
import { translateAccommodationType } from '../../utils/accommodationTypeMapper';

// Przykładowe dane (Twoje)
const mockListings = [
  {
    id: 'd290f1ee-6c54-4b01-90e6-d701748f0851',
    hostProfileId: 'f47ac10b-58cc-4372-a567-0e02b2c3d479',
    title: 'Apartament w centrum miasta',
    accommodationType: 'Apartment',
    beds: 2,
    maxGuests: 4,
    country: 'Polska',
    city: 'Warszawa',
    street: 'Marszałkowska',
    postalCode: '00-545',
    buildingNumber: '50/12',
    amountPerDay: 250,
    currency: 'EUR',
  },
  {
    id: 'a1b2c3d4-e5f6-7890-1234-56789abcdef0',
    hostProfileId: 'c2d29867-3d0b-4497-9e96-66f568656209',
    title: 'Domek w górach',
    accommodationType: 'House',
    beds: 2,
    maxGuests: 4,
    country: 'Polska',
    postalCode: '00-555',
    city: 'Zakopane',
    street: 'Krupówki',
    buildingNumber: '15',
    amountPerDay: 250,
    currency: 'EUR',
  },
  {
    id: '98765432-1234-5678-90ab-cdef12345678',
    hostProfileId: 'f47ac10b-58cc-4372-a567-0e02b2c3d479',
    title: 'Hotel Mercury - Pokój Deluxe',
    accommodationType: 'Hotel',
    beds: 2,
    maxGuests: 4,
    country: 'Polska',
    postalCode: '00-595',
    city: 'Gdańsk',
    street: 'Długa',
    buildingNumber: '5',
    amountPerDay: 250,
    currency: 'EUR',
  },
];

const AdminListingsPage = () => {
  const [listings, setListings] = useState(mockListings);
  const navigate = useNavigate(); // <--- Inicjalizacja nawigacji

  // ZMIANA: Przekierowanie do NOWEGO pliku AdminListingDetailsPage
  const handleView = (id) => {
    const listingToView = listings.find((listing) => listing.id === id);
    const mockImages = [
      'https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=500&h=500&fit=crop',
    ];
    navigate(`/admin/listing/${id}`, { state: { listing: listingToView, images: mockImages } });
  };

  // ZMIANA: Przekierowanie do NOWEGO pliku AdminEditListingPage
  const handleEdit = (id) => {
    const listingToEdit = listings.find((listing) => listing.id === id);
    const mockImages = [
      'https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=500&h=500&fit=crop',
    ];

    navigate(`/admin/listing/${id}/edit`, {
      state: { listing: listingToEdit, images: mockImages },
    });
  };

  return (
    <Box>
      <Typography variant="h4" sx={{ mb: 3, fontWeight: 'bold', color: DARK_GRAY }}>
        Baza Ogłoszeń
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
            {listings.map((listing) => (
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

                {/* AKCJE */}
                <TableCell align="center">
                  <Box sx={{ display: 'flex', justifyContent: 'center', gap: 0.5 }}>
                    {/* Zielone Oczko */}
                    <Tooltip title="Podgląd">
                      <IconButton
                        size="small"
                        sx={{ color: '#2e7d32' }}
                        onClick={() => handleView(listing.id)}
                      >
                        <VisibilityIcon fontSize="small" />
                      </IconButton>
                    </Tooltip>

                    {/* Niebieski Długopis */}
                    <Tooltip title="Edytuj">
                      <IconButton
                        size="small"
                        sx={{ color: PRIMARY_BLUE }}
                        onClick={() => handleEdit(listing.id)}
                      >
                        <EditIcon fontSize="small" />
                      </IconButton>
                    </Tooltip>

                    {/* Czerwony Kosz */}
                    <Tooltip title="Usuń">
                      <IconButton size="small" sx={{ color: '#dc3545' }}>
                        <DeleteIcon fontSize="small" />
                      </IconButton>
                    </Tooltip>
                  </Box>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
};

export default AdminListingsPage;
