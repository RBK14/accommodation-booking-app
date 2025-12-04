import { useNavigate, Outlet } from 'react-router-dom';
import { useContext, useState } from 'react';
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
import AddIcon from '@mui/icons-material/Add';
import DateRangeIcon from '@mui/icons-material/DateRange';
import StarIcon from '@mui/icons-material/Star';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import LogoutIcon from '@mui/icons-material/Logout';
import AuthContext from '../context/AuthProvider';
import { PRIMARY_BLUE, DARK_GRAY, LIGHT_GRAY, TEXT_WHITE } from '../assets/styles/colors';

const DRAWER_WIDTH = 240;

const HostLayout = () => {
  const navigate = useNavigate();
  const { auth, logout } = useContext(AuthContext);
  const [anchorEl, setAnchorEl] = useState(null);

  const menuItems = [
    { label: 'Oferty', icon: <HomeIcon />, path: '/host' },
    { label: 'Nowa oferta', icon: <AddIcon />, path: '/host/new-offer' },
    { label: 'Rezerwacje', icon: <DateRangeIcon />, path: '/host/reservations' },
    { label: 'Opinie', icon: <StarIcon />, path: '/host/review' },
  ];

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
    navigate('/login');
  };

  return (
    <Box sx={{ display: 'flex', minHeight: '100vh', backgroundColor: LIGHT_GRAY }}>
      {/* AppBar */}
      <AppBar
        position="fixed"
        sx={{
          width: '100%',
          backgroundColor: PRIMARY_BLUE,
          zIndex: (theme) => theme.zIndex.drawer + 1,
        }}
      >
        <Toolbar sx={{ display: 'flex', justifyContent: 'space-between' }}>
          <Box
            sx={{
              fontSize: '1.5rem',
              fontWeight: 'bold',
              fontFamily: ['Roboto', 'Arial', 'sans-serif'].join(','),
            }}
          >
            Panel gospodarza
          </Box>

          {/* Profile Menu */}
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
            <Box
              sx={{
                color: TEXT_WHITE,
                fontSize: '0.95rem',
                fontFamily: ['Roboto', 'Arial', 'sans-serif'].join(','),
              }}
            >
              {auth?.firstName}
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
          </Box>
        </Toolbar>
      </AppBar>

      {/* Sidebar Drawer */}
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

      {/* Main Content */}
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
        <Container maxWidth="lg" sx={{ py: 3 }}>
          <Outlet />
        </Container>
      </Box>
    </Box>
  );
};

export default HostLayout;
