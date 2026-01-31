/**
 * Accommodation Type Mapper
 * Provides mapping between backend accommodation type keys and display labels.
 */

const ACCOMMODATION_TYPE_MAP = {
  Apartment: 'Apartment',
  House: 'House',
  Hotel: 'Hotel Room',
  Villa: 'Villa',
  Room: 'Room',
  Loft: 'Loft',
  Studio: 'Studio',
};

/**
 * Translates accommodation type key to display label.
 * @param {string} type - Accommodation type key
 * @returns {string} Display label or original type if not found
 */
export const translateAccommodationType = (type) => {
  return ACCOMMODATION_TYPE_MAP[type] || type;
};

/**
 * Reverse translates display label back to accommodation type key.
 * @param {string} type - Display label
 * @returns {string} Type key or original label if not found
 */
export const reverseTranslateAccommodationType = (type) => {
  const reverseMap = Object.fromEntries(
    Object.entries(ACCOMMODATION_TYPE_MAP).map(([en, pl]) => [pl, en])
  );
  return reverseMap[type] || type;
};

/**
 * Returns all available accommodation type keys.
 * @returns {Array<string>} Array of type keys
 */
export const getAccommodationTypes = () => {
  return Object.keys(ACCOMMODATION_TYPE_MAP);
};

/**
 * Returns all accommodation type display labels.
 * @returns {Array<string>} Array of display labels
 */
export const getAccommodationTypesTranslated = () => {
  return Object.values(ACCOMMODATION_TYPE_MAP);
};

export default ACCOMMODATION_TYPE_MAP;
