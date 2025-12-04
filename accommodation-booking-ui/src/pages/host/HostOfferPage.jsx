import { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Box, Card, CardContent } from '@mui/material';
import {
  OfferDetailsSection,
  OfferGallerySection,
  ReservationsSection,
  ReviewsSection,
} from '../../components/ui/host';

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
      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Box sx={{ display: 'flex', gap: 3 }}>
            <OfferDetailsSection listing={listing} onEdit={handleEdit} onDelete={handleDelete} />
            <OfferGallerySection images={images} />
          </Box>
        </CardContent>
      </Card>

      <ReservationsSection />
      <ReviewsSection />
    </Box>
  );
};

export default HostOfferPage;
