# PhotographerPlatform.Billing Requirements (Filtered)

Source: docs/requirements.md

Filtered to requirements partially or fully implemented by this service.

## 7. Billing & Subscriptions

### 7.1 Plans & Limits
- Requirement: Support multiple subscription plans with storage and feature limits.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Plan limits
      Scenario: Exceed storage limit
        Given my plan has a storage limit
        When I attempt to upload beyond the limit
        Then I am notified and upload is blocked
    ```

### 7.2 Billing Management
- Requirement: Users can manage payment methods and invoices.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Billing management
      Scenario: Update payment method
        Given I am on the billing page
        When I add a new payment method
        Then the payment method is saved and set as default
    ```

### 7.3 Trial & Upgrade Flow
- Requirement: Free trial with upgrade prompts and expiration handling.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Trial expiration
      Scenario: Trial expires
        Given I am on a trial plan
        When the trial ends
        Then I am prompted to upgrade
        And access to premium features is restricted
    ```


