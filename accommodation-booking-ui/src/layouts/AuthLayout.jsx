import { Outlet } from 'react-router-dom';
import { Box, Container } from '@mui/material';
import { Footer } from '../components/shared';
import { LIGHT_GRAY } from '../assets/styles/colors';

const AuthLayout = () => {
  return (
    <Box
      sx={{
        minHeight: '100vh',
        display: 'flex',
        flexDirection: 'column',
        background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
      }}
    >
      <Box
        component="main"
        sx={{
          flex: 1,
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          padding: 3,
        }}
      >
        <Container maxWidth="sm">
          <Outlet />
        </Container>
      </Box>

      <Footer />
    </Box>
  );
};

export default AuthLayout;
