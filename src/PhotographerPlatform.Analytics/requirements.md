# PhotographerPlatform.Analytics Requirements (Filtered)

Source: docs/requirements.md

Filtered to requirements partially or fully implemented by this service.

## 8. Analytics & Insights

### 8.1 Gallery Insights
- Requirement: Track views, favorites, downloads, and sales per gallery.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Gallery analytics
      Scenario: View gallery insights
        Given a gallery has activity
        When I view the gallery insights
        Then I see counts for views, favorites, downloads, and sales
    ```

### 8.2 Store Performance
- Requirement: Track revenue, conversion, and product sales.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Store performance metrics
      Scenario: View store dashboard
        Given I have store sales
        When I view the store dashboard
        Then I see revenue and product sales metrics
    ```


