import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  Card,
  CardMedia,
  CardContent,
  CardActions,
  Typography,
  Button,
  Grid,
} from '@mui/material';
import VisibilityIcon from '@mui/icons-material/Visibility';
import { PRIMARY_BLUE, DARK_GRAY } from '../../assets/styles/colors';

const HostOffersPage = () => {
  const navigate = useNavigate();
  const [offers, setOffers] = useState([
    {
      id: 1,
      title: 'Apartament w centrum miasta',
      address: 'ul. Marszałkowska 50, 00-545 Warszawa',
      image: 'https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=500&h=300&fit=crop',
    },
    {
      id: 2,
      title: 'Przytulny studio z widokiem',
      address: 'ul. Szewska 10, 31-009 Kraków',
      image:
        'https://images.unsplash.com/photo-1702014862053-946a122b920d?q=80&w=1170?w=500&h=300&fit=crop',
    },
    {
      id: 3,
      title: 'Luksusowy penthouse',
      address: 'al. Jerozolimskie 96, 02-201 Warszawa',
      image: 'https://images.unsplash.com/photo-1565623833408-d77e39b88af6?w=500&h=300&fit=crop',
    },
    {
      id: 4,
      title: 'Domek nad jeziorem',
      address: 'Stęczno, 87-148 Nowa Wieś',
      image: 'https://images.unsplash.com/photo-1570129477492-45c003edd2be?w=500&h=300&fit=crop',
    },
    {
      id: 5,
      title: 'Willa na wsi z ogrodem',
      address: 'Gostchorze, 43-500 Chrzanów',
      image: 'https://images.unsplash.com/photo-1599932904184-02a07911e629?w=500&h=300&fit=crop',
    },
    {
      id: 6,
      title: 'Nowoczesny apartament nad morzem',
      address: 'ul. Słoneczna 15, 80-287 Gdańsk',
      image: 'https://images.unsplash.com/photo-1542928658-22251e208ac1?w=500&h=300&fit=crop',
    },
    {
      id: 7,
      title: 'Apartament w centrum miasta',
      address: 'ul. Marszałkowska 50, 00-545 Warszawa',
      image: 'https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=500&h=300&fit=crop',
    },
    {
      id: 8,
      title: 'Przytulny studio z widokiem',
      address: 'ul. Szewska 10, 31-009 Kraków',
      image:
        'https://images.unsplash.com/photo-1702014862053-946a122b920d?q=80&w=1170?w=500&h=300&fit=crop',
    },
    {
      id: 9,
      title: 'Luksusowy penthouse',
      address: 'al. Jerozolimskie 96, 02-201 Warszawa',
      image: 'https://images.unsplash.com/photo-1565623833408-d77e39b88af6?w=500&h=300&fit=crop',
    },
    {
      id: 10,
      title: 'Domek nad jeziorem',
      address: 'Stęczno, 87-148 Nowa Wieś',
      image: 'https://images.unsplash.com/photo-1570129477492-45c003edd2be?w=500&h=300&fit=crop',
    },
    {
      id: 11,
      title: 'Willa na wsi z ogrodem',
      address: 'Gostchorze, 43-500 Chrzanów',
      image: 'https://images.unsplash.com/photo-1599932904184-02a07911e629?w=500&h=300&fit=crop',
    },
    {
      id: 12,
      title: 'Nowoczesny apartament nad morzem',
      address: 'ul. Słoneczna 15, 80-287 Gdańsk',
      image: 'https://images.unsplash.com/photo-1542928658-22251e208ac1?w=500&h=300&fit=crop',
    },
  ]);

  const handleView = (id) => {
    navigate(`/host/offer/${id}`);
  };

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
        {offers.map((offer) => (
          <Grid item sx={{ width: '30%', minWidth: '300px' }} key={offer.id}>
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
              {/* Zdjęcie */}
              <CardMedia
                component="img"
                height="200"
                image={offer.image}
                alt={offer.title}
                sx={{
                  objectFit: 'cover',
                }}
              />

              {/* Zawartość */}
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
                  {offer.title}
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
                  📍 {offer.address}
                </Typography>
              </CardContent>

              {/* Przyciski */}
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
                  onClick={() => handleView(offer.id)}
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

export default HostOffersPage;
