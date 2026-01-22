# PhotographerPlatform.Communication Requirements (Filtered)

Source: docs/requirements.md

Filtered to requirements partially or fully implemented by this service.

## 6. Client Communication

### 6.1 Email Notifications
- Requirement: Automated emails for gallery delivery, orders, and reminders.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Automated email notifications
      Scenario: Gallery delivery email
        Given I deliver a gallery to a client
        When the delivery is sent
        Then the client receives a delivery email
    ```

### 6.2 Messaging & Notes
- Requirement: Internal notes and client-visible messages per project.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Project messages
      Scenario: Add a client-visible message
        Given I am viewing a project
        When I add a message marked as client-visible
        Then the client can see the message
    ```


