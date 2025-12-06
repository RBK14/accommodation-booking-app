import { Box, Typography, Button, Stack } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import { PRIMARY_BLUE, DARK_GRAY } from '../../../assets/styles/colors';
import { translateAccommodationType } from '../../../utils/accommodationTypeMapper';

const OfferDetailsSection = ({ listing, onEdit, onDelete }) => {
    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'space-between',
                width: 450,
                minWidth: 300,
            }}
        >
            <Box sx={{ display: 'flex', flexDirection: 'column', height: 'auto', gap: 2 }}>
                <Box>
                    <Typography variant="h6" sx={{ mb: 2, fontWeight: 'bold' }}>
                        {listing.title}
                    </Typography>
                </Box>

                <Box>
                    <Typography variant="subtitle2" sx={{ color: DARK_GRAY, fontWeight: 'bold' }}>
                        ID Oferty
                    </Typography>
                    <Typography variant="body2">{listing.id}</Typography>
                </Box>

                <Box>
                    <Typography variant="subtitle2" sx={{ color: DARK_GRAY, fontWeight: 'bold' }}>
                        ID Gospodarza
                    </Typography>
                    <Typography variant="body2">{listing.hostProfileId}</Typography>
                </Box>

                <Box>
                    <Typography variant="subtitle2" sx={{ color: DARK_GRAY, fontWeight: 'bold' }}>
                        Typ zakwaterowania
                    </Typography>
                    <Typography variant="body2">
                        {translateAccommodationType(listing.accommodationType)}
                    </Typography>
                </Box>

                <Box>
                    <Typography variant="subtitle2" sx={{ color: DARK_GRAY, fontWeight: 'bold' }}>
                        Opis
                    </Typography>
                    <Typography variant="body2">{listing.description}</Typography>
                </Box>

                <Box>
                    <Typography variant="subtitle2" sx={{ color: DARK_GRAY, fontWeight: 'bold' }}>
                        Liczba łóżek
                    </Typography>
                    <Typography variant="body2">{listing.beds}</Typography>
                </Box>

                <Box>
                    <Typography variant="subtitle2" sx={{ color: DARK_GRAY, fontWeight: 'bold' }}>
                        Maksimum gości
                    </Typography>
                    <Typography variant="body2">{listing.maxGuests}</Typography>
                </Box>

                <Box>
                    <Typography variant="subtitle2" sx={{ color: DARK_GRAY, fontWeight: 'bold' }}>
                        Lokalizacja
                    </Typography>
                    <Typography variant="body2">
                        {listing.street} {listing.buildingNumber}
                    </Typography>
                    <Typography variant="body2">
                        {listing.postalCode} {listing.city}, {listing.country}
                    </Typography>
                </Box>

                <Box>
                    <Typography variant="subtitle2" sx={{ color: DARK_GRAY, fontWeight: 'bold' }}>
                        Cena za noc
                    </Typography>
                    <Typography variant="body2">
                        {listing.amountPerDay} {listing.currency}
                    </Typography>
                </Box>
            </Box>

            <Stack direction="row" spacing={2} sx={{ mt: 2 }}>
                <Button
                    variant="contained"
                    startIcon={<EditIcon />}
                    sx={{
                        backgroundColor: PRIMARY_BLUE,
                        '&:hover': {
                            backgroundColor: '#0a58ca',
                        },
                    }}
                    onClick={onEdit}
                >
                    Edytuj
                </Button>
                <Button
                    variant="outlined"
                    startIcon={<DeleteIcon />}
                    sx={{
                        borderColor: DARK_GRAY,
                        color: DARK_GRAY,
                        '&:hover': {
                            borderColor: DARK_GRAY,
                            backgroundColor: 'rgba(33, 37, 41, 0.04)',
                        },
                    }}
                    onClick={onDelete}
                >
                    Usuń
                </Button>
            </Stack>
        </Box>
    );
};

export default OfferDetailsSection;
