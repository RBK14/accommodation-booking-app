import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, CircularProgress, Alert, Button } from '@mui/material';
import RateReviewIcon from '@mui/icons-material/RateReview';
import EditIcon from '@mui/icons-material/Edit';
import { ReservationsSection } from '../../components/host';
import { PRIMARY_BLUE } from '../../assets/styles/colors';
import { useAuth, useReservationsApi, useReviewsApi } from '../../hooks';

const GuestReservationsPage = () => {
    const navigate = useNavigate();
    const { auth, userData } = useAuth();
    const { getReservations, loading, error } = useReservationsApi();
    const { getReviews } = useReviewsApi();
    const [reservations, setReservations] = useState([]);
    const [reviews, setReviews] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            if (!userData?.profileId || !auth?.token) return;

            // Pobierz rezerwacje
            const reservationsResult = await getReservations(
                { guestProfileId: userData.profileId },
                auth.token
            );
            if (reservationsResult.success) {
                setReservations(reservationsResult.data);
            }

            // Pobierz opinie gościa
            const reviewsResult = await getReviews({ guestProfileId: userData.profileId }, auth.token);
            if (reviewsResult.success) {
                setReviews(reviewsResult.data);
            }
        };

        fetchData();
    }, [userData?.profileId, auth?.token]);

    const isCompleted = (status) => {
        return status?.toLowerCase() === 'completed';
    };

    const hasReview = (listingId) => {
        return reviews.some((review) => review.listingId === listingId);
    };

    const getReviewForListing = (listingId) => {
        return reviews.find((review) => review.listingId === listingId);
    };

    const handleCreateOrEditReview = (reservationId, reservation) => {
        const existingReview = getReviewForListing(reservation.listingId);
        navigate(`/guest/review/${reservationId}`, {
            state: {
                reservation,
                existingReview: existingReview || null
            },
        });
    };

    // Dodaj przycisk "Wystaw opinię" lub "Edytuj opinię" do zakończonych rezerwacji
    const reservationsWithActions = reservations.map((reservation) => {
        const hasExistingReview = hasReview(reservation.listingId);

        return {
            ...reservation,
            customAction: isCompleted(reservation.status) ? (
                <Button
                    fullWidth
                    variant={hasExistingReview ? 'outlined' : 'contained'}
                    size="small"
                    startIcon={hasExistingReview ? <EditIcon /> : <RateReviewIcon />}
                    sx={{
                        backgroundColor: hasExistingReview ? 'transparent' : PRIMARY_BLUE,
                        borderColor: hasExistingReview ? PRIMARY_BLUE : 'transparent',
                        color: hasExistingReview ? PRIMARY_BLUE : 'white',
                        '&:hover': {
                            backgroundColor: hasExistingReview ? 'rgba(13, 110, 253, 0.04)' : '#0a58ca',
                            borderColor: PRIMARY_BLUE,
                        },
                    }}
                    onClick={() => handleCreateOrEditReview(reservation.id, reservation)}
                >
                    {hasExistingReview ? 'Edytuj opinię' : 'Wystaw opinię'}
                </Button>
            ) : null,
        };
    });

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

    return (
        <Box sx={{ p: 3 }}>
            <ReservationsSection
                reservations={reservationsWithActions}
                showListingTitle={true}
                title="Moje rezerwacje"
            />
        </Box>
    );
};

export default GuestReservationsPage;