import { Outlet } from 'react-router-dom';
import { Box } from '@mui/material';
import { Footer, Navbar } from '../components';
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
