# PhotographerPlatform.Galleries Requirements (Filtered)

Source: docs/requirements.md

Filtered to requirements partially or fully implemented by this service.

## 3. Gallery Delivery

### 3.1 Gallery Creation
- Requirement: Create galleries tied to projects with cover image and metadata.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Gallery creation
      Scenario: Create a gallery with cover
        Given I have a project
        When I create a gallery and set a cover image
        Then the gallery is created with the selected cover
    ```

### 3.2 Image Upload & Organization
- Requirement: Upload images in bulk, arrange order, and group into sets.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Image upload
      Scenario: Bulk upload images
        Given I am editing a gallery
        When I upload multiple images
        Then all images appear in the gallery in upload order
    ```

### 3.3 Proofing & Favorites
- Requirement: Clients can favorite and comment on images.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Client favorites
      Scenario: Client favorites an image
        Given I am a client viewing a gallery
        When I favorite an image
        Then the image appears in my favorites list
    ```

### 3.4 Downloads & Permissions
- Requirement: Control download permissions (none, web-size, full-size) and expiration dates.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Download permissions
      Scenario: Client downloads web-size image
        Given a gallery allows web-size downloads
        When I download an image
        Then I receive a web-size file
    ```

### 3.5 Gallery Access Controls
- Requirement: Support public, unlisted, and password-protected galleries.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Password-protected gallery
      Scenario: Client enters correct password
        Given a gallery is password-protected
        When I enter the correct password
        Then I can view the gallery
    ```

### 3.6 Watermarking
- Requirement: Apply optional watermarking on preview images.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Watermarked previews
      Scenario: View watermarked image
        Given a gallery has watermark enabled
        When I view an image preview
        Then the preview displays the watermark
    ```


## 11. Performance & Reliability

### 11.1 CDN & Image Optimization
- Requirement: Serve images through CDN with responsive sizes.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Responsive image delivery
      Scenario: Serve optimized images
        Given a client views a gallery on mobile
        When images are loaded
        Then images are delivered in an optimized size
    ```

### 11.2 Availability & Backups
- Requirement: Maintain high availability and automated backups.
  - Acceptance Criteria (Gherkin):
    ```gherkin
    Feature: Backup recovery
      Scenario: Restore from backup
        Given a system backup exists
        When a recovery is initiated
        Then data is restored to the latest backup point
    ```


