import React, { useContext, useState } from 'react';
import { Outlet, useNavigate } from 'react-router-dom';
import { AppBar, Toolbar, Box, Button, IconButton, Menu, MenuItem } from '@mui/material';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import LogoutIcon from '@mui/icons-material/Logout';
import AuthContext from '../context/AuthProvider';
import { PRIMARY_BLUE, DARK_GRAY, LIGHT_GRAY, TEXT_WHITE } from '../assets/styles/colors';

const MainLayout = () => {
  const navigate = useNavigate();
  const { auth, logout } = useContext(AuthContext);
  const [anchorEl, setAnchorEl] = useState(null);

  const handleMenuOpen = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleAccountClick = () => {
    navigate('/host/account');
    handleMenuClose();
  };

  const handleLogout = () => {
    logout();
    handleMenuClose();
    navigate('/');
  };

  const handleHomeClick = () => {
    navigate('/');
  };

  return (
    <Box
      sx={{
        minHeight: '100vh',
        display: 'flex',
        flexDirection: 'column',
      }}
    >
      {/* Navbar */}
      <AppBar
        position="static"
        sx={{
          backgroundColor: PRIMARY_BLUE,
        }}
      >
        <Toolbar sx={{ display: 'flex', justifyContent: 'space-between' }}>
          {/* Logo/Brand */}
          <Box
            onClick={handleHomeClick}
            sx={{
              fontSize: '1.5rem',
              fontWeight: 'bold',
              fontFamily: ['Roboto', 'Arial', 'sans-serif'].join(','),
              color: TEXT_WHITE,
              cursor: 'pointer',
              '&:hover': {
                opacity: 0.8,
              },
            }}
          >
            Hostly
          </Box>

          {/* Navigation */}
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
            {auth ? (
              <>
                <Box
                  sx={{
                    color: TEXT_WHITE,
                    fontSize: '0.95rem',
                    fontFamily: ['Roboto', 'Arial', 'sans-serif'].join(','),
                  }}
                >
                  {auth.firstName}
                </Box>
                <IconButton
                  onClick={handleMenuOpen}
                  sx={{
                    color: TEXT_WHITE,
                    '&:hover': {
                      backgroundColor: 'rgba(255, 255, 255, 0.1)',
                    },
                  }}
                >
                  <AccountCircleIcon fontSize="large" />
                </IconButton>
                <Menu
                  anchorEl={anchorEl}
                  open={Boolean(anchorEl)}
                  onClose={handleMenuClose}
                  anchorOrigin={{
                    vertical: 'bottom',
                    horizontal: 'right',
                  }}
                  transformOrigin={{
                    vertical: 'top',
                    horizontal: 'right',
                  }}
                >
                  <MenuItem onClick={handleAccountClick} sx={{ gap: 1 }}>
                    <AccountCircleIcon fontSize="small" />
                    Moje konto
                  </MenuItem>
                  <MenuItem onClick={handleLogout} sx={{ gap: 1 }}>
                    <LogoutIcon fontSize="small" />
                    Wyloguj się
                  </MenuItem>
                </Menu>
              </>
            ) : (
              <>
                <Button
                  color="inherit"
                  onClick={() => navigate('/login')}
                  sx={{
                    '&:hover': {
                      backgroundColor: 'rgba(255, 255, 255, 0.1)',
                    },
                  }}
                >
                  Logowanie
                </Button>
                <Button
                  color="inherit"
                  onClick={() => navigate('/register')}
                  sx={{
                    '&:hover': {
                      backgroundColor: 'rgba(255, 255, 255, 0.1)',
                    },
                  }}
                >
                  Rejestracja
                </Button>
              </>
            )}
          </Box>
        </Toolbar>
      </AppBar>

      {/* Main Content */}
      <Box
        component="main"
        sx={{
          flex: 1,
          backgroundColor: LIGHT_GRAY,
          padding: '2rem 1.5rem',
        }}
      >
        <Outlet />
      </Box>

      {/* Footer */}
      <Box
        component="footer"
        sx={{
          backgroundColor: DARK_GRAY,
          color: TEXT_WHITE,
          padding: '0.75rem 1.5rem',
          textAlign: 'center',
          fontFamily: ['Roboto', 'Arial', 'sans-serif'].join(','),
        }}
      >
        © 2025 Hostly. Wszystkie prawa zastrzeżone.
      </Box>
    </Box>
  );
};

export default MainLayout;
