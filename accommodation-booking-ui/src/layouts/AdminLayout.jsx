import { useState, useContext } from 'react';
import { useNavigate, Outlet } from 'react-router-dom';
import {
  AppBar,
  Toolbar,
  Drawer,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  Box,
  Container,
  IconButton,
  Menu,
  MenuItem,
} from '@mui/material';

import HomeIcon from '@mui/icons-material/Home';
import PeopleIcon from '@mui/icons-material/People';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import LogoutIcon from '@mui/icons-material/Logout';

// Logika i style
import { AuthContext } from '../context';
import { PRIMARY_BLUE, DARK_GRAY, LIGHT_GRAY, TEXT_WHITE } from '../assets/styles/colors';

const DRAWER_WIDTH = 240;

const AdminLayout = () => {
  const navigate = useNavigate();
  const { auth, userData, logout } = useContext(AuthContext);
  const [anchorEl, setAnchorEl] = useState(null);

  // --- DEFINICJA MENU BOCZNEGO ---
  const menuItems = [
    {
      label: 'Moje konto',
      icon: <AccountCircleIcon />,
      path: '/admin/account',
    },
    {
      label: 'Użytkownicy',
      icon: <PeopleIcon />,
      path: '/admin/users',
    },
    {
      label: 'Ogłoszenia',
      icon: <HomeIcon />,
      path: '/admin/listings',
    },
  ];

  const handleMenuOpen = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleHomeClick = () => {
    navigate('/');
    handleMenuClose();
  };

  const handleLogout = () => {
    logout();
    handleMenuClose();
    navigate('/login');
  };

  return (
    <Box sx={{ display: 'flex', minHeight: '100vh', backgroundColor: LIGHT_GRAY }}>
      {/* --- APP BAR (TOP BAR) --- */}
      <AppBar
        position="fixed"
        sx={{
          width: '100%',
          backgroundColor: PRIMARY_BLUE,
          zIndex: (theme) => theme.zIndex.drawer + 1,
        }}
      >
        <Toolbar sx={{ display: 'flex', justifyContent: 'space-between' }}>
          {/* Panel Title */}
          <Box
            sx={{
              fontSize: '1.5rem',
              fontWeight: 'bold',
              fontFamily: ['Roboto', 'Arial', 'sans-serif'].join(','),
              cursor: 'pointer',
              '&:hover': {
                opacity: 0.8,
              },
            }}
            onClick={handleHomeClick}
          >
            Panel Administratora
          </Box>

          {/* User Menu (Right side) */}
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
            <Box
              sx={{
                color: TEXT_WHITE,
                fontSize: '0.95rem',
                fontFamily: ['Roboto', 'Arial', 'sans-serif'].join(','),
              }}
            >
              {userData?.firstName || 'Admin'}
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
              <MenuItem onClick={handleHomeClick} sx={{ gap: 1 }}>
                <HomeIcon fontSize="small" />
                Strona główna
              </MenuItem>
              <MenuItem onClick={handleLogout} sx={{ gap: 1 }}>
                <LogoutIcon fontSize="small" />
                Wyloguj się
              </MenuItem>
            </Menu>
          </Box>
        </Toolbar>
      </AppBar>

      {/* --- SIDEBAR (LEWE MENU) --- */}
      <Drawer
        variant="permanent"
        sx={{
          width: DRAWER_WIDTH,
          flexShrink: 0,
          '& .MuiDrawer-paper': {
            width: DRAWER_WIDTH,
            boxSizing: 'border-box',
            backgroundColor: DARK_GRAY,
            color: TEXT_WHITE,
            marginTop: '64px',
            overflowX: 'hidden',
          },
        }}
      >
        <List sx={{ pt: 2 }}>
          {menuItems.map((item, index) => (
            <ListItem
              button
              key={index}
              onClick={() => navigate(item.path)}
              sx={{
                justifyContent: 'flex-start',
                minHeight: '56px',
                px: 2,
                py: 2,
                my: 0.5,
                gap: 2,
                '&:hover': {
                  backgroundColor: `rgba(13, 110, 253, 0.1)`,
                  borderLeft: `6px solid ${PRIMARY_BLUE}`,
                },
                cursor: 'pointer',
              }}
            >
              <ListItemIcon
                sx={{
                  color: TEXT_WHITE,
                  minWidth: '40px',
                  justifyContent: 'center',
                }}
              >
                {item.icon}
              </ListItemIcon>
              <ListItemText
                primary={item.label}
                sx={{
                  '& .MuiTypography-root': {
                    fontSize: '0.95rem',
                    whiteSpace: 'nowrap',
                    overflow: 'hidden',
                    textOverflow: 'ellipsis',
                  },
                }}
              />
            </ListItem>
          ))}
        </List>
      </Drawer>

      {/* --- MAIN CONTENT --- */}
      <Box
        component="main"
        sx={{
          flexGrow: 1,
          backgroundColor: LIGHT_GRAY,
          marginTop: '64px',
          overflow: 'auto',
          minHeight: 'calc(100vh - 64px)',
        }}
      >
        <Container maxWidth="xl" sx={{ py: 3 }}>
          <Outlet />
        </Container>
      </Box>
    </Box>
  );
};

export default AdminLayout;
