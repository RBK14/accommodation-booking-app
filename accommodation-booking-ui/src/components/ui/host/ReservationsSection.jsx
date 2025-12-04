import { Card, CardContent, Typography } from '@mui/material';

const ReservationsSection = () => {
  return (
    <Card sx={{ mb: 3 }}>
      <CardContent>
        <Typography variant="h6" sx={{ mb: 2, fontWeight: 'bold' }}>
          Rezerwacje
        </Typography>
        <Typography variant="body2" sx={{ color: 'textSecondary' }}>
          Rezerwacje
        </Typography>
      </CardContent>
    </Card>
  );
};

export default ReservationsSection;
