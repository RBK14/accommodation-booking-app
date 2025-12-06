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
    Tooltip
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import VisibilityIcon from '@mui/icons-material/Visibility'; // <--- WAŻNE: Dodany import ikony
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';
import { translateAccommodationType } from '../../utils/accommodationTypeMapper';

// Przykładowe dane (Twoje)
const mockOffers = [
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
        currency: 'EUR'
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
        currency: 'EUR'
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
        currency: 'EUR'
    }
];

const AdminOffersPage = () => {
    const [offers, setOffers] = useState(mockOffers);
    const navigate = useNavigate(); // <--- Inicjalizacja nawigacji

    // ZMIANA: Przekierowanie do NOWEGO pliku AdminOfferDetailsPage
    const handleView = (id) => {
        const offerToView = offers.find(offer => offer.id === id);
        const mockImages = [
            'https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=500&h=500&fit=crop'
        ];
        navigate(`/admin/offer/${id}`, { state: { listing: offerToView, images: mockImages } });
    };

    // ZMIANA: Przekierowanie do NOWEGO pliku AdminEditOfferPage
    const handleEdit = (id) => {
        const offerToEdit = offers.find(offer => offer.id === id);
        const mockImages = [
            'https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=500&h=500&fit=crop'
        ];

        navigate(`/admin/offer/${id}/edit`, { state: { listing: offerToEdit, images: mockImages } });
    };

    return (
        <Box>
            <Typography variant="h4" sx={{ mb: 3, fontWeight: 'bold', color: DARK_GRAY }}>
                Baza Ofert
            </Typography>

            <TableContainer component={Paper} sx={{ boxShadow: '0 4px 12px rgba(0,0,0,0.05)' }}>
                <Table sx={{ minWidth: 1000 }} aria-label="tabela ofert">
                    <TableHead sx={{ backgroundColor: '#f8f9fa' }}>
                        <TableRow>
                            <TableCell sx={{ fontWeight: 'bold', width: '15%', whiteSpace: 'nowrap' }}>ID Oferty</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', width: '15%', whiteSpace: 'nowrap' }}>ID Gospodarza</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', width: '20%' }}>Tytuł</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', width: '7%' }}>Typ</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', width: '7%' }}>Kraj</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', width: '7%' }}>Miasto</TableCell>
                            <TableCell sx={{ fontWeight: 'bold', width: '20%' }}>Adres</TableCell>
                            <TableCell align="center" sx={{ fontWeight: 'bold', width: '9%' }}>Akcje</TableCell>
                        </TableRow>
                    </TableHead>

                    <TableBody>
                        {offers.map((offer) => (
                            <TableRow
                                key={offer.id}
                                sx={{ '&:last-child td, &:last-child th': { border: 0 }, '&:hover': { backgroundColor: '#f1f3f5' } }}
                            >
                                <TableCell component="th" scope="row" sx={{ fontFamily: 'monospace', fontSize: '0.8rem', color: '#6c757d' }}>
                                    {offer.id}
                                </TableCell>

                                <TableCell sx={{ fontFamily: 'monospace', fontSize: '0.8rem', color: '#6c757d' }}>
                                    {offer.hostProfileId}
                                </TableCell>

                                <TableCell sx={{ fontWeight: 'bold' }}>
                                    {offer.title}
                                </TableCell>

                                <TableCell>
                                    {translateAccommodationType(offer.accommodationType)}
                                </TableCell>

                                <TableCell>{offer.country}</TableCell>
                                <TableCell>{offer.city}</TableCell>

                                <TableCell>
                                    {offer.street} {offer.buildingNumber}
                                </TableCell>

                                {/* AKCJE */}
                                <TableCell align="center">
                                    <Box sx={{ display: 'flex', justifyContent: 'center', gap: 0.5 }}>
                                        {/* Zielone Oczko */}
                                        <Tooltip title="Podgląd">
                                            <IconButton
                                                size="small"
                                                sx={{ color: '#2e7d32' }}
                                                onClick={() => handleView(offer.id)}
                                            >
                                                <VisibilityIcon fontSize="small" />
                                            </IconButton>
                                        </Tooltip>

                                        {/* Niebieski Długopis */}
                                        <Tooltip title="Edytuj">
                                            <IconButton
                                                size="small"
                                                sx={{ color: PRIMARY_BLUE }}
                                                onClick={() => handleEdit(offer.id)}
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

export default AdminOffersPage;