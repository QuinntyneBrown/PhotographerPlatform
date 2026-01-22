# PhotographerPlatform.Communication - Implementation Roadmap

## Overview
This roadmap outlines the implementation plan for the Communication microservice, which handles automated email notifications, messaging, and internal notes for projects.

---

## Phase 1: Foundation & Infrastructure

### 1.1 Project Setup
- [ ] Initialize .NET project structure with clean architecture
- [ ] Configure dependency injection container
- [ ] Set up logging and telemetry infrastructure
- [ ] Configure database context and migrations
- [ ] Set up message broker for event consumption

### 1.2 Data Models & Storage
- [ ] Create `EmailTemplate` entity (type, subject, body, variables)
- [ ] Create `EmailLog` entity (recipient, template, status, sent_at)
- [ ] Create `Message` entity (project, sender, content, visibility)
- [ ] Create `Note` entity (project, author, content, internal flag)
- [ ] Set up database indexes and relationships

### 1.3 Email Provider Integration
- [ ] Integrate email service provider (SendGrid/AWS SES/Postmark)
- [ ] Configure SMTP/API settings
- [ ] Set up email sending queue
- [ ] Implement delivery status tracking

---

## Phase 2: Email Notifications (Requirement 6.1)

### 2.1 Email Template System
- [ ] Design template engine (Razor/Handlebars)
- [ ] Create base email layout template
- [ ] Implement variable substitution system
- [ ] Add template versioning support
- [ ] Create template preview functionality

### 2.2 Gallery Delivery Emails
- [ ] Create gallery delivery email template
- [ ] Include gallery link, cover image, photographer info
- [ ] Add download instructions if applicable
- [ ] Implement personalization (client name)
- [ ] Add gallery expiration notice if set

### 2.3 Order Notification Emails
- [ ] Create order confirmation email template
- [ ] Create order shipped email template
- [ ] Create order delivered email template
- [ ] Include order details, items, tracking info
- [ ] Add receipt/invoice attachment

### 2.4 Reminder Emails
- [ ] Create gallery expiration reminder template
- [ ] Create unpaid invoice reminder template
- [ ] Create proofing deadline reminder template
- [ ] Implement scheduling for reminder emails

### 2.5 Email Trigger System
- [ ] Create event handlers for gallery delivery
- [ ] Create event handlers for order status changes
- [ ] Implement reminder scheduling service
- [ ] Add email throttling to prevent spam

### 2.6 Email Notification API
- [ ] Create `POST /api/notifications/send` - Manual send
- [ ] Create `GET /api/notifications/templates` - List templates
- [ ] Create `PUT /api/notifications/templates/{id}` - Edit template
- [ ] Create `GET /api/notifications/logs` - Email history
- [ ] Create `POST /api/notifications/preview` - Preview email

### 2.7 Email Notification Tests
- [ ] Unit tests for template rendering
- [ ] Unit tests for event handlers
- [ ] Integration tests for email delivery
- [ ] Acceptance test: "Gallery delivery email" scenario

---

## Phase 3: Messaging & Notes (Requirement 6.2)

### 3.1 Internal Notes System
- [ ] Implement note creation with author tracking
- [ ] Add note editing and deletion
- [ ] Create note categorization (general, follow-up, important)
- [ ] Implement note search functionality

### 3.2 Client-Visible Messages
- [ ] Create message posting with visibility flag
- [ ] Implement message threading
- [ ] Add attachment support for messages
- [ ] Create read/unread status tracking

### 3.3 Message Visibility Control
- [ ] Implement internal-only message flag
- [ ] Create client-visible message flag
- [ ] Build visibility toggle functionality
- [ ] Add bulk visibility update

### 3.4 Message Notifications
- [ ] Send email notification for new client messages
- [ ] Create in-app notification for new messages
- [ ] Implement notification preferences

### 3.5 Messaging API
- [ ] Create `GET /api/projects/{id}/messages` - List messages
- [ ] Create `POST /api/projects/{id}/messages` - Add message
- [ ] Create `PUT /api/messages/{id}` - Edit message
- [ ] Create `DELETE /api/messages/{id}` - Delete message
- [ ] Create `PUT /api/messages/{id}/visibility` - Change visibility

### 3.6 Notes API
- [ ] Create `GET /api/projects/{id}/notes` - List notes
- [ ] Create `POST /api/projects/{id}/notes` - Add note
- [ ] Create `PUT /api/notes/{id}` - Edit note
- [ ] Create `DELETE /api/notes/{id}` - Delete note

### 3.7 Messaging & Notes Tests
- [ ] Unit tests for message operations
- [ ] Unit tests for note operations
- [ ] Integration tests for visibility controls
- [ ] Acceptance test: "Add a client-visible message" scenario

---

## Phase 4: Advanced Communication Features

### 4.1 Email Customization
- [ ] Allow custom email branding (logo, colors)
- [ ] Create custom footer configuration
- [ ] Implement custom reply-to addresses
- [ ] Add email signature support

### 4.2 Communication Preferences
- [ ] Create user email preference settings
- [ ] Implement unsubscribe functionality
- [ ] Add email frequency controls
- [ ] Build preference management UI endpoints

### 4.3 Bulk Communications
- [ ] Create bulk email sending functionality
- [ ] Implement recipient list management
- [ ] Add campaign tracking
- [ ] Create send scheduling for bulk emails

### 4.4 Message History & Search
- [ ] Implement full-text search for messages
- [ ] Create communication timeline view
- [ ] Add export communication history
- [ ] Build message archiving

---

## Phase 5: Delivery & Reliability

### 5.1 Email Delivery Optimization
- [ ] Implement email queue with priority levels
- [ ] Add retry logic for failed deliveries
- [ ] Create bounce handling
- [ ] Implement complaint handling (spam reports)

### 5.2 Delivery Tracking
- [ ] Track email open rates
- [ ] Track link click rates
- [ ] Implement delivery status webhooks
- [ ] Create delivery analytics dashboard

### 5.3 Email Validation
- [ ] Implement email address validation
- [ ] Add domain verification checks
- [ ] Create invalid email handling
- [ ] Build email health scoring

---

## Phase 6: Compliance & Security

### 6.1 Email Compliance
- [ ] Implement CAN-SPAM compliance
- [ ] Add GDPR consent tracking
- [ ] Create email audit logging
- [ ] Build compliance reporting

### 6.2 Data Security
- [ ] Encrypt message content at rest
- [ ] Implement secure attachment storage
- [ ] Add message retention policies
- [ ] Create secure message links

---

## Dependencies

| Dependency | Type | Description |
|------------|------|-------------|
| Identity Service | Internal | User information, client details |
| Galleries Service | Internal | Gallery delivery events |
| Store Service | Internal | Order events |
| Workspace Service | Internal | Project information |
| SendGrid/SES | External | Email delivery |

---

## API Endpoints Summary

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/notifications/send` | POST | Send manual notification |
| `/api/notifications/templates` | GET | List email templates |
| `/api/notifications/templates/{id}` | PUT | Edit template |
| `/api/notifications/logs` | GET | Email history |
| `/api/notifications/preview` | POST | Preview email |
| `/api/projects/{id}/messages` | GET/POST | Project messages |
| `/api/messages/{id}` | PUT/DELETE | Edit/delete message |
| `/api/messages/{id}/visibility` | PUT | Change visibility |
| `/api/projects/{id}/notes` | GET/POST | Project notes |
| `/api/notes/{id}` | PUT/DELETE | Edit/delete note |

---

## Email Templates Required

| Template | Trigger | Recipients |
|----------|---------|------------|
| Gallery Delivery | Gallery published | Client |
| Order Confirmation | Order placed | Client |
| Order Shipped | Order shipped | Client |
| Order Delivered | Order delivered | Client |
| Gallery Expiring | 7 days before expiry | Client |
| Invoice Reminder | Payment due | Client |
| New Message | Client message | Photographer |
| Welcome | User signup | New user |

---

## Success Criteria

- [ ] Clients receive delivery emails when galleries are delivered
- [ ] Messages marked as client-visible are accessible to clients
- [ ] Internal notes are only visible to team members
- [ ] Email delivery has >95% success rate
- [ ] All acceptance criteria from requirements.md are passing
