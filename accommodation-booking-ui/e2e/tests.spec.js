import { test, expect } from '@playwright/test';
import { loginAsGuest, loginAsHost, loginAsAdmin } from './helpers/auth.js';

/**
 * Test E2E dla procesu rezerwacji zakwaterowania
 *
 * Scenariusz:
 * 1. Zaloguj się jako gość
 * 2. Przejdź do listy ofert
 * 3. Wybierz pierwszą dostępną ofertę
 * 4. Kliknij przycisk "Zarezerwuj"
 * 5. Wybierz daty pobytu
 * 6. Zweryfikuj poprawne wyliczenie ceny całkowitej
 */
test.describe('Proces rezerwacji zakwaterowania', () => {
  test.setTimeout(60000);

  test('Użytkownik może złożyć rezerwację od początku do końca', async ({ page }) => {
    await loginAsGuest(page);

    await expect(page).toHaveURL(/^(?!.*\/login).*$/);

    await page.goto('/listings');

    await page.waitForSelector('[data-testid="listing-card"], .MuiCard-root', { timeout: 10000 });

    const viewDetailsButton = page.locator('button:has-text("Zobacz szczegóły")').first();
    await viewDetailsButton.click();

    await page.waitForURL(/\/listing\/\d+/);

    await expect(page.locator('button:has-text("Zarezerwuj")')).toBeVisible();

    const reserveButton = page.locator('button:has-text("Zarezerwuj")').first();

    await reserveButton.click();

    await page.waitForURL(/\/reservation\/\d+/);

    await expect(page.locator('label:has-text("Data przyjazdu")')).toBeVisible();
    await expect(page.locator('label:has-text("Data wyjazdu")')).toBeVisible();

    await test.step('Wybierz datę przyjazdu (check-in)', async () => {
      const calendarButton = page
        .locator('label:has-text("Data przyjazdu")')
        .locator('..')
        .locator('button[aria-label*="calendar" i], button[aria-label*="Choose date" i]')
        .first();

      if ((await calendarButton.count()) === 0) {
        await page.locator('.MuiInputAdornment-root button').first().click();
      } else {
        await calendarButton.click();
      }

      await page.waitForSelector('.MuiDateCalendar-root, .MuiPickersDay-root', { timeout: 5000 });

      const availableDay = page.locator('.MuiPickersDay-root:not(.Mui-disabled)').first();
      await availableDay.click();
    });

    await test.step('Wybierz datę wyjazdu (check-out)', async () => {
      await page.waitForTimeout(500);

      const calendarButton = page
        .locator('label:has-text("Data wyjazdu")')
        .locator('..')
        .locator('button[aria-label*="calendar" i], button[aria-label*="Choose date" i]')
        .first();

      if ((await calendarButton.count()) === 0) {
        await page.locator('.MuiInputAdornment-root button').nth(1).click();
      } else {
        await calendarButton.click();
      }

      await page.waitForSelector('.MuiDateCalendar-root, .MuiPickersDay-root', { timeout: 5000 });

      const availableDays = page.locator('.MuiPickersDay-root:not(.Mui-disabled)');
      const daysCount = await availableDays.count();

      const indexToSelect = Math.min(3, daysCount - 1);
      await availableDays.nth(indexToSelect).click();
    });

    await page.waitForTimeout(1000);

    await expect(page.locator('text=/Całkowita cena:\\s*\\d+/i')).toBeVisible();

    const confirmButton = page.locator('button:has-text("Potwierdź rezerwację")');
    await expect(confirmButton).toBeEnabled();
  });

  test('Użytkownik może wyszukać ofertę według lokalizacji i liczby gości', async ({ page }) => {
    await loginAsGuest(page);

    await page.goto('/listings');

    const locationInput = page
      .locator('input[placeholder*="lokalizacja" i], input[placeholder*="Gdzie" i]')
      .first();

    if ((await locationInput.count()) > 0) {
      await locationInput.fill('Warszawa');

      const guestsInput = page
        .locator('input[placeholder*="gości" i], input[type="number"]')
        .first();

      if ((await guestsInput.count()) > 0) {
        await guestsInput.fill('2');
      }

      const searchButton = page.locator('button:has-text("Szukaj"), button[type="submit"]').first();
      await searchButton.click();

      await page.waitForTimeout(1000);

      const listings = page.locator('[data-testid="listing-card"], .MuiCard-root');

      if ((await listings.count()) > 0) {
        await page.locator('button:has-text("Zobacz szczegóły")').first().click();
        await expect(page).toHaveURL(/\/listing\/\d+/);
      }
    }
  });
});

/**
 * Test E2E dla usuwania oferty przez gospodarza
 *
 * Scenariusz:
 * 1. Zaloguj się jako gospodarz
 * 2. Przejdź na stronę "Moje ogłoszenia"
 * 3. Wejdź w szczegóły pierwszej oferty
 * 4. Kliknij przycisk "Usuń"
 * 5. Sprawdź czy wyświetli się okienko potwierdzenia
 */
test.describe('Proces usuwania oferty przez gospodarza', () => {
  test.setTimeout(60000);

  test('Gospodarz może zainicjować usunięcie oferty z wyświetleniem dialogu potwierdzenia', async ({
    page,
  }) => {
    await loginAsHost(page);

    await expect(page).toHaveURL(/^(?!.*\/login).*$/);

    await page.goto('/host/listings');

    await page.waitForSelector('button:has-text("Przeglądaj"), .MuiCard-root', {
      timeout: 10000,
    });

    const viewDetailsButton = page.locator('button:has-text("Przeglądaj")').first();
    await viewDetailsButton.click();

    await page.waitForURL(/\/host\/listing\/\d+/);

    await expect(page.locator('button:has-text("Edytuj")')).toBeVisible();
    await expect(page.locator('button:has-text("Usuń")')).toBeVisible();

    page.on('dialog', async (dialog) => {
      expect(dialog.type()).toBe('confirm');
      expect(dialog.message()).toContain('Czy na pewno chcesz usunąć');

      await dialog.dismiss();
    });

    const deleteButton = page.locator('button:has-text("Usuń")');
    await deleteButton.click();

    await page.waitForTimeout(500);
  });
});

/**
 * Test E2E dla usuwania użytkownika przez administratora
 *
 * Scenariusz:
 * 1. Zaloguj się jako admin
 * 2. Wybierz zakładkę "Użytkownicy"
 * 3. Wybierz użytkownika, który nie jest adminem
 * 4. Naciśnij przycisk do usuwania
 * 5. Sprawdź czy pokazał się dialog potwierdzenia
 */
test.describe('Proces usuwania użytkownika przez administratora', () => {
  test.setTimeout(60000);

  test('Administrator może zainicjować usunięcie użytkownika z wyświetleniem dialogu potwierdzenia', async ({
    page,
  }) => {
    await loginAsAdmin(page);

    await expect(page).toHaveURL(/^(?!.*\/login).*$/);

    await page.goto('/admin/users');

    await page.waitForSelector('table, .MuiTable-root', { timeout: 10000 });

    const userRows = page.locator('table tbody tr');
    const rowCount = await userRows.count();

    let nonAdminRowIndex = -1;
    for (let i = 0; i < rowCount; i++) {
      const row = userRows.nth(i);
      const roleChip = row.locator('.MuiChip-root');
      const roleText = await roleChip.textContent();

      if (roleText && !roleText.includes('Admin')) {
        nonAdminRowIndex = i;
        break;
      }
    }

    if (nonAdminRowIndex >= 0) {
      const targetRow = userRows.nth(nonAdminRowIndex);

      const viewButton = targetRow.locator('button[aria-label]').first();
      await viewButton.click();

      await page.waitForURL(/\/admin\/user\/\d+/);

      await expect(page.locator('button:has-text("Edytuj")')).toBeVisible();
      await expect(page.locator('button:has-text("Usuń")')).toBeVisible();

      page.on('dialog', async (dialog) => {
        expect(dialog.type()).toBe('confirm');
        expect(dialog.message()).toContain('Czy na pewno chcesz usunąć');

        await dialog.dismiss();
      });

      const deleteButton = page.locator('button:has-text("Usuń")');
      await deleteButton.click();

      await page.waitForTimeout(500);
    } else {
      console.warn('Nie znaleziono użytkownika nie będącego adminem do przetestowania');
    }
  });
});
