/**
 * Mapa konwersji typów zakwaterowania z angielskiego na polski
 */
const ACCOMMODATION_TYPE_MAP = {
  Apartment: 'Apartament',
  House: 'Dom',
  Hotel: 'Pokój hotelowy',
  Villa: 'Willa',
  Room: 'Pokój',
  Loft: 'Loft',
  Studio: 'Studio',
};

/**
 * Konwertuje typ zakwaterowania z angielskiego na polski
 * @param {string} type - Typ zakwaterowania w języku angielskim
 * @returns {string} Typ zakwaterowania w języku polskim
 */
export const translateAccommodationType = (type) => {
  return ACCOMMODATION_TYPE_MAP[type] || type;
};

/**
 * Zwraca odwrotne mapowanie - z polskiego na angielski
 * @param {string} type - Typ zakwaterowania w języku polskim
 * @returns {string} Typ zakwaterowania w języku angielskim
 */
export const reverseTranslateAccommodationType = (type) => {
  const reverseMap = Object.fromEntries(
    Object.entries(ACCOMMODATION_TYPE_MAP).map(([en, pl]) => [pl, en])
  );
  return reverseMap[type] || type;
};

/**
 * Zwraca wszystkie dostępne typy zakwaterowania (angielskie klucze)
 * @returns {string[]} Tablica z typami zakwaterowania w języku angielskim
 */
export const getAccommodationTypes = () => {
  return Object.keys(ACCOMMODATION_TYPE_MAP);
};

/**
 * Zwraca wszystkie dostępne typy zakwaterowania (polskie wartości)
 * @returns {string[]} Tablica z typami zakwaterowania w języku polskim
 */
export const getAccommodationTypesTranslated = () => {
  return Object.values(ACCOMMODATION_TYPE_MAP);
};

export default ACCOMMODATION_TYPE_MAP;
