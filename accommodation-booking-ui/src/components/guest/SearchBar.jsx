import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, TextField, Button, Paper, InputAdornment } from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import PeopleIcon from '@mui/icons-material/People';
import { PRIMARY_BLUE } from '../../assets/styles/colors';

const SearchBar = ({ initialLocation = '', initialGuests = '', compact = false }) => {
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useState({
    location: initialLocation,
    guests: initialGuests,
  });

  const handleChange = (field) => (event) => {
    setSearchParams({
      ...searchParams,
      [field]: event.target.value,
    });
  };

  const handleSearch = () => {
    const queryParams = new URLSearchParams();

    if (searchParams.location) queryParams.append('location', searchParams.location);
    if (searchParams.guests) queryParams.append('guests', searchParams.guests);

    navigate(`/listings?${queryParams.toString()}`);
  };

  return (
    <Paper
      elevation={compact ? 2 : 3}
      sx={{
        padding: compact ? 2 : 3,
        borderRadius: 2,
        backgroundColor: 'white',
      }}
    >
      <Box
        sx={{
          display: 'flex',
          gap: compact ? 1.5 : 2,
          flexWrap: 'wrap',
          alignItems: 'flex-end',
        }}
      >
        <TextField
          label="Lokalizacja"
          placeholder="Dokąd się wybierasz?"
          value={searchParams.location}
          onChange={handleChange('location')}
          sx={{ flex: 1, minWidth: '200px' }}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <LocationOnIcon />
              </InputAdornment>
            ),
          }}
        />

        <TextField
          label="Goście"
          type="number"
          placeholder="Liczba gości"
          value={searchParams.guests}
          onChange={handleChange('guests')}
          sx={{ flex: 1, minWidth: '150px' }}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <PeopleIcon />
              </InputAdornment>
            ),
            inputProps: {
              min: 1,
              max: 20,
            },
          }}
        />

        <Button
          variant="contained"
          size={compact ? 'medium' : 'large'}
          startIcon={<SearchIcon />}
          onClick={handleSearch}
          sx={{
            backgroundColor: PRIMARY_BLUE,
            minWidth: compact ? '120px' : '150px',
            height: compact ? '48px' : '56px',
            '&:hover': {
              backgroundColor: '#0a58ca',
            },
          }}
        >
          Szukaj
        </Button>
      </Box>
    </Paper>
  );
};

export default SearchBar;
