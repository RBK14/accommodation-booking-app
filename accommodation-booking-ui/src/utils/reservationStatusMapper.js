/**
 * Reservation Status Mapper
 * Provides mapping between backend reservation status keys and display labels.
 */

const RESERVATION_STATUS_MAP = {
  Accepted: 'Zaakceptowana',
  InProgress: 'W trakcie',
  Completed: 'Zakończona',
  Cancelled: 'Anulowana',
  NoShow: 'Nie pojawił się',
};

/**
 * Translates reservation status key to display label.
 * @param {string} status - Status key
 * @returns {string} Display label or original status if not found
 */
export const translateReservationStatus = (status) => {
  return RESERVATION_STATUS_MAP[status] || status;
};

/**
 * Reverse translates display label back to status key.
 * @param {string} status - Display label
 * @returns {string} Status key or original label if not found
 */
export const reverseTranslateReservationStatus = (status) => {
  const reverseMap = Object.fromEntries(
    Object.entries(RESERVATION_STATUS_MAP).map(([en, pl]) => [pl, en])
  );
  return reverseMap[status] || status;
};

export default RESERVATION_STATUS_MAP;
