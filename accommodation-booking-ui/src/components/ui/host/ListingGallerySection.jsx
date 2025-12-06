import { Box, Typography, ImageList, ImageListItem } from '@mui/material';
import { DARK_GRAY } from '../../../assets/styles/colors';

const ListingGallerySection = ({ images }) => {
  return (
    <Box sx={{ flex: 1, display: 'flex', flexDirection: 'column' }}>
      <Typography variant="subtitle2" sx={{ color: DARK_GRAY, fontWeight: 'bold', mb: 2 }}>
        Galeria zdjęć ({images.length})
      </Typography>
      <Box sx={{ height: '75vh', overflowY: 'auto', overflowX: 'hidden' }}>
        <ImageList
          variant="masonry"
          cols={2}
          gap={8}
          sx={{ width: '100%', minWidth: '400px', margin: 0 }}
        >
          {images.map((image, index) => (
            <ImageListItem key={index}>
              <img
                srcSet={`${image}?w=248&fit=crop&auto=format&dpr=2 2x`}
                src={`${image}?w=248&fit=crop&auto=format`}
                alt={`Zdjęcie ${index + 1}`}
                loading="lazy"
                style={{
                  borderRadius: '8px',
                }}
              />
            </ImageListItem>
          ))}
        </ImageList>
      </Box>
    </Box>
  );
};

export default ListingGallerySection;
