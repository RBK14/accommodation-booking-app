import { defineConfig, devices } from '@playwright/test';

/**
 * Konfiguracja Playwright dla testów E2E
 * @see https://playwright.dev/docs/test-configuration
 */
export default defineConfig({
  testDir: './e2e',

  /* Maksymalny czas na jeden test */
  timeout: 30 * 1000,

  /* Konfiguracja expect */
  expect: {
    timeout: 5000,
  },

  /* Uruchom testy sekwencyjnie w jednym pliku */
  fullyParallel: false,

  /* Nie kontynuuj testów jeśli jakiś test nie przejdzie w CI */
  forbidOnly: !!process.env.CI,

  /* Liczba powtórzeń dla testów które nie przeszły */
  retries: process.env.CI ? 2 : 0,

  /* Liczba workerów */
  workers: process.env.CI ? 1 : undefined,

  /* Reporter */
  reporter: 'html',

  /* Ustawienia wspólne dla wszystkich projektów */
  use: {
    /* Bazowy URL dla page.goto() */
    baseURL: 'http://localhost:5173',

    /* Zbieraj trace tylko gdy test nie przejdzie */
    trace: 'on-first-retry',

    /* Screenshot tylko przy błędzie */
    screenshot: 'only-on-failure',

    /* Video tylko przy błędzie */
    video: 'retain-on-failure',
  },

  /* Konfiguracja różnych przeglądarek */
  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },

    // Odkomentuj poniższe jeśli chcesz testować w innych przeglądarkach
    // {
    //   name: 'firefox',
    //   use: { ...devices['Desktop Firefox'] },
    // },

    // {
    //   name: 'webkit',
    //   use: { ...devices['Desktop Safari'] },
    // },
  ],

  /* Uruchom dev server przed testami */
  webServer: {
    command: 'npm run dev',
    url: 'http://localhost:5173',
    reuseExistingServer: !process.env.CI,
    timeout: 120 * 1000,
  },
});
