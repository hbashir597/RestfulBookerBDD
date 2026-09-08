@api @regression @contract
Feature: Manage a booking through its lifecycle

  Background:
    Given the booking service is available
    And I have a valid admin token

  Scenario: Create, retrieve, update, patch and delete a booking
    When I create a unique booking
    Then the booking should be created
    And the booking should be retrievable
    And the GET booking response should match the booking schema
    When I replace the booking details
    Then the replaced details should be persisted
    When I partially update the price and additional needs
    Then only the selected booking fields should change
    When I delete the booking
    Then retrieving the deleted booking should return 404

  @negative
  Scenario: An unauthenticated update is rejected
    Given I created a unique booking
    When I try to replace it without authentication
    Then the update response should be 401 or 403

  @api @regression
  Scenario Outline: Create bookings with different deposit statuses
    Given I have a valid booking with deposit paid "<depositPaid>"
    When I create the booking
    Then the booking should be created successfully
    And the returned booking should have deposit paid "<depositPaid>"

    Examples:
      | depositPaid |
      | true        |
      | false       |