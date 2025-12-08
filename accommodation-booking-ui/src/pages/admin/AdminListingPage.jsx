import { useState } from 'react';
import { useNavigate, useParams, useLocation } from 'react-router-dom';
import { Box, Card, CardContent } from '@mui/material';
import { ListingGallerySection, ReservationsSection, ReviewsSection } from '../../components/host';
import { ListingDetailsSection } from '../../components/shared';

const AdminListingPage = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const location = useLocation();

  const [listing, setListing] = useState(
    location.state?.listing || {
      id: id,
      title: 'Brak danych (odśwież stronę przez listę)',
      description: '',
      accommodationType: '',
      beds: 0,
      maxGuests: 0,
      country: '',
      city: '',
      street: '',
      buildingNumber: '',
      amountPerDay: 0,
      currency: 'PLN',
    }
  );

  const [images, setImages] = useState(location.state?.images || []);

  const handleEdit = () => {
    navigate(`/admin/listing/${id}/edit`, { state: { listing, images } });
  };

  const handleDelete = () => {
    console.log(`Usuwanie ogłoszenia: ${id}`);
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

      <ReservationsSection />
      <ReviewsSection />
    </Box>
  );
};

export default AdminListingPage;
