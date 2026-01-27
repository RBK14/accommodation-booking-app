Feature: Reservation Creation
    As a Guest
    I want to book accommodation
    So that I can reserve a place to stay

Scenario: Successfully create a reservation as a logged-in guest
    Given I am registered and logged in as "Guest"
    And A listing exists with the following data:
        | Title              | Description      | Type      | Beds | MaxGuests | Amount | Currency | Country | City    | Street | Building | Postal | Photos                              |
        | Mountain Cottage   | Beautiful views  | House     | 3    | 6         | 400    | PLN      | Poland  | Zakopane | Górska | 15       | 34-500 | https://example.com/cottage.jpg |
    When I send a request to create a reservation with the following data:
        | CheckInDaysFromNow | CheckOutDaysFromNow |
        | 10                 | 14                  |
    Then The server response should have status 200 OK
    And I should receive the details of the created reservation in the response
    And The reservation should be saved in the database

Scenario: Check available dates for a listing with no reservations
    Given I am registered and logged in as "Guest"
    And A listing exists with the following data:
        | Title            | Description     | Type      | Beds | MaxGuests | Amount | Currency | Country | City     | Street | Building | Postal | Photos                          |
        | Beach House      | Ocean view      | House     | 2    | 4         | 500    | PLN      | Poland  | Sopot    | Morska | 20       | 81-700 | https://example.com/beach.jpg   |
    When I request available dates for the listing starting from 5 days from now for 10 days
    Then The server response should have status 200 OK
    And I should receive 11 available dates in the response

Scenario: Check available dates excludes occupied dates
    Given I am registered and logged in as "Guest"
    And A listing exists with the following data:
        | Title           | Description      | Type      | Beds | MaxGuests | Amount | Currency | Country | City      | Street  | Building | Postal | Photos                           |
        | City Apartment  | Downtown         | Apartment | 1    | 2         | 300    | PLN      | Poland  | Warszawa  | Nowa    | 5        | 00-100 | https://example.com/city.jpg     |
    And The listing has an existing reservation from 7 days from now to 10 days from now
    When I request available dates for the listing starting from 5 days from now for 10 days
    Then The server response should have status 200 OK
    And The available dates should not include the occupied period

Scenario: Create reservation after checking available dates
    Given I am registered and logged in as "Guest"
    And A listing exists with the following data:
        | Title              | Description      | Type      | Beds | MaxGuests | Amount | Currency | Country | City    | Street | Building | Postal | Photos                              |
        | Mountain Cottage   | Beautiful views  | House     | 3    | 6         | 400    | PLN      | Poland  | Zakopane | Górska | 15       | 34-500 | https://example.com/cottage.jpg |
    And The listing has an existing reservation from 5 days from now to 8 days from now
    When I request available dates for the listing starting from 3 days from now for 10 days
    Then The server response should have status 200 OK
    And The available dates should not include the occupied period
    When I send a request to create a reservation with the following data:
        | CheckInDaysFromNow | CheckOutDaysFromNow |
        | 10                 | 14                  |
    Then The server response should have status 200 OK
    And I should receive the details of the created reservation in the response
    And The reservation should be saved in the database
