import { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import {
  Box,
  Typography,
  TextField,
  Button,
  Card,
  CardContent,
  Stack,
  Grid,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  ImageList,
  ImageListItem,
  IconButton,
} from '@mui/material';
import SaveIcon from '@mui/icons-material/Save';
import CancelIcon from '@mui/icons-material/Cancel';
import DeleteIcon from '@mui/icons-material/Delete';
import AddIcon from '@mui/icons-material/Add';
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';
import { translateAccommodationType } from '../../utils/accommodationTypeMapper';

const HostEditOfferPage = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const [formData, setFormData] = useState({
    title: '',
    description: '',
    accommodationType: '',
    beds: '',
    maxGuests: '',
    country: '',
    city: '',
    postalCode: '',
    street: '',
    buildingNumber: '',
    amountPerDay: '',
    currency: 'PLN',
  });
  const [images, setImages] = useState([]);

  // Pobierz dane z state (jeśli przesłane z HostOfferPage)
  useEffect(() => {
    if (location.state?.listing) {
      setFormData(location.state.listing);
    }
  }, [location.state]);

  // Pobierz zdjęcia z state (jeśli przesłane)
  useEffect(() => {
    if (location.state?.images) {
      setImages(location.state.images);
    }
  }, [location.state?.images]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleAddImage = () => {
    const imageUrl = prompt('Podaj URL zdjęcia:');
    if (imageUrl && imageUrl.trim()) {
      setImages([...images, imageUrl.trim()]);
    }
  };

  const handleDeleteImage = (index) => {
    setImages(images.filter((_, i) => i !== index));
  };

  const handleSave = () => {
    // TODO: Wysłanie danych na backend
    console.log('Zapisywanie oferty:', formData);
    console.log('Zdjęcia:', images);
    navigate(-1); // Wróć na poprzednią stronę
  };

  const handleCancel = () => {
    navigate(-1); // Wróć bez zapisywania
  };

  return (
    <Box sx={{ p: 3 }}>
      <Card>
        <CardContent>
          <Box sx={{ display: 'flex', gap: 3 }}>
            {/* Lewa strona - Formularz */}
            <Box
              sx={{ display: 'flex', flexDirection: 'column', width: 450, minWidth: 300, gap: 2 }}
            >
              <Stack spacing={2}>
                <TextField
                  fullWidth
                  label="Nazwa oferty"
                  name="title"
                  value={formData.title}
                  onChange={handleChange}
                  variant="outlined"
                />

                <TextField
                  fullWidth
                  label="Opis"
                  name="description"
                  value={formData.description}
                  onChange={handleChange}
                  variant="outlined"
                  multiline
                  rows={4}
                />

                <FormControl fullWidth>
                  <InputLabel>Typ zakwaterowania</InputLabel>
                  <Select
                    name="accommodationType"
                    value={formData.accommodationType}
                    onChange={handleChange}
                    label="Typ zakwaterowania"
                  >
                    <MenuItem value="Apartment">{translateAccommodationType('Apartment')}</MenuItem>
                    <MenuItem value="House">{translateAccommodationType('House')}</MenuItem>
                    <MenuItem value="Hotel">{translateAccommodationType('Hotel')}</MenuItem>
                  </Select>
                </FormControl>

                <TextField
                  fullWidth
                  label="Liczba łóżek"
                  name="beds"
                  type="number"
                  value={formData.beds}
                  onChange={handleChange}
                  variant="outlined"
                />

                <TextField
                  fullWidth
                  label="Maksimum gości"
                  name="maxGuests"
                  type="number"
                  value={formData.maxGuests}
                  onChange={handleChange}
                  variant="outlined"
                />

                <TextField
                  fullWidth
                  label="Kraj"
                  name="country"
                  value={formData.country}
                  onChange={handleChange}
                  variant="outlined"
                />

                <Grid container spacing={2}>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      fullWidth
                      label="Miasto"
                      name="city"
                      value={formData.city}
                      onChange={handleChange}
                      variant="outlined"
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      fullWidth
                      label="Kod pocztowy"
                      name="postalCode"
                      value={formData.postalCode}
                      onChange={handleChange}
                      variant="outlined"
                    />
                  </Grid>
                </Grid>

                <Grid container spacing={2}>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      fullWidth
                      label="Ulica"
                      name="street"
                      value={formData.street}
                      onChange={handleChange}
                      variant="outlined"
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      fullWidth
                      label="Numer budynku"
                      name="buildingNumber"
                      value={formData.buildingNumber}
                      onChange={handleChange}
                      variant="outlined"
                    />
                  </Grid>
                </Grid>

                <Grid container spacing={2}>
                  <Grid item xs={12} sm={8}>
                    <TextField
                      fullWidth
                      label="Cena za noc"
                      name="amountPerDay"
                      type="number"
                      value={formData.amountPerDay}
                      onChange={handleChange}
                      variant="outlined"
                    />
                  </Grid>
                  <Grid item xs={12} sm={4}>
                    <FormControl fullWidth>
                      <InputLabel>Waluta</InputLabel>
                      <Select
                        name="currency"
                        value={formData.currency}
                        onChange={handleChange}
                        label="Waluta"
                      >
                        <MenuItem value="PLN">PLN</MenuItem>
                        <MenuItem value="EUR">EUR</MenuItem>
                        <MenuItem value="USD">USD</MenuItem>
                      </Select>
                    </FormControl>
                  </Grid>
                </Grid>
              </Stack>

              {/* Przyciski */}
              <Stack direction="row" spacing={2} sx={{ mt: 4 }}>
                <Button
                  variant="contained"
                  startIcon={<SaveIcon />}
                  sx={{
                    backgroundColor: PRIMARY_BLUE,
                    '&:hover': {
                      backgroundColor: '#0a58ca',
                    },
                  }}
                  onClick={handleSave}
                >
                  Zapisz
                </Button>
                <Button
                  variant="outlined"
                  startIcon={<CancelIcon />}
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
            </Box>

            {/* Prawa strona - Galeria */}
            <Box sx={{ flex: 1, display: 'flex', flexDirection: 'column' }}>
              <Box
                sx={{
                  display: 'flex',
                  justifyContent: 'space-between',
                  alignItems: 'center',
                  mb: 2,
                }}
              >
                <Typography variant="subtitle2" sx={{ color: DARK_GRAY, fontWeight: 'bold' }}>
                  Galeria zdjęć ({images.length})
                </Typography>
                <Button
                  variant="contained"
                  size="small"
                  startIcon={<AddIcon />}
                  sx={{
                    backgroundColor: PRIMARY_BLUE,
                    '&:hover': {
                      backgroundColor: '#0a58ca',
                    },
                  }}
                  onClick={handleAddImage}
                >
                  Dodaj
                </Button>
              </Box>
              <Box sx={{ height: '75vh', overflowY: 'auto', overflowX: 'hidden' }}>
                {images.length > 0 ? (
                  <ImageList
                    variant="masonry"
                    cols={2}
                    gap={8}
                    sx={{ width: '100%', minWidth: '400px', margin: 0 }}
                  >
                    {images.map((image, index) => (
                      <ImageListItem
                        key={index}
                        sx={{ position: 'relative', '&:hover .delete-btn': { opacity: 1 } }}
                      >
                        <img
                          srcSet={`${image}?w=248&fit=crop&auto=format&dpr=2 2x`}
                          src={`${image}?w=248&fit=crop&auto=format`}
                          alt={`Zdjęcie ${index + 1}`}
                          loading="lazy"
                          style={{
                            borderRadius: '8px',
                          }}
                        />
                        <IconButton
                          className="delete-btn"
                          size="small"
                          sx={{
                            position: 'absolute',
                            top: 4,
                            right: 4,
                            backgroundColor: 'rgba(255, 255, 255, 0.9)',
                            opacity: 0,
                            transition: 'opacity 0.2s',
                            '&:hover': {
                              backgroundColor: 'rgba(255, 255, 255, 1)',
                            },
                          }}
                          onClick={() => handleDeleteImage(index)}
                        >
                          <DeleteIcon sx={{ fontSize: 18, color: '#d32f2f' }} />
                        </IconButton>
                      </ImageListItem>
                    ))}
                  </ImageList>
                ) : (
                  <Box
                    sx={{
                      display: 'flex',
                      alignItems: 'center',
                      justifyContent: 'center',
                      height: '100%',
                    }}
                  >
                    <Typography variant="body2" sx={{ color: 'textSecondary' }}>
                      Brak zdjęć. Kliknij "Dodaj", aby dodać zdjęcie.
                    </Typography>
                  </Box>
                )}
              </Box>
            </Box>
          </Box>
        </CardContent>
      </Card>
    </Box>
  );
};

export default HostEditOfferPage;
