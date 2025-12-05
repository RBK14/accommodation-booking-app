import { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Box, Card, CardContent } from '@mui/material';
import {
  ListingDetailsSection,
  ListingGallerySection,
  ReservationsSection,
  ReviewsSection,
} from '../../components/ui/host';

const HostListingPage = () => {
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

  // Opinie (przykładowe)
  const [reviews, setReviews] = useState([
    {
      id: 1,
      rating: 5,
      comment:
        'Wspaniały apartament! Dokładnie tak jak na zdjęciach. Lokalizacja idealna, blisko centrum. Gospodarz bardzo pomocny i komunikatywny.',
      guestName: 'Anna Kowalska',
      date: '2024-11-15',
    },
    {
      id: 2,
      rating: 4,
      comment:
        'Bardzo dobry apartament, czysto i przytulnie. Jedyny minus to brak klimatyzacji w lecie, ale poza tym polecam!',
      guestName: 'Jan Nowak',
      date: '2024-10-28',
    },
    {
      id: 3,
      rating: 5,
      comment:
        'Rewelacja! Apartament w super stanie, świetnie wyposażona kuchnia. Widok z okna przepiękny. Na pewno wrócę!',
      guestName: 'Katarzyna Wiśniewska',
      date: '2024-10-10',
    },
    {
      id: 4,
      rating: 3,
      comment:
        'Apartament w porządku, ale trochę głośno przez ulicę. Dobre miejsce na krótki pobyt.',
      guestName: 'Piotr Lewandowski',
      date: '2024-09-22',
    },
    {
      id: 5,
      rating: 3,
      comment:
        'Apartament w porządku, ale trochę głośno przez ulicę. Dobre miejsce na krótki pobyt.',
      guestName: 'Piotr Lewandowski',
      date: '2024-09-22',
    },
    {
      id: 6,
      rating: 3,
      comment:
        'Apartament w porządku, ale trochę głośno przez ulicę. Dobre miejsce na krótki pobyt.',
      guestName: 'Piotr Lewandowski',
      date: '2024-09-22',
    },
    {
      id: 7,
      rating: 3,
      comment:
        'Apartament w porządku, ale trochę głośno przez ulicę. Dobre miejsce na krótki pobyt.',
      guestName: 'Piotr Lewandowski',
      date: '2024-09-22',
    },
  ]);

  // Rezerwacje (przykładowe)
  const [reservations, setReservations] = useState([
    {
      id: 1,
      country: 'Polska',
      city: 'Warszawa',
      postalCode: '00-545',
      street: 'ul. Marszałkowska',
      buildingNumber: '50',
      pricePerDay: 250,
      totalPrice: 1500,
      currency: 'EUR',
      status: 'Potwierdzona',
      checkIn: '2024-12-15',
      checkOut: '2024-12-21',
    },
    {
      id: 2,
      country: 'Polska',
      city: 'Warszawa',
      postalCode: '00-545',
      street: 'ul. Marszałkowska',
      buildingNumber: '50',
      pricePerDay: 250,
      totalPrice: 750,
      currency: 'EUR',
      status: 'Oczekująca',
      checkIn: '2024-12-25',
      checkOut: '2024-12-28',
    },
    {
      id: 3,
      country: 'Polska',
      city: 'Warszawa',
      postalCode: '00-545',
      street: 'ul. Marszałkowska',
      buildingNumber: '50',
      pricePerDay: 250,
      totalPrice: 2000,
      currency: 'EUR',
      status: 'Zakończona',
      checkIn: '2024-11-01',
      checkOut: '2024-11-09',
    },
  ]);

  const handleEdit = () => {
    navigate(`/host/listing/${id}/edit`, { state: { listing, images } });
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
            <ListingDetailsSection listing={listing} onEdit={handleEdit} onDelete={handleDelete} />
            <ListingGallerySection images={images} />
          </Box>
        </CardContent>
      </Card>

      <ReservationsSection reservations={reservations} />
      <ReviewsSection reviews={reviews} />
    </Box>
  );
};

export default HostListingPage;
