/**
 * Mapa konwersji statusów rezerwacji z angielskiego na polski
 */
const RESERVATION_STATUS_MAP = {
  Accepted: 'Zaakceptowana',
  InProgress: 'W trakcie',
  Completed: 'Zakończona',
  Cancelled: 'Anulowana',
  NoShow: 'Nie pojawił się',
};

/**
 * Konwertuje status rezerwacji z angielskiego na polski
 * @param {string} status - Status rezerwacji w języku angielskim
 * @returns {string} Status rezerwacji w języku polskim
 */
export const translateReservationStatus = (status) => {
  return RESERVATION_STATUS_MAP[status] || status;
};

/**
 * Zwraca odwrotne mapowanie - z polskiego na angielski
 * @param {string} status - Status rezerwacji w języku polskim
 * @returns {string} Status rezerwacji w języku angielskim
 */
export const reverseTranslateReservationStatus = (status) => {
  const reverseMap = Object.fromEntries(
    Object.entries(RESERVATION_STATUS_MAP).map(([en, pl]) => [pl, en])
  );
  return reverseMap[status] || status;
};

export default RESERVATION_STATUS_MAP;
