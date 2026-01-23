# Shared - Implementation Roadmap

## Overview
This roadmap covers the Shared project, intended to host cross-cutting platform capabilities and shared libraries used by multiple services (e.g., integrations primitives, webhook framework, resilience utilities, CDN/image helpers, and backup/restore automation).

Primary requirements source: `requirements.md` in this folder.

---

## Phase 1: Shared Package Structure & Governance

### 1.1 Project Organization
- [ ] Define Shared library boundaries (no domain coupling; reusable primitives only)
- [ ] Establish versioning strategy (internal packages vs project references)
- [ ] Add CI checks (build, test, static analysis)

### 1.2 Cross-Cutting Building Blocks
- [ ] Standardize error model (problem details) and exception mapping helpers
- [ ] Standardize correlation IDs and tracing helpers
- [ ] Provide common HTTP clients with resilience defaults (timeouts, retries, circuit breakers)
- [ ] Provide common auth helpers (claims access, policy helpers)

---

## Phase 2: Integrations & Automation Primitives (Requirements 10.1, 10.2, 10.3)

### 2.1 Payment Provider Abstractions (Req 10.1)
- [ ] Define payment provider interface contracts (authorize, capture, refund)
- [ ] Define common request/response DTOs and error mapping
- [ ] Add idempotency key helper and signature verification helpers
- [ ] Provide provider configuration model and secret-loading patterns

### 2.2 Lab Fulfillment Abstractions (Req 10.2)
- [ ] Define print lab interface contracts (quote, submit order, status, cancel)
- [ ] Define standardized fulfillment status model
- [ ] Provide webhook signature verification helpers (for lab callbacks)

### 2.3 API Keys & Webhooks Framework (Req 10.3)
- [ ] Define API key model (hashing, rotation, scoping)
- [ ] Implement webhook subscription model (event types, target URL, secret)
- [ ] Implement webhook dispatcher (retries, backoff, dead-letter)
- [ ] Provide event envelope standard (id, type, occurredAt, tenant, payload)
- [ ] Provide webhook signing helper (HMAC) and verification guide utilities

### 2.4 Integrations Tests
- [ ] Unit tests for HMAC signing/verification
- [ ] Unit tests for retry/backoff policies
- [ ] Contract tests for event envelope serialization

---

## Phase 3: CDN & Image Optimization Helpers (Requirement 11.1)

### 3.1 CDN Utilities
- [ ] Standardize cache-control header helpers
- [ ] Provide URL builder for responsive image variants (size/format)
- [ ] Provide signed URL generation helpers where needed

### 3.2 Image Optimization Utilities
- [ ] Define image variant naming conventions
- [ ] Provide simple image resizing/format conversion abstraction (implementation left to service)
- [ ] Provide metadata extraction helpers (EXIF parsing utilities) if broadly used

### 3.3 Tests
- [ ] Unit tests for URL generation
- [ ] Unit tests for cache header policies

---

## Phase 4: Availability & Backups Automation (Requirement 11.2)

### 4.1 Backup/Restore Automation
- [ ] Define backup execution abstraction (DB dump, blob snapshot, config export)
- [ ] Provide backup retention policy utilities
- [ ] Provide restore playbook templates (runbook markdown templates)

### 4.2 Reliability Primitives
- [ ] Health check helpers (DB, storage, message broker)
- [ ] Startup validation helpers (fail fast on missing config)
- [ ] Graceful shutdown helper patterns (drain, stop accepting traffic)

### 4.3 Tests
- [ ] Unit tests for retention policy computations
- [ ] Integration test harness hooks (if feasible) for backup commands

---

## Phase 5: Documentation & Adoption

### 5.1 Usage Guides
- [ ] Document how to add a webhook event type
- [ ] Document how to onboard a new payment provider implementation
- [ ] Document how to integrate CDN helper patterns

### 5.2 Migration & Rollout
- [ ] Identify duplicated logic across services and replace with Shared utilities
- [ ] Add deprecation notes for removed per-service helpers
