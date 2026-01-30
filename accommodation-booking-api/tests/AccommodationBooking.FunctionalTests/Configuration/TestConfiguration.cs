namespace AccommodationBooking.FunctionalTests.Configuration
{
    public static class TestConfiguration
    {
        // URL aplikacji
        public const string BaseUrl = "http://localhost:5173";
        public const string LoginUrl = $"{BaseUrl}/login";
        public const string RegisterUrl = $"{BaseUrl}/register";

        // Timeouty
        public const int DefaultTimeout = 10;
        public const int LongTimeout = 30;
        public const int ShortTimeout = 5;

        // Dane testowe
        public static class TestData
        {
            public static class Guest
            {
                public const string Email = "guest@test.com";
                public const string Password = "Test123!";
            }

            public static class Host
            {
                public const string Email = "host@test.com";
                public const string Password = "Test123!";
            }

            public static class Admin
            {
                public const string Email = "admin@test.com";
                public const string Password = "Test123!";
            }

            public static class Listing
            {
                public const string ListingId = "ABF7798A-DE6C-46D9-8EDF-C711F1BEA1FF";
            }
        }

        // Opcje przeglądarki
        public static class BrowserOptions
        {
            public const bool Headless = false;
            public const bool Maximized = true;
            public const bool DisableNotifications = true;
        }
    }
}
