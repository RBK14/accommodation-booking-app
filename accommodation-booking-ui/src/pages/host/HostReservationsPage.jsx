import { useState } from 'react';
import { Box } from '@mui/material';
import { ReservationsSection } from '../../components/host';

const HostReservationsPage = () => {
  // Przykładowe rezerwacje ze wszystkich ogłoszeń
  const [reservations, setReservations] = useState([
    {
      id: 1,
      title: 'Apartament w centrum miasta',
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
      title: 'Apartament w centrum miasta',
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
      title: 'Domek nad jeziorem',
      country: 'Polska',
      city: 'Nowa Wieś',
      postalCode: '87-148',
      street: 'Stęczno',
      buildingNumber: '12',
      pricePerDay: 300,
      totalPrice: 2100,
      currency: 'PLN',
      status: 'Potwierdzona',
      checkIn: '2024-12-20',
      checkOut: '2024-12-27',
    },
    {
      id: 4,
      title: 'Apartament w centrum miasta',
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
    {
      id: 5,
      title: 'Luksusowy penthouse',
      country: 'Polska',
      city: 'Warszawa',
      postalCode: '02-201',
      street: 'al. Jerozolimskie',
      buildingNumber: '96',
      pricePerDay: 500,
      totalPrice: 2500,
      currency: 'EUR',
      status: 'Potwierdzona',
      checkIn: '2024-12-18',
      checkOut: '2024-12-23',
    },
    {
      id: 6,
      title: 'Przytulny studio z widokiem',
      country: 'Polska',
      city: 'Kraków',
      postalCode: '31-009',
      street: 'ul. Szewska',
      buildingNumber: '10',
      pricePerDay: 180,
      totalPrice: 540,
      currency: 'PLN',
      status: 'Anulowana',
      checkIn: '2024-12-10',
      checkOut: '2024-12-13',
    },
    {
      id: 7,
      title: 'Willa na wsi z ogrodem',
      country: 'Polska',
      city: 'Chrzanów',
      postalCode: '43-500',
      street: 'Gostchorze',
      buildingNumber: '5',
      pricePerDay: 400,
      totalPrice: 2800,
      currency: 'PLN',
      status: 'Zakończona',
      checkIn: '2024-10-15',
      checkOut: '2024-10-22',
    },
  ]);

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
