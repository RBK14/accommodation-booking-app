import { useState, useEffect } from 'react';
import {
  Box,
  Card,
  CardContent,
  Typography,
  Button,
  Pagination,
  CircularProgress,
  Alert,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Chip,
} from '@mui/material';
import VisibilityIcon from '@mui/icons-material/Visibility';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import PeopleIcon from '@mui/icons-material/People';
import HotelIcon from '@mui/icons-material/Hotel';
import { useNavigate } from 'react-router-dom';
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';
import { translateAccommodationType } from '../../utils';

const ListingsList = ({ listings = [], loading = false, error = null }) => {
  const navigate = useNavigate();
  const [page, setPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(10);
  const [displayedListings, setDisplayedListings] = useState([]);

  // Paginacja
  const totalPages = Math.ceil(listings.length / itemsPerPage);
  const startIndex = (page - 1) * itemsPerPage;
  const endIndex = startIndex + itemsPerPage;

  // Lazy loading - ładujemy tylko aktualną stronę
  useEffect(() => {
    const currentPageListings = listings.slice(startIndex, endIndex);
    setDisplayedListings(currentPageListings);
  }, [listings, startIndex, endIndex]);

  const handlePageChange = (event, value) => {
    setPage(value);
    // Scroll do góry przy zmianie strony
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  const handleItemsPerPageChange = (event) => {
    setItemsPerPage(event.target.value);
    setPage(1); // Reset do pierwszej strony
  };

  const handleViewListing = (id) => {
    navigate(`/listing/${id}`);
  };

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', py: 8 }}>
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Alert severity="error" sx={{ mb: 3 }}>
        {error}
      </Alert>
    );
  }

  if (listings.length === 0) {
    return (
      <Alert severity="info" sx={{ mb: 3 }}>
        Nie znaleziono ogłoszeń spełniających kryteria wyszukiwania
      </Alert>
    );
  }

  return (
    <Box>
      {/* Informacje o wynikach i kontrolki */}
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          mb: 3,
        }}
      >
        <Typography variant="body1" sx={{ color: DARK_GRAY }}>
          Znaleziono <strong>{listings.length}</strong> ogłoszeń
        </Typography>

        <FormControl size="small" sx={{ minWidth: 120 }}>
          <InputLabel>Na stronie</InputLabel>
          <Select value={itemsPerPage} onChange={handleItemsPerPageChange} label="Na stronie">
            <MenuItem value={5}>5</MenuItem>
            <MenuItem value={10}>10</MenuItem>
            <MenuItem value={20}>20</MenuItem>
            <MenuItem value={50}>50</MenuItem>
          </Select>
        </FormControl>
      </Box>

      {/* Lista ogłoszeń */}
      <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
        {displayedListings.map((listing) => (
          <Card
            key={listing.id}
            sx={{
              display: 'flex',
              flexDirection: { xs: 'column', sm: 'row' },
              transition: 'transform 0.2s, box-shadow 0.2s',
              '&:hover': {
                transform: 'translateY(-2px)',
                boxShadow: '0 8px 16px rgba(0, 0, 0, 0.15)',
              },
            }}
          >
            {/* Zdjęcie po lewej stronie */}
            <Box
              sx={{
                width: { xs: '100%', sm: 280 },
                height: { xs: 200, sm: '100%' },
                minHeight: { xs: 200, sm: 280 },
                backgroundColor: '#f5f5f5',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                flexShrink: 0,
                overflow: 'hidden',
              }}
            >
              {listing.photos && listing.photos.length > 0 ? (
                <Box
                  component="img"
                  src={`${listing.photos[0]}?w=720&h=720&&fit=crop&crop=entropy`}
                  alt={listing.title}
                  sx={{
                    width: '100%',
                    height: '100%',
                    objectFit: 'cover',
                  }}
                />
              ) : (
                <Typography variant="body2" color="textSecondary">
                  Brak zdjęcia
                </Typography>
              )}
            </Box>

            {/* Treść karty */}
            <Box sx={{ display: 'flex', flexDirection: 'column', flexGrow: 1, minWidth: 0 }}>
              <CardContent sx={{ flexGrow: 1, pb: 1 }}>
                <Box
                  sx={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    alignItems: 'flex-start',
                    mb: 2,
                    gap: 2,
                  }}
                >
                  <Box sx={{ flexGrow: 1, minWidth: 0 }}>
                    <Typography
                      variant="h5"
                      component="div"
                      sx={{
                        fontWeight: 'bold',
                        color: DARK_GRAY,
                        mb: 0.5,
                        overflow: 'hidden',
                        textOverflow: 'ellipsis',
                        whiteSpace: 'nowrap',
                      }}
                    >
                      {listing.title}
                    </Typography>

                    <Chip
                      label={translateAccommodationType(listing.accommodationType)}
                      size="small"
                      sx={{ mb: 1 }}
                    />
                  </Box>

                  <Box sx={{ textAlign: 'right', flexShrink: 0 }}>
                    <Typography variant="h5" sx={{ color: PRIMARY_BLUE, fontWeight: 'bold' }}>
                      {listing.amountPerDay} {listing.currency}
                    </Typography>
                    <Typography variant="body2" sx={{ color: 'textSecondary' }}>
                      / noc
                    </Typography>
                  </Box>
                </Box>

                <Box sx={{ display: 'flex', alignItems: 'center', mb: 1, gap: 0.5 }}>
                  <LocationOnIcon sx={{ fontSize: 18, color: 'textSecondary' }} />
                  <Typography variant="body2" color="textSecondary">
                    {listing.street} {listing.buildingNumber}
                    {listing.apartmentNumber && `/${listing.apartmentNumber}`}, {listing.city},{' '}
                    {listing.postalCode}
                  </Typography>
                </Box>

                <Box sx={{ display: 'flex', gap: 3, mt: 2 }}>
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                    <HotelIcon sx={{ fontSize: 18, color: 'textSecondary' }} />
                    <Typography variant="body2" color="textSecondary">
                      {listing.beds} {listing.beds === 1 ? 'łóżko' : 'łóżka'}
                    </Typography>
                  </Box>

                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                    <PeopleIcon sx={{ fontSize: 18, color: 'textSecondary' }} />
                    <Typography variant="body2" color="textSecondary">
                      max {listing.maxGuests} {listing.maxGuests === 1 ? 'gość' : 'gości'}
                    </Typography>
                  </Box>
                </Box>

                {listing.description && (
                  <Typography
                    variant="body2"
                    color="textSecondary"
                    sx={{
                      mt: 2,
                      display: '-webkit-box',
                      WebkitLineClamp: 2,
                      WebkitBoxOrient: 'vertical',
                      overflow: 'hidden',
                      textOverflow: 'ellipsis',
                    }}
                  >
                    {listing.description}
                  </Typography>
                )}
              </CardContent>

              <Box sx={{ px: 2, pb: 2, pt: 0 }}>
                <Button
                  variant="contained"
                  startIcon={<VisibilityIcon />}
                  sx={{
                    backgroundColor: PRIMARY_BLUE,
                    '&:hover': {
                      backgroundColor: '#0a58ca',
                    },
                  }}
                  onClick={() => handleViewListing(listing.id)}
                >
                  Zobacz szczegóły
                </Button>
              </Box>
            </Box>
          </Card>
        ))}
      </Box>

      {/* Paginacja */}
      {totalPages > 1 && (
        <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
          <Pagination
            count={totalPages}
            page={page}
            onChange={handlePageChange}
            color="primary"
            size="large"
            showFirstButton
            showLastButton
          />
        </Box>
      )}
    </Box>
  );
};

export default ListingsList;
