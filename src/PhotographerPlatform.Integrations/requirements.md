# PhotographerPlatform.Integrations Requirements (Filtered)

Source: docs/requirements.md

Filtered to requirements partially or fully implemented by this service.

## 10. Integrations & Automation

### 10.1 Payment Providers
- Requirement: Integrate with payment processors for credit cards.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Payment integration
      Scenario: Process card payment
        Given payment integration is configured
        When a client submits a card payment
        Then the payment is authorized and captured
    ```

### 10.2 Lab Fulfillment
- Requirement: Integrate with print labs for automated fulfillment.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Lab fulfillment
      Scenario: Auto-submit print order
        Given a print lab integration is enabled
        When a print order is placed
        Then the order is submitted to the lab automatically
    ```

### 10.3 API & Webhooks
- Requirement: Provide API keys and webhooks for events (orders, galleries, payments).
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Webhooks
      Scenario: Receive order webhook
        Given I have a webhook subscribed to order events
        When an order is created
        Then my endpoint receives the order event payload
    ```


