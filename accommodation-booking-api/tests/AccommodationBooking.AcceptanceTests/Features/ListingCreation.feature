Feature: Tworzenie Oferty
    Jako gospodarz (Host)
    Chcę dodać nową ofertę wynajmu
    Aby goście mogli ją rezerwować

Scenario: Poprawne utworzenie nowej oferty przez zalogowanego gospodarza
    Given Jestem zarejestrowany i zalogowany jako "Host"
    When Wysyłam żądanie utworzenia oferty z następującymi danymi:
        | Title           | Description      | Type      | Beds | MaxGuests | Amount | Currency | Country | City     | Street | Building | Postal | Photos |
        | Apartament Wrocław | Piękny widok | Apartment | 2    | 4         | 250    | PLN      | Poland  | Wrocław  | Rynek  | 10/2     | 50-100 | photo1.jpg |
    Then Odpowiedź serwera powinna mieć status 200 OK
    And W odpowiedzi powinienem otrzymać szczegóły utworzonej oferty
    And Oferta powinna być zapisana w bazie danych