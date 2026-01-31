/**
 * Host Listings Page Component
 * Displays all listings owned by the current host.
 * Provides navigation to create new listings and view individual listing details.
 */
import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  Card,
  CardContent,
  CardActions,
  Typography,
  Button,
  Grid,
  CircularProgress,
  Alert,
} from '@mui/material';
import VisibilityIcon from '@mui/icons-material/Visibility';
import AddIcon from '@mui/icons-material/Add';
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';
import { useAuth } from '../../hooks';
import { useListingsApi } from '../../hooks';

const HostListingsPage = () => {
  const navigate = useNavigate();
  const { auth, userData } = useAuth();
  const { getListings, loading, error } = useListingsApi();
  const [listings, setListings] = useState([]);

  /**
   * Fetches listings for the current host on mount.
   */
  useEffect(() => {
    const fetchListings = async () => {
      if (!userData?.profileId || !auth?.token) return;

      const result = await getListings(userData.profileId, auth.token);
      if (result.success) {
        setListings(result.data);
      }
    };

    fetchListings();
  }, [userData?.profileId, auth?.token]);

  /**
   * Navigates to listing details page.
   * @param {string} id - Listing ID
   */
  const handleView = (id) => {
    navigate(`/host/listing/${id}`);
  };

  /**
   * Navigates to new listing creation page.
   */
  const handleAddNew = () => {
    navigate('/host/new-listing');
  };

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
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

  if (listings.length === 0) {
    return (
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'center',
          alignItems: 'center',
          height: '100%',
          minHeight: '400px',
          textAlign: 'center',
          p: 3,
        }}
      >
        <Typography variant="h5" sx={{ color: DARK_GRAY, mb: 2, fontWeight: 'bold' }}>
          Brak ogłoszeń
        </Typography>
        <Typography variant="body1" sx={{ color: 'textSecondary', mb: 4, maxWidth: '500px' }}>
          Nie masz jeszcze żadnych ogłoszeń. Dodaj swoje pierwsze ogłoszenie, aby zacząć wynajmować
          swoją nieruchomość.
        </Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          size="large"
          sx={{
            backgroundColor: PRIMARY_BLUE,
            '&:hover': {
              backgroundColor: '#0a58ca',
            },
            px: 4,
            py: 1.5,
          }}
          onClick={handleAddNew}
        >
          Dodaj ogłoszenie
        </Button>
      </Box>
    );
  }

  return (
    <Box
      sx={{
        width: '100%',
        height: '100%',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'flex-start',
        py: 2,
      }}
    >
      <Grid
        container
        spacing={3}
        sx={{
          width: '100%',
          maxWidth: '100%',
          justifyContent: 'center',
        }}
      >
        {listings.map((listing) => (
          <Grid item sx={{ width: '30%', minWidth: '300px' }} key={listing.id}>
            <Card
              sx={{
                display: 'flex',
                flexDirection: 'column',
                height: '100%',
                transition: 'transform 0.2s, box-shadow 0.2s',
                '&:hover': {
                  transform: 'translateY(-4px)',
                  boxShadow: '0 12px 24px rgba(0, 0, 0, 0.15)',
                },
              }}
            >
              {/* Listing image */}
              <Box
                sx={{
                  height: 280,
                  backgroundColor: '#f5f5f5',
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  overflow: 'hidden',
                }}
              >
                {listing.photos && listing.photos.length > 0 ? (
                  <Box
                    component="img"
                    src={`${listing.photos[0]}?w=720&h=720&fit=crop&crop=entropy`}
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

              {/* Card content */}
              <CardContent sx={{ flexGrow: 1 }}>
                <Typography
                  gutterBottom
                  variant="h6"
                  component="div"
                  sx={{
                    fontWeight: 'bold',
                    color: DARK_GRAY,
                    overflow: 'hidden',
                    textOverflow: 'ellipsis',
                    whiteSpace: 'nowrap',
                  }}
                >
                  {listing.title}
                </Typography>
                <Typography
                  variant="body2"
                  color="textSecondary"
                  sx={{
                    overflow: 'hidden',
                    textOverflow: 'ellipsis',
                    whiteSpace: 'nowrap',
                  }}
                >
                  📍 {listing.street} {listing.buildingNumber}, {listing.postalCode} {listing.city}
                </Typography>
              </CardContent>

              {/* Action buttons */}
              <CardActions sx={{ pt: 0 }}>
                <Button
                  size="small"
                  variant="contained"
                  startIcon={<VisibilityIcon />}
                  sx={{
                    width: '100%',
                    backgroundColor: PRIMARY_BLUE,
                    '&:hover': {
                      backgroundColor: '#0a58ca',
                    },
                  }}
                  onClick={() => handleView(listing.id)}
                >
                  Przeglądaj
                </Button>
              </CardActions>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Box>
  );
};

export default HostListingsPage;
