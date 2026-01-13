import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Box, Card, CardContent, CircularProgress, Alert } from '@mui/material';
import { ReservationsSection, ReviewsSection } from '../../components/host';
import { ListingDetailsSection, ListingGallerySection } from '../../components/shared';
import { useAuth, useListingsApi, useReservationsApi, useReviewsApi } from '../../hooks';
import { toast } from 'react-toastify';

const AdminListingPage = () => {
    const navigate = useNavigate();
    const { id } = useParams();
    const { auth } = useAuth();
    const {
        getListing,
        deleteListing,
        loading: listingLoading,
        error: listingError,
    } = useListingsApi();
    const { getReservations, loading: reservationsLoading } = useReservationsApi();
    const { getReviews, loading: reviewsLoading } = useReviewsApi();

    const [listing, setListing] = useState(null);
    const [images, setImages] = useState([]);
    const [reviews, setReviews] = useState([]);
    const [reservations, setReservations] = useState([]);

    const fetchData = async () => {
        if (!auth?.token || !id) return;

        // Pobierz szczegóły ogłoszenia
        const listingResult = await getListing(id, auth.token);
        if (listingResult.success) {
            setListing(listingResult.data);
            // TODO: Pobierz zdjęcia gdy będzie endpoint
            setImages([]);
        }

        // Pobierz rezerwacje dla tego ogłoszenia
        const reservationsResult = await getReservations({ listingId: id }, auth.token);
        if (reservationsResult.success) {
            setReservations(reservationsResult.data);
        }

        // Pobierz opinie dla tego ogłoszenia
        const reviewsResult = await getReviews({ listingId: id }, auth.token);
        if (reviewsResult.success) {
            setReviews(reviewsResult.data);
        }
    };

    useEffect(() => {
        fetchData();
    }, [id, auth?.token]);

    const handleEdit = () => {
        navigate(`/admin/listing/${id}/edit`);
    };

    const handleDelete = async () => {
        if (!window.confirm('Czy na pewno chcesz usunąć to ogłoszenie?')) {
            return;
        }

        const result = await deleteListing(id, auth.token);
        if (result.success) {
            toast.success('Ogłoszenie zostało usunięte');
            navigate('/admin/listings');
        } else {
            toast.error(result.error || 'Nie udało się usunąć ogłoszenia');
        }
    };

    const handleReviewUpdated = () => {
        // Odśwież listę opinii po aktualizacji
        fetchData();
    };

    const handleReviewDeleted = () => {
        // Odśwież listę opinii po usunięciu
        fetchData();
    };

    if (listingLoading || reservationsLoading || reviewsLoading) {
        return (
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', py: 10 }}>
                <CircularProgress />
            </Box>
        );
    }

    if (listingError) {
        return (
            <Box sx={{ p: 3 }}>
                <Alert severity="error">{listingError}</Alert>
            </Box>
        );
    }

    if (!listing) {
        return (
            <Box sx={{ p: 3 }}>
                <Alert severity="info">Nie znaleziono ogłoszenia</Alert>
            </Box>
        );
    }

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

            <ReservationsSection 
                reservations={reservations} 
                title="Rezerwacje tego ogłoszenia" 
                showListingTitle={false} 
            />
            <ReviewsSection 
                reviews={reviews} 
                title="Opinie" 
                showListingTitle={false}
                allowEdit={true}  // WŁĄCZ EDYCJĘ dla Admina
                onReviewUpdated={handleReviewUpdated}  // Callback po aktualizacji
                onReviewDeleted={handleReviewDeleted}  // Callback po usunięciu
            />
        </Box>
    );
};

export default AdminListingPage;