import { useContext, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { AppBar, Toolbar, Box, Button, IconButton, Menu, MenuItem } from '@mui/material';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import LogoutIcon from '@mui/icons-material/Logout';
import AuthContext from '../../context/AuthProvider';
import { PRIMARY_BLUE, TEXT_WHITE } from '../../assets/styles/colors';

const Navbar = () => {
  const navigate = useNavigate();
  const { auth, userData, logout } = useContext(AuthContext);
  const [anchorEl, setAnchorEl] = useState(null);

  const handleMenuOpen = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleAccountClick = () => {
    if (auth?.role === 'Guest') {
      navigate('/account');
    } else if (auth?.role === 'Host') {
      navigate('/host/account');
    } else if (auth?.role === 'Admin') {
      navigate('/admin/account');
    }
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
                {userData?.firstName}
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
  );
};

export default Navbar;
