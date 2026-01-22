# PhotographerPlatform.Billing - Implementation Roadmap

## Overview
This roadmap outlines the implementation plan for the Billing microservice, which manages subscription plans, payment methods, invoices, trials, and upgrade flows.

---

## Phase 1: Foundation & Infrastructure

### 1.1 Project Setup
- [ ] Initialize .NET project structure with clean architecture
- [ ] Configure dependency injection container
- [ ] Set up logging and telemetry infrastructure
- [ ] Configure database context and migrations
- [ ] Set up message broker for billing events

### 1.2 Data Models & Storage
- [ ] Create `SubscriptionPlan` entity (name, limits, price, features)
- [ ] Create `Subscription` entity (user, plan, status, dates)
- [ ] Create `PaymentMethod` entity (type, token, default flag)
- [ ] Create `Invoice` entity (amount, status, items, dates)
- [ ] Create `UsageTracking` entity (storage, features used)
- [ ] Set up database indexes and relationships

### 1.3 External Payment Integration
- [ ] Integrate Stripe Billing SDK
- [ ] Configure webhook handlers for Stripe events
- [ ] Implement secure token storage for payment methods
- [ ] Set up test/sandbox environment

---

## Phase 2: Plans & Limits (Requirement 7.1)

### 2.1 Subscription Plan Management
- [ ] Create plan definition structure (tiers: Free, Pro, Business, Enterprise)
- [ ] Define storage limits per plan
- [ ] Define feature flags per plan
- [ ] Create plan comparison matrix
- [ ] Implement plan pricing with currency support

### 2.2 Usage Tracking
- [ ] Implement storage usage tracking service
- [ ] Create usage aggregation per account
- [ ] Build real-time usage monitoring
- [ ] Implement usage sync with external services (Galleries, etc.)

### 2.3 Limit Enforcement
- [ ] Create storage limit check service
- [ ] Implement feature access validation
- [ ] Create limit exceeded notification service
- [ ] Build graceful degradation for limit breaches

### 2.4 Upload Blocking Logic
- [ ] Create pre-upload validation endpoint
- [ ] Implement storage quota check before upload
- [ ] Return clear error messages with upgrade prompts
- [ ] Add buffer/grace period handling

### 2.5 Plans & Limits API
- [ ] Create `GET /api/plans` - List available plans
- [ ] Create `GET /api/subscriptions/current` - Current subscription
- [ ] Create `GET /api/usage` - Usage statistics
- [ ] Create `POST /api/usage/check` - Pre-operation limit check

### 2.6 Plans & Limits Tests
- [ ] Unit tests for limit calculations
- [ ] Unit tests for usage tracking
- [ ] Integration tests for limit enforcement
- [ ] Acceptance test: "Exceed storage limit" scenario

---

## Phase 3: Billing Management (Requirement 7.2)

### 3.1 Payment Method Management
- [ ] Implement add payment method flow
- [ ] Integrate Stripe SetupIntent for secure card collection
- [ ] Create payment method storage (tokenized)
- [ ] Implement set default payment method
- [ ] Create delete payment method with validation

### 3.2 Payment Method API
- [ ] Create `GET /api/billing/payment-methods` - List methods
- [ ] Create `POST /api/billing/payment-methods` - Add method
- [ ] Create `PUT /api/billing/payment-methods/{id}/default` - Set default
- [ ] Create `DELETE /api/billing/payment-methods/{id}` - Remove method
- [ ] Create `POST /api/billing/setup-intent` - Get Stripe setup intent

### 3.3 Invoice Management
- [ ] Create invoice generation service
- [ ] Implement invoice PDF generation
- [ ] Create invoice email delivery
- [ ] Build invoice history storage

### 3.4 Invoice API
- [ ] Create `GET /api/billing/invoices` - List invoices
- [ ] Create `GET /api/billing/invoices/{id}` - Invoice details
- [ ] Create `GET /api/billing/invoices/{id}/pdf` - Download PDF
- [ ] Create `POST /api/billing/invoices/{id}/resend` - Resend invoice

### 3.5 Billing Management Tests
- [ ] Unit tests for payment method operations
- [ ] Unit tests for invoice generation
- [ ] Integration tests with Stripe
- [ ] Acceptance test: "Update payment method" scenario

---

## Phase 4: Trial & Upgrade Flow (Requirement 7.3)

### 4.1 Trial Management
- [ ] Implement trial creation on signup
- [ ] Configure trial duration per plan
- [ ] Create trial status tracking
- [ ] Build trial expiration date calculation

### 4.2 Trial Expiration Handling
- [ ] Create trial expiration background job
- [ ] Implement pre-expiration notifications (7 days, 3 days, 1 day)
- [ ] Build trial expired state handling
- [ ] Create feature restriction on expiration

### 4.3 Upgrade Prompts
- [ ] Design upgrade prompt triggers
- [ ] Create contextual upgrade messages
- [ ] Implement upgrade CTA endpoints
- [ ] Build upgrade flow tracking/analytics

### 4.4 Subscription Upgrades
- [ ] Implement plan upgrade flow
- [ ] Handle proration for mid-cycle upgrades
- [ ] Create immediate access on upgrade
- [ ] Build upgrade confirmation notifications

### 4.5 Subscription Downgrades
- [ ] Implement plan downgrade flow
- [ ] Handle end-of-period downgrade scheduling
- [ ] Create limit check before downgrade
- [ ] Build downgrade warning notifications

### 4.6 Trial & Upgrade API
- [ ] Create `GET /api/subscriptions/trial-status` - Trial info
- [ ] Create `POST /api/subscriptions/upgrade` - Upgrade plan
- [ ] Create `POST /api/subscriptions/downgrade` - Downgrade plan
- [ ] Create `POST /api/subscriptions/cancel` - Cancel subscription
- [ ] Create `POST /api/subscriptions/reactivate` - Reactivate

### 4.7 Trial & Upgrade Tests
- [ ] Unit tests for trial calculations
- [ ] Unit tests for upgrade proration
- [ ] Integration tests for subscription changes
- [ ] Acceptance test: "Trial expires" scenario

---

## Phase 5: Subscription Lifecycle Management

### 5.1 Renewal Processing
- [ ] Implement automatic renewal charging
- [ ] Create renewal failure handling
- [ ] Build dunning management (retry logic)
- [ ] Implement grace period for failed payments

### 5.2 Cancellation Handling
- [ ] Create cancellation flow with reason capture
- [ ] Implement immediate vs end-of-period cancellation
- [ ] Build data retention on cancellation
- [ ] Create win-back campaigns trigger

### 5.3 Webhook Processing
- [ ] Handle `invoice.paid` webhook
- [ ] Handle `invoice.payment_failed` webhook
- [ ] Handle `customer.subscription.updated` webhook
- [ ] Handle `customer.subscription.deleted` webhook

---

## Phase 6: Advanced Features

### 6.1 Billing Portal
- [ ] Implement self-service billing portal
- [ ] Create subscription management UI endpoints
- [ ] Add payment history access
- [ ] Build receipt download functionality

### 6.2 Notifications
- [ ] Create payment successful notifications
- [ ] Implement payment failed notifications
- [ ] Build upcoming renewal reminders
- [ ] Add plan change confirmations

### 6.3 Reporting
- [ ] Create MRR (Monthly Recurring Revenue) calculations
- [ ] Build churn rate tracking
- [ ] Implement revenue forecasting
- [ ] Create billing admin dashboard endpoints

---

## Phase 7: Security & Compliance

### 7.1 PCI Compliance
- [ ] Ensure no raw card data storage
- [ ] Implement secure token handling
- [ ] Add audit logging for billing operations
- [ ] Create PCI compliance documentation

### 7.2 Data Protection
- [ ] Implement billing data encryption at rest
- [ ] Create data retention policies
- [ ] Build data export for GDPR requests
- [ ] Add billing data anonymization

---

## Dependencies

| Dependency | Type | Description |
|------------|------|-------------|
| Identity Service | Internal | User account information |
| Galleries Service | Internal | Storage usage data |
| Communication Service | Internal | Email notifications |
| Stripe | External | Payment processing & subscriptions |

---

## API Endpoints Summary

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/plans` | GET | List subscription plans |
| `/api/subscriptions/current` | GET | Current subscription details |
| `/api/subscriptions/trial-status` | GET | Trial information |
| `/api/subscriptions/upgrade` | POST | Upgrade subscription |
| `/api/subscriptions/downgrade` | POST | Downgrade subscription |
| `/api/subscriptions/cancel` | POST | Cancel subscription |
| `/api/usage` | GET | Usage statistics |
| `/api/usage/check` | POST | Pre-operation limit check |
| `/api/billing/payment-methods` | GET/POST | Manage payment methods |
| `/api/billing/payment-methods/{id}/default` | PUT | Set default method |
| `/api/billing/invoices` | GET | List invoices |
| `/api/billing/invoices/{id}/pdf` | GET | Download invoice PDF |

---

## Success Criteria

- [ ] Users are blocked from uploading when storage limit is exceeded
- [ ] Users can add, update, and manage payment methods
- [ ] Trial users are prompted to upgrade when trial expires
- [ ] Premium features are restricted after trial expiration
- [ ] All acceptance criteria from requirements.md are passing
