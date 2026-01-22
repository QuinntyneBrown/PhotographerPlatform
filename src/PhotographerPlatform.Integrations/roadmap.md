# PhotographerPlatform.Integrations - Implementation Roadmap

## Overview
This roadmap outlines the implementation plan for the Integrations microservice, which manages external service integrations including payment providers, print lab fulfillment, and webhook/API functionality.

---

## Phase 1: Foundation & Infrastructure

### 1.1 Project Setup
- [ ] Initialize .NET project structure with clean architecture
- [ ] Configure dependency injection container
- [ ] Set up logging and telemetry infrastructure
- [ ] Configure database context and migrations
- [ ] Set up message broker for async processing

### 1.2 Data Models & Storage
- [ ] Create `Integration` entity (type, config, status, credentials)
- [ ] Create `PaymentProvider` entity (name, api_key, settings)
- [ ] Create `LabProvider` entity (name, credentials, endpoints)
- [ ] Create `ApiKey` entity (key_hash, scopes, user, created)
- [ ] Create `Webhook` entity (url, events, secret, active)
- [ ] Create `WebhookDelivery` entity (webhook, payload, status, attempts)
- [ ] Set up database indexes and relationships

### 1.3 Integration Framework
- [ ] Create base integration adapter interface
- [ ] Implement credential encryption/decryption
- [ ] Build configuration validation framework
- [ ] Create integration health check system

---

## Phase 2: Payment Provider Integration (Requirement 10.1)

### 2.1 Payment Provider Abstraction
- [ ] Design `IPaymentProvider` interface
- [ ] Define payment request/response contracts
- [ ] Create payment method tokenization interface
- [ ] Implement provider factory pattern

### 2.2 Stripe Integration
- [ ] Implement Stripe adapter
- [ ] Handle PaymentIntent creation
- [ ] Implement card authorization
- [ ] Handle payment capture
- [ ] Process refunds
- [ ] Implement Stripe webhook handling

### 2.3 PayPal Integration (Optional)
- [ ] Implement PayPal adapter
- [ ] Handle order creation
- [ ] Implement authorization flow
- [ ] Process captures and refunds

### 2.4 Payment Configuration
- [ ] Create provider setup wizard endpoints
- [ ] Implement API key validation
- [ ] Build test mode support
- [ ] Create provider switching logic

### 2.5 Payment Processing API
- [ ] Create `POST /api/payments/authorize` - Authorize payment
- [ ] Create `POST /api/payments/capture` - Capture payment
- [ ] Create `POST /api/payments/refund` - Process refund
- [ ] Create `GET /api/payments/{id}` - Payment status
- [ ] Create `POST /api/payments/setup-intent` - Card setup

### 2.6 Payment Provider Configuration API
- [ ] Create `GET /api/integrations/payment-providers` - List providers
- [ ] Create `POST /api/integrations/payment-providers` - Configure provider
- [ ] Create `PUT /api/integrations/payment-providers/{id}` - Update
- [ ] Create `DELETE /api/integrations/payment-providers/{id}` - Remove
- [ ] Create `POST /api/integrations/payment-providers/{id}/test` - Test

### 2.7 Payment Integration Tests
- [ ] Unit tests for Stripe adapter
- [ ] Unit tests for payment flow
- [ ] Integration tests with Stripe test mode
- [ ] Acceptance test: "Process card payment" scenario

---

## Phase 3: Lab Fulfillment Integration (Requirement 10.2)

### 3.1 Lab Provider Abstraction
- [ ] Design `ILabProvider` interface
- [ ] Define order submission contracts
- [ ] Create product mapping interface
- [ ] Implement status polling interface

### 3.2 WHCC Integration
- [ ] Implement WHCC adapter
- [ ] Handle order submission
- [ ] Implement product catalog sync
- [ ] Create shipping option mapping
- [ ] Handle order status updates

### 3.3 Miller's Lab Integration
- [ ] Implement Miller's adapter
- [ ] Handle order submission
- [ ] Implement product mapping
- [ ] Process status webhooks

### 3.4 Generic Lab Integration
- [ ] Create configurable generic adapter
- [ ] Implement REST API connector
- [ ] Build field mapping configuration
- [ ] Create response transformation

### 3.5 Order Submission Pipeline
- [ ] Create order queue consumer
- [ ] Implement automatic submission on order placement
- [ ] Handle file transfer to lab (image upload)
- [ ] Implement retry logic for failures
- [ ] Create manual submission fallback

### 3.6 Lab Fulfillment API
- [ ] Create `POST /api/fulfillment/submit` - Submit order
- [ ] Create `GET /api/fulfillment/orders/{id}/status` - Order status
- [ ] Create `POST /api/fulfillment/orders/{id}/cancel` - Cancel order

### 3.7 Lab Configuration API
- [ ] Create `GET /api/integrations/labs` - List lab providers
- [ ] Create `POST /api/integrations/labs` - Configure lab
- [ ] Create `PUT /api/integrations/labs/{id}` - Update config
- [ ] Create `DELETE /api/integrations/labs/{id}` - Remove lab
- [ ] Create `POST /api/integrations/labs/{id}/test` - Test connection
- [ ] Create `GET /api/integrations/labs/{id}/products` - Sync products

### 3.8 Lab Fulfillment Tests
- [ ] Unit tests for lab adapters
- [ ] Unit tests for order submission pipeline
- [ ] Integration tests with lab sandboxes
- [ ] Acceptance test: "Auto-submit print order" scenario

---

## Phase 4: API & Webhooks (Requirement 10.3)

### 4.1 API Key Management
- [ ] Implement API key generation (secure random)
- [ ] Create key hashing for storage
- [ ] Implement key scopes/permissions
- [ ] Build key rotation functionality
- [ ] Add key expiration support

### 4.2 API Key Authentication
- [ ] Create API key authentication handler
- [ ] Implement key validation middleware
- [ ] Add rate limiting per key
- [ ] Track usage statistics per key

### 4.3 API Key Endpoints
- [ ] Create `POST /api/apikeys` - Generate new key
- [ ] Create `GET /api/apikeys` - List keys (masked)
- [ ] Create `DELETE /api/apikeys/{id}` - Revoke key
- [ ] Create `PUT /api/apikeys/{id}/rotate` - Rotate key
- [ ] Create `GET /api/apikeys/{id}/usage` - Usage stats

### 4.4 Webhook Subscription System
- [ ] Design webhook event types
  - [ ] `order.created`
  - [ ] `order.updated`
  - [ ] `order.fulfilled`
  - [ ] `gallery.created`
  - [ ] `gallery.delivered`
  - [ ] `payment.completed`
  - [ ] `payment.failed`
- [ ] Create webhook subscription storage
- [ ] Implement URL validation
- [ ] Generate webhook signing secrets

### 4.5 Webhook Registration Endpoints
- [ ] Create `POST /api/webhooks` - Register webhook
- [ ] Create `GET /api/webhooks` - List webhooks
- [ ] Create `GET /api/webhooks/{id}` - Webhook details
- [ ] Create `PUT /api/webhooks/{id}` - Update webhook
- [ ] Create `DELETE /api/webhooks/{id}` - Remove webhook
- [ ] Create `PUT /api/webhooks/{id}/events` - Update events

### 4.6 Webhook Delivery System
- [ ] Create delivery queue and worker
- [ ] Implement HMAC-SHA256 payload signing
- [ ] Add timestamp to prevent replay attacks
- [ ] Implement retry with exponential backoff
- [ ] Create delivery timeout handling
- [ ] Build dead letter queue for failures

### 4.7 Webhook Delivery Tracking
- [ ] Log all delivery attempts
- [ ] Track response status codes
- [ ] Record response times
- [ ] Create delivery history endpoint

### 4.8 Webhook Management Endpoints
- [ ] Create `GET /api/webhooks/{id}/deliveries` - Delivery history
- [ ] Create `POST /api/webhooks/{id}/test` - Send test event
- [ ] Create `POST /api/webhooks/{id}/deliveries/{did}/retry` - Retry

### 4.9 Webhook Event Publishing
- [ ] Create event publisher service
- [ ] Integrate with message broker
- [ ] Implement event filtering by subscription
- [ ] Add event batching (optional)

### 4.10 API & Webhook Tests
- [ ] Unit tests for API key generation
- [ ] Unit tests for webhook signing
- [ ] Unit tests for delivery retry logic
- [ ] Integration tests for full webhook flow
- [ ] Acceptance test: "Receive order webhook" scenario

---

## Phase 5: Integration Management

### 5.1 Integration Dashboard
- [ ] Create integration status overview endpoint
- [ ] Implement connection health checks
- [ ] Build sync status tracking
- [ ] Create error reporting

### 5.2 Integration Logs
- [ ] Log all external API calls
- [ ] Track request/response data
- [ ] Create log search and filtering
- [ ] Implement log retention policies

### 5.3 Integration Alerts
- [ ] Create failure alerting
- [ ] Implement degradation notifications
- [ ] Build credential expiration warnings
- [ ] Add quota/limit warnings

---

## Phase 6: Advanced Integration Features

### 6.1 Third-Party App Marketplace (Future)
- [ ] Design app integration framework
- [ ] Create OAuth 2.0 authorization flow
- [ ] Implement permission scoping
- [ ] Build app review process

### 6.2 Zapier/Make Integration
- [ ] Create Zapier trigger endpoints
- [ ] Implement action endpoints
- [ ] Build authentication for Zapier
- [ ] Create integration documentation

### 6.3 Calendar Integration
- [ ] Google Calendar sync
- [ ] Apple Calendar sync
- [ ] Create booking integration

---

## Dependencies

| Dependency | Type | Description |
|------------|------|-------------|
| Store Service | Internal | Order events for fulfillment |
| Galleries Service | Internal | Gallery events for webhooks |
| Billing Service | Internal | Payment coordination |
| Stripe | External | Payment processing |
| Print Labs | External | Order fulfillment |
| Message Broker | Infrastructure | Event delivery |

---

## API Endpoints Summary

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/payments/authorize` | POST | Authorize payment |
| `/api/payments/capture` | POST | Capture payment |
| `/api/payments/refund` | POST | Process refund |
| `/api/integrations/payment-providers` | GET/POST | Manage providers |
| `/api/fulfillment/submit` | POST | Submit lab order |
| `/api/fulfillment/orders/{id}/status` | GET | Order status |
| `/api/integrations/labs` | GET/POST | Manage lab integrations |
| `/api/apikeys` | GET/POST | Manage API keys |
| `/api/apikeys/{id}` | DELETE | Revoke API key |
| `/api/webhooks` | GET/POST | Manage webhooks |
| `/api/webhooks/{id}` | GET/PUT/DELETE | Webhook CRUD |
| `/api/webhooks/{id}/test` | POST | Test webhook |
| `/api/webhooks/{id}/deliveries` | GET | Delivery history |

---

## Webhook Event Schema

```json
{
  "id": "evt_123456",
  "type": "order.created",
  "created": "2024-01-15T10:30:00Z",
  "data": {
    "object": {
      "id": "ord_789",
      "status": "pending",
      "total": 150.00
    }
  }
}
```

---

## Success Criteria

- [ ] Card payments are authorized and captured via configured provider
- [ ] Print orders are automatically submitted to labs when placed
- [ ] API keys can be generated and used for authentication
- [ ] Webhooks deliver signed payloads to subscribed endpoints
- [ ] Webhook deliveries retry on failure with exponential backoff
- [ ] All acceptance criteria from requirements.md are passing
