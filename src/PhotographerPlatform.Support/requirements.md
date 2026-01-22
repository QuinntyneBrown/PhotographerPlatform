# PhotographerPlatform.Support Requirements (Filtered)

Source: docs/requirements.md

Filtered to requirements partially or fully implemented by this service.

## 13. Support & Help

### 13.1 Help Center
- Requirement: Provide searchable help documentation and onboarding guides.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Help center search
      Scenario: Search for an article
        Given I am on the help center
        When I search for "gallery delivery"
        Then I see relevant help articles
    ```

### 13.2 Support Requests
- Requirement: Allow users to submit support tickets.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Support tickets
      Scenario: Submit a support request
        Given I am signed in
        When I submit a support form
        Then a ticket is created and I receive confirmation
    ```

