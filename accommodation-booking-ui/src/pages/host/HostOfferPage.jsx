import { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import {
  Box,
  Typography,
  Button,
  Stack,
  Card,
  CardContent,
  Grid,
  ImageList,
  ImageListItem,
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';
import { translateAccommodationType } from '../../utils/accommodationTypeMapper';

const HostOfferPage = () => {
  const navigate = useNavigate();
  const { id } = useParams();

  // Dane oferty (przykładowe)
  const [listing, setListing] = useState({
    title: 'Apartament w centrum miasta',
    description: 'Piękny apartament w samym sercu miasta z widokiem na ulicę.',
    accommodationType: 'Apartment',
    beds: 2,
    maxGuests: 4,
    country: 'Polska',
    city: 'Warszawa',
    postalCode: '00-545',
    street: 'ul. Marszałkowska',
    buildingNumber: '50',
    amountPerDay: 250,
    currency: 'EUR',
  });

  // Galeria zdjęć
  const [images, setImages] = useState([
    'https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=500&h=500&fit=crop',
    'https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?w=500&h=500&fit=crop',
    'https://images.unsplash.com/photo-1536376072261-38c75010e6c9?w=500&h=500&fit=crop',
    'https://images.unsplash.com/photo-1565623833408-d77e39b88af6?w=500&h=500&fit=crop',
    'https://images.unsplash.com/photo-1552321554-5fefe8c9ef14?w=500&h=500&fit=crop',
  ]);

  const handleEdit = () => {
    navigate(`/host/offer/${id}/edit`, { state: { listing, images } });
  };

  const handleDelete = () => {
    // TODO: Logika usuwania
    console.log(`Usuwanie oferty: ${id}`);
  };

  return (
    <Box sx={{ p: 3 }}>
      {/* Pierwsza karta - Dane i Galeria */}
      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Box sx={{ display: 'flex', gap: 3 }}>
            {/* Lewa strona - Dane */}
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
                  onClick={handleEdit}
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
                  onClick={handleDelete}
                >
                  Usuń
                </Button>
              </Stack>
            </Box>

            {/* Prawa strona - Galeria */}
            <Box sx={{ flex: 1, display: 'flex', flexDirection: 'column' }}>
              <Typography variant="subtitle2" sx={{ color: DARK_GRAY, fontWeight: 'bold', mb: 2 }}>
                Galeria zdjęć ({images.length})
              </Typography>
              <Box sx={{ height: '75vh', overflowY: 'auto', overflowX: 'hidden' }}>
                <ImageList
                  variant="masonry"
                  cols={2}
                  gap={8}
                  sx={{ width: '100%', minWidth: '400px', margin: 0 }}
                >
                  {images.map((image, index) => (
                    <ImageListItem key={index}>
                      <img
                        srcSet={`${image}?w=248&fit=crop&auto=format&dpr=2 2x`}
                        src={`${image}?w=248&fit=crop&auto=format`}
                        alt={`Zdjęcie ${index + 1}`}
                        loading="lazy"
                        style={{
                          borderRadius: '8px',
                        }}
                      />
                    </ImageListItem>
                  ))}
                </ImageList>
              </Box>
            </Box>
          </Box>
        </CardContent>
      </Card>

      {/* Karta z rezerwacjami */}
      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Typography variant="h6" sx={{ mb: 2, fontWeight: 'bold' }}>
            Rezerwacje
          </Typography>
          <Typography variant="body2" sx={{ color: 'textSecondary' }}>
            Rezerwacje
          </Typography>
        </CardContent>
      </Card>

      {/* Karta z opiniami */}
      <Card>
        <CardContent>
          <Typography variant="h6" sx={{ mb: 2, fontWeight: 'bold' }}>
            Opinie
          </Typography>
          <Typography variant="body2" sx={{ color: 'textSecondary' }}>
            Opinie
          </Typography>
        </CardContent>
      </Card>
    </Box>
  );
};

export default HostOfferPage;
