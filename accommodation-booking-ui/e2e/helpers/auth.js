/**
 * Funkcje pomocnicze do autoryzacji w testach E2E
 */

import { testUsers } from '../config/test-data.js';

/**
 * Loguje użytkownika do aplikacji
 * @param {import('@playwright/test').Page} page - Obiekt strony Playwright
 * @param {string} email - Email użytkownika
 * @param {string} password - Hasło użytkownika
 */
export async function login(page, email, password) {
  await page.goto('/login');

  await page.waitForSelector('input[type="email"]', { timeout: 10000 });

  await page.fill('input[type="email"]', email);
  await page.fill('input[type="password"]', password);

  await page.click('button[type="submit"]');
  
  await page.waitForURL(/^(?!.*\/login).*$/, { timeout: 15000 });
  await page.waitForLoadState('domcontentloaded', { timeout: 10000 });
}

/**
 * Loguje się jako użytkownik testowy typu guest
 * @param {import('@playwright/test').Page} page - Obiekt strony Playwright
 */
export async function loginAsGuest(page) {
  await login(page, testUsers.guest.email, testUsers.guest.password);
}

/**
 * Loguje się jako użytkownik testowy typu host
 * @param {import('@playwright/test').Page} page - Obiekt strony Playwright
 */
export async function loginAsHost(page) {
  await login(page, testUsers.host.email, testUsers.host.password);
}

/**
 * Loguje się jako użytkownik testowy typu admin
 * @param {import('@playwright/test').Page} page - Obiekt strony Playwright
 */
export async function loginAsAdmin(page) {
  await login(page, testUsers.admin.email, testUsers.admin.password);
}

/**
 * Wylogowuje użytkownika z aplikacji
 * @param {import('@playwright/test').Page} page - Obiekt strony Playwright
 */
export async function logout(page) {
  await page.click('[aria-label="Wyloguj"]');
  await page.waitForURL('/login');
}
