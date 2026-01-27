Feature: Listing Creation
    As a Host
    I want to add a new rental listing
    So that guests can book it

Scenario: Successfully create a new listing as a logged-in host
    Given I am registered and logged in as "Host"
    When I send a request to create a listing with the following data:
        | Title              | Description      | Type      | Beds | MaxGuests | Amount | Currency | Country | City    | Street | Building | Postal | Photos                               |
        | Apartament Wrocław | Piękny widok     | Apartment | 2    | 4         | 250    | PLN      | Poland  | Wrocław | Rynek  | 10/2     | 50-100 | https://example.com/photo1.jpg |
    Then The server response should have status 200 OK
    And I should receive the details of the created listing in the response
    And The listing should be saved in the database