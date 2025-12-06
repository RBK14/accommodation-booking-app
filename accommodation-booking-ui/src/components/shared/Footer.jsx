import { Box } from '@mui/material';
import { DARK_GRAY, TEXT_WHITE } from '../../assets/styles/colors';

const Footer = () => {
  return (
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
  );
};

export default Footer;
