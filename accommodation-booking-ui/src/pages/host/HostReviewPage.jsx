import { useState } from 'react';
import { Box } from '@mui/material';
import { ReviewsSection } from '../../components/ui/host';

const HostReviewPage = () => {
  const [reviews, setReviews] = useState([
    {
      id: 1,
      rating: 5,
      comment:
        'Wspaniały apartament! Dokładnie tak jak na zdjęciach. Lokalizacja idealna, blisko centrum. Gospodarz bardzo pomocny i komunikatywny.',
      guestName: 'Anna Kowalska',
      date: '2024-11-15',
      listingTitle: 'Apartament w centrum miasta',
    },
    {
      id: 2,
      rating: 4,
      comment:
        'Bardzo dobry apartament, czysto i przytulnie. Jedyny minus to brak klimatyzacji w lecie, ale poza tym polecam!',
      guestName: 'Jan Nowak',
      date: '2024-10-28',
      listingTitle: 'Apartament w centrum miasta',
    },
    {
      id: 3,
      rating: 5,
      comment: 'Piękny domek, idealne miejsce na weekend. Cisza i spokój, polecam!',
      guestName: 'Maria Zielińska',
      date: '2024-11-20',
      listingTitle: 'Domek nad jeziorem',
    },
    {
      id: 4,
      rating: 5,
      comment:
        'Rewelacja! Apartament w super stanie, świetnie wyposażona kuchnia. Widok z okna przepiękny. Na pewno wrócę!',
      guestName: 'Katarzyna Wiśniewska',
      date: '2024-10-10',
      listingTitle: 'Apartament w centrum miasta',
    },
    {
      id: 5,
      rating: 3,
      comment:
        'Apartament w porządku, ale trochę głośno przez ulicę. Dobre miejsce na krótki pobyt.',
      guestName: 'Piotr Lewandowski',
      date: '2024-09-22',
      listingTitle: 'Apartament w centrum miasta',
    },
    {
      id: 6,
      rating: 4,
      comment: 'Ładne studio, wszystko zgodne z opisem. Dobra komunikacja z właścicielem.',
      guestName: 'Tomasz Kaczmarek',
      date: '2024-11-05',
      listingTitle: 'Przytulny studio z widokiem',
    },
    {
      id: 7,
      rating: 5,
      comment: 'Luksusowy penthouse, wszystko na najwyższym poziomie. Widok zapiera dech!',
      guestName: 'Agnieszka Dąbrowska',
      date: '2024-10-18',
      listingTitle: 'Luksusowy penthouse',
    },
    {
      id: 8,
      rating: 4,
      comment: 'Spokojne miejsce nad jeziorem, idealne na relaks. Polecam rodzinom z dziećmi.',
      guestName: 'Michał Woźniak',
      date: '2024-09-30',
      listingTitle: 'Domek nad jeziorem',
    },
    {
      id: 9,
      rating: 5,
      comment: 'Willa z pięknym ogrodem, wszystko czyste i zadbane. Wspaniały pobyt!',
      guestName: 'Ewa Jabłońska',
      date: '2024-11-12',
      listingTitle: 'Willa na wsi z ogrodem',
    },
    {
      id: 10,
      rating: 4,
      comment: 'Apartament nad morzem, świetna lokalizacja. Tylko parę minut do plaży.',
      guestName: 'Paweł Mazur',
      date: '2024-08-25',
      listingTitle: 'Nowoczesny apartament nad morzem',
    },
  ]);

  return (
    <Box sx={{ p: 3 }}>
      <ReviewsSection reviews={reviews} showListingTitle={true} title="Wszystkie opinie" />
    </Box>
  );
};

export default HostReviewPage;
