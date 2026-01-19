/**
 * Pomocnicze narzędzia do debugowania testów
 */

/**
 * Robi screenshot i zapisuje do pliku
 * @param {import('@playwright/test').Page} page - Obiekt strony Playwright
 * @param {string} name - Nazwa screenshota
 */
export async function takeDebugScreenshot(page, name) {
  await page.screenshot({
    path: `e2e/screenshots/${name}-${Date.now()}.png`,
    fullPage: true,
  });
}

/**
 * Wyświetla informacje o obecnym stanie strony
 * @param {import('@playwright/test').Page} page - Obiekt strony Playwright
 */
export async function logPageState(page) {
  const url = page.url();
  const title = await page.title();
  console.log(`[DEBUG] URL: ${url}`);
  console.log(`[DEBUG] Title: ${title}`);
}

/**
 * Czeka z opcjonalnym logowaniem
 * @param {import('@playwright/test').Page} page - Obiekt strony Playwright
 * @param {number} ms - Czas oczekiwania w milisekundach
 * @param {string} reason - Powód oczekiwania (opcjonalnie)
 */
export async function waitWithLog(page, ms, reason = '') {
  if (reason) {
    console.log(`[DEBUG] Waiting ${ms}ms: ${reason}`);
  }
  await page.waitForTimeout(ms);
}

/**
 * Sprawdza czy element istnieje na stronie
 * @param {import('@playwright/test').Page} page - Obiekt strony Playwright
 * @param {string} selector - Selektor elementu
 * @returns {Promise<boolean>}
 */
export async function elementExists(page, selector) {
  const count = await page.locator(selector).count();
  return count > 0;
}
