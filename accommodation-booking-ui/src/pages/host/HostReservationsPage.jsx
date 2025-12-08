import { useState, useEffect } from 'react';
import { Box, CircularProgress, Alert } from '@mui/material';
import { ReservationsSection } from '../../components/host';
import { useAuth, useReservationsApi } from '../../hooks';

const HostReservationsPage = () => {
  const { auth, userData } = useAuth();
  const { getReservations, loading, error } = useReservationsApi();
  const [reservations, setReservations] = useState([]);

  useEffect(() => {
    const fetchReservations = async () => {
      if (!userData?.profileId || !auth?.token) return;

      const result = await getReservations({ hostProfileId: userData.profileId }, auth.token);
      if (result.success) {
        setReservations(result.data);
      }
    };

    fetchReservations();
  }, [userData?.profileId, auth?.token]);

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
        reservations={reservations}
        showListingTitle={true}
        title="Wszystkie rezerwacje"
      />
    </Box>
  );
};

export default HostReservationsPage;
