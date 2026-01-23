# PhotographerPlatform.Websites - Implementation Roadmap

## Overview
This roadmap outlines the implementation plan for the Websites service, which provides the marketing site and portfolio website builder features (templates, editor, publishing), plus custom domains, SEO/social sharing, accessibility, and localization.

Primary requirements source: `requirements.md` in this folder.

---

## Phase 1: Foundation & Architecture

### 1.1 Project Setup
- [ ] Confirm service boundaries (marketing site vs portfolio sites vs builder/editor APIs)
- [ ] Establish clean architecture (Domain/Application/Infrastructure/Api)
- [ ] Add structured logging, tracing, and metrics
- [ ] Add health checks (liveness/readiness) and dependency checks
- [ ] Add configuration model (multi-tenant, per-site settings)

### 1.2 Multi-Tenancy & Routing Model
- [ ] Define tenant identifiers (account/workspace/site)
- [ ] Define site identifiers and routing strategy (subdomain + custom domain)
- [ ] Implement host-based routing and site resolution middleware
- [ ] Add tenant/site scoping for all queries and caches

### 1.3 Data Model
- [ ] Create `Site` entity (owner, status, primary domain, theme, locale)
- [ ] Create `Page` entity (site, slug, title, layout, draft/published)
- [ ] Create `ContentBlock` entity (page, type, props, ordering)
- [ ] Create `Template` entity (name, version, default blocks)
- [ ] Create `DomainBinding` entity (site, hostname, verification, ssl status)
- [ ] Create `Asset` entity (site, storage key, mime, dimensions)

### 1.4 Storage & Delivery
- [ ] Set up asset storage (images, fonts, uploads) with CDN fronting
- [ ] Add cache headers strategy for published assets/pages
- [ ] Implement signed URLs for private builder uploads (if needed)
- [ ] Add purge/invalidation hooks for publish events

---

## Phase 2: Marketing Website (Requirement 1.1)

### 2.1 Information Architecture
- [ ] Define top-level routes: Home, Products, Pricing, Examples, Sign Up
- [ ] Build navigation components and layout shell
- [ ] Add content model and rendering pipeline (static or CMS-backed)

### 2.2 Marketing Pages
- [ ] Implement Products page (feature highlights, plan comparison links)
- [ ] Implement Pricing page (plans, limits, CTA)
- [ ] Implement Examples page (portfolio examples / customer stories)
- [ ] Implement Sign Up entry point (handoff to auth flow)

### 2.3 Marketing Site Tests
- [ ] Acceptance test: “Marketing website navigation” scenario
- [ ] Accessibility checks for marketing flows
- [ ] Basic SEO checks (metadata present, crawlability)

---

## Phase 3: Authentication UX Integration (Requirement 1.2)

> Note: If authentication is owned by the Identity service, this service should still provide the user-facing website flows and integrate with Identity APIs.

### 3.1 Auth Flows
- [ ] Sign up UI (email/password or configured provider)
- [ ] Sign in UI
- [ ] Password reset request + reset completion
- [ ] Session management (refresh, logout everywhere, remember-me if required)

### 3.2 Auth Integration
- [ ] Wire up OIDC/OAuth client and token/session storage
- [ ] Add CSRF protections for web flows
- [ ] Add rate limiting / bot protections on sign-in and sign-up

### 3.3 Auth Tests
- [ ] Acceptance test: “New user signs up” scenario
- [ ] Negative tests (invalid creds, locked user, reset token expiry)

---

## Phase 4: Role-Based Access UI Enforcement (Requirement 1.3)

### 4.1 Role Model & Policies
- [ ] Define roles: Owner, Admin, Collaborator, Client
- [ ] Implement role-based routing guards (server-side + client-side where applicable)
- [ ] Implement authorization policies for builder actions (publish, domain binding, template edit)

### 4.2 RBAC Tests
- [ ] Acceptance test: “Collaborator access limits” scenario
- [ ] Unit tests for policy mapping

---

## Phase 5: Website Builder (Requirement 5.1)

### 5.1 Template System
- [ ] Implement template catalog (listing, preview)
- [ ] Create template instantiation into a new `Site`
- [ ] Add template versioning strategy (migration for older sites)

### 5.2 Visual Editor (Core)
- [ ] Build page editor API (get draft, patch blocks, reorder)
- [ ] Build block library contract (text, image, gallery embed, CTA, contact)
- [ ] Implement autosave and draft history snapshots
- [ ] Implement preview rendering (draft vs published)

### 5.3 Publishing Pipeline
- [ ] Implement publish action: validate → render → persist published version
- [ ] Add publish rollback (restore previous published snapshot)
- [ ] Add audit trail for publish events

### 5.4 Website Builder API
- [ ] `POST /api/sites` (create from template)
- [ ] `GET /api/sites/{siteId}` (site details)
- [ ] `GET /api/sites/{siteId}/pages` (list pages)
- [ ] `POST /api/sites/{siteId}/pages` (create page)
- [ ] `PATCH /api/pages/{pageId}` (edit draft)
- [ ] `POST /api/sites/{siteId}/publish` (publish)

### 5.5 Builder Tests
- [ ] Acceptance test: “Publish a template site” scenario
- [ ] Integration tests for publish pipeline and CDN invalidation

---

## Phase 6: Custom Domains & SSL (Requirement 5.2)

### 6.1 Domain Verification
- [ ] Implement add-domain flow (hostname capture)
- [ ] Provide DNS verification instructions (TXT/CNAME)
- [ ] Implement verification checks and status polling

### 6.2 SSL Provisioning
- [ ] Integrate with certificate management (ACME or managed provider)
- [ ] Implement provisioning state machine (pending/valid/renewal/failure)
- [ ] Implement automatic renewals and alerts on failure

### 6.3 Traffic Routing
- [ ] Configure host routing to resolve custom domains to `Site`
- [ ] Enforce HTTPS and HSTS for custom domains
- [ ] Implement redirects (www/non-www) per site settings

### 6.4 Custom Domain Tests
- [ ] Acceptance test: “Connect a custom domain” scenario
- [ ] Integration tests with mocked DNS/ACME provider

---

## Phase 7: SEO & Social Sharing (Requirement 5.3)

### 7.1 SEO Metadata
- [ ] Per-page title/description
- [ ] Canonical URLs and structured metadata
- [ ] Sitemap generation (per site)
- [ ] Robots controls (noindex for drafts)

### 7.2 Social Previews
- [ ] OpenGraph tags (title/description/image)
- [ ] Twitter/X card tags
- [ ] Social preview image generation (template-based)

### 7.3 SEO Tests
- [ ] Acceptance test: “Update SEO metadata” scenario
- [ ] Automated checks for sitemap/canonical correctness

---

## Phase 8: Accessibility & Localization (Requirements 12.1, 12.2)

### 8.1 Accessibility (WCAG 2.1 AA)
- [ ] Keyboard navigation for editor and published sites
- [ ] ARIA labeling patterns for block library
- [ ] Color contrast and focus states
- [ ] Image alt-text support in editor

### 8.2 Localization
- [ ] Locale-aware formatting utilities (dates/numbers/currency)
- [ ] Site-level locale settings
- [ ] Translatable strings strategy (resource files)

### 8.3 A11y & i18n Tests
- [ ] Acceptance test: “Keyboard navigation” scenario
- [ ] Acceptance test: “Display localized dates” scenario

---

## Phase 9: Operability, Performance, and Security

### 9.1 Observability
- [ ] Dashboards for publish latency, render errors, domain provisioning failures
- [ ] Tracing across publish pipeline dependencies

### 9.2 Performance
- [ ] Page render performance budgets
- [ ] Caching strategy for public pages
- [ ] Load tests for high-traffic marketing pages

### 9.3 Security
- [ ] Content sanitization for rich text blocks
- [ ] Rate limit sensitive endpoints
- [ ] Threat model for custom domains and host header attacks

---

## Dependencies & Integration Points
- Identity: authentication and role claims (OIDC/OAuth)
- Storage/CDN: assets, images, cache invalidation
- Workspace/Galleries/Store (optional): embed content blocks that reference galleries/products
