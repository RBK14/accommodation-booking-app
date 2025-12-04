import { Card, CardContent, Typography } from '@mui/material';

const ReviewsSection = () => {
  return (
    <Card>
      <CardContent>
        <Typography variant="h6" sx={{ mb: 2, fontWeight: 'bold' }}>
          Opinie
        </Typography>
        <Typography variant="body2" sx={{ color: 'textSecondary' }}>
          Opinie
        </Typography>
      </CardContent>
    </Card>
  );
};

export default ReviewsSection;
