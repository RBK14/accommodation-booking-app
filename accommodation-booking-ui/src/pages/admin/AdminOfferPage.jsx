import { useState } from 'react';
import { useNavigate, useParams, useLocation } from 'react-router-dom';
import { Box, Card, CardContent } from '@mui/material';
import {
    OfferGallerySection,
    ReservationsSection,
    ReviewsSection,
} from '../../components/ui/host';
import { OfferDetailsSection } from '../../components/ui/admin';

const AdminOfferPage = () => {
    const navigate = useNavigate();
    const { id } = useParams();
    const location = useLocation();

    // POPRAWKA: Przywracamy 'listing' zamiast 'formData'.
    // Inicjalizujemy od razu z location.state (jeśli istnieje) lub pustym obiektem/domyślnymi danymi (na wypadek odświeżenia strony).
    const [listing, setListing] = useState(location.state?.listing || {
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
    });

    // Galeria zdjęć
    const [images, setImages] = useState(location.state?.images || []);

    const handleEdit = () => {
        // Przekazujemy obecny stan listing i images do edycji
        navigate(`/admin/offer/${id}/edit`, { state: { listing, images } });
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
                        {/* TERAZ TO ZADZIAŁA: zmienna listing istnieje */}
                        <OfferDetailsSection
                            listing={listing}
                            onEdit={handleEdit}
                            onDelete={handleDelete}
                        />
                        <OfferGallerySection images={images} />
                    </Box>
                </CardContent>
            </Card>

            <ReservationsSection />
            <ReviewsSection />
        </Box>
    );
};

export default AdminOfferPage;