@ui @smoke
Feature: RESTful Booker landing page

  Scenario: The learning API landing page is accessible
    Given I open the RESTful Booker landing page
    Then the page should identify RESTful Booker
    And the page should contain API learning content