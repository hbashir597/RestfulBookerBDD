@api @smoke
Feature: Booking service health

  Scenario: Ping confirms that the service is available
    When I call the booking service health endpoint
    Then the health response should be successful