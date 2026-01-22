# PhotographerPlatform.Workspace Requirements (Filtered)

Source: docs/requirements.md

Filtered to requirements partially or fully implemented by this service.

## 2. Photographer Workspace (Dashboard)

### 2.1 Project Management
- Requirement: Create, edit, archive projects with metadata (client, date, location, tags).
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Project creation
      Scenario: Create a new project
        Given I am in the dashboard
        When I create a project with required fields
        Then the project appears in my project list
    ```

### 2.2 Client Management (CRM)
- Requirement: Manage client profiles, contact info, notes, and project history.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Client profile
      Scenario: Update client contact details
        Given a client exists in my CRM
        When I update the client phone number
        Then the new phone number is saved and displayed
    ```

### 2.3 Search & Filters
- Requirement: Search and filter projects, galleries, and clients by multiple attributes.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Global search and filtering
      Scenario: Filter projects by tag
        Given I have projects tagged "wedding"
        When I filter projects by tag "wedding"
        Then only wedding projects are shown
    ```


