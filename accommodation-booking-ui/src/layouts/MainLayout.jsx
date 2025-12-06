import { Outlet } from 'react-router-dom';
import { Box } from '@mui/material';
import Navbar from '../components/shared/Navbar';
import Footer from '../components/shared/Footer';
import { LIGHT_GRAY } from '../assets/styles/colors';

const MainLayout = () => {
  return (
    <Box
      sx={{
        minHeight: '100vh',
        display: 'flex',
        flexDirection: 'column',
      }}
    >
      <Navbar />

      <Box
        component="main"
        sx={{
          flex: 1,
          backgroundColor: LIGHT_GRAY,
        }}
      >
        <Outlet />
      </Box>

      <Footer />
    </Box>
  );
};

export default MainLayout;
