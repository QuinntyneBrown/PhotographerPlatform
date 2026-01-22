# PhotographerPlatform.Store Requirements (Filtered)

Source: docs/requirements.md

Filtered to requirements partially or fully implemented by this service.

## 4. Online Store

### 4.1 Product Catalog
- Requirement: Create print and digital product listings with pricing and taxes.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Product creation
      Scenario: Add a digital product
        Given I am in the store settings
        When I create a digital product with a price
        Then the product appears in the product catalog
    ```

### 4.2 Cart & Checkout
- Requirement: Clients can purchase products with a streamlined checkout flow.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Checkout flow
      Scenario: Client completes a purchase
        Given I have items in my cart
        When I submit payment details
        Then the order is confirmed
        And I receive an order confirmation
    ```

### 4.3 Coupons & Discounts
- Requirement: Create discount codes with usage limits and expiry.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Discount codes
      Scenario: Apply a valid discount
        Given a valid discount code exists
        When I apply the discount code at checkout
        Then the order total is reduced accordingly
    ```

### 4.4 Order Management
- Requirement: View and manage order status, fulfillment, and refunds.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Order status updates
      Scenario: Mark an order as fulfilled
        Given an order is paid
        When I mark the order as fulfilled
        Then the order status is updated to fulfilled
    ```


