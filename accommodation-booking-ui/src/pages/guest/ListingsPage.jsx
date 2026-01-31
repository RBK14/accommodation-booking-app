/**
 * Listings Page Component
 * Displays available accommodation listings with search and filtering capabilities.
 */
import { useState, useEffect, useMemo } from 'react';
import { useSearchParams } from 'react-router-dom';
import { Box, Container, Typography } from '@mui/material';
import { DARK_GRAY } from '../../assets/styles/colors';
import { SearchBar, ListingsList } from '../../components/guest';
import { useListingsApi } from '../../hooks';
import { useAuth } from '../../hooks';

const ListingsPage = () => {
  const [searchParams] = useSearchParams();
  const { auth } = useAuth();
  const { getListings, loading, error } = useListingsApi();
  const [allListings, setAllListings] = useState([]);

  const location = searchParams.get('location') || '';
  const guests = searchParams.get('guests') || '';

  useEffect(() => {
    const fetchListings = async () => {
      if (!auth?.token) return;

      const result = await getListings(null, auth.token);
      if (result.success) {
        setAllListings(result.data);
      }
    };

    fetchListings();
  }, [auth?.token]);

  /**
   * Filters listings based on search parameters (location, guests).
   * Searches across city, street, and country fields.
   */
  const filteredListings = useMemo(() => {
    let filtered = [...allListings];

    if (location) {
      const locationLower = location.toLowerCase().trim();
      filtered = filtered.filter((listing) => {
        const city = (listing.city || '').toLowerCase();
        const street = (listing.street || '').toLowerCase();
        const country = (listing.country || '').toLowerCase();

        return (
          city.includes(locationLower) ||
          street.includes(locationLower) ||
          country.includes(locationLower) ||
          `${city} ${street}`.includes(locationLower) ||
          `${street} ${city}`.includes(locationLower)
        );
      });
    }

    if (guests) {
      const guestsNumber = parseInt(guests, 10);
      if (!isNaN(guestsNumber)) {
        filtered = filtered.filter((listing) => listing.maxGuests >= guestsNumber);
      }
    }

    return filtered;
  }, [allListings, location, guests]);

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Box sx={{ mb: 4 }}>
        <Typography variant="h4" sx={{ fontWeight: 'bold', color: DARK_GRAY, mb: 3 }}>
          Dostępne ogłoszenia
        </Typography>

        <Box sx={{ maxWidth: '800px' }}>
          <SearchBar initialLocation={location} initialGuests={guests} compact />
        </Box>
      </Box>

      <ListingsList listings={filteredListings} loading={loading} error={error} />
    </Container>
  );
};

export default ListingsPage;
