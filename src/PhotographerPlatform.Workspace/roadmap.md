a# PhotographerPlatform.Workspace - Implementation Roadmap

## Overview
This roadmap outlines the implementation plan for the Workspace service, which supports the photographer dashboard domain: project management, client management (CRM), and search/filtering.

Primary requirements source: `requirements.md` in this folder.

---

## Phase 1: Foundation & Architecture

### 1.1 Project Setup
- [ ] Confirm bounded context (projects, clients, notes, tags) and service API shape
- [ ] Establish clean architecture (Domain/Application/Infrastructure/Api)
- [ ] Add structured logging, tracing, and metrics
- [ ] Add health checks (liveness/readiness)
- [ ] Add database migrations and CI gate for schema drift

### 1.2 Data Model
- [ ] Create `Project` entity (name, date, location, status, archived, tags)
- [ ] Create `Client` entity (name, email, phone, address)
- [ ] Create `ClientNote` entity (client, content, createdBy)
- [ ] Create `ProjectClient` relationship (primary + additional contacts)
- [ ] Create `Tag` entity (tenant-scoped)
- [ ] Add auditing fields (created/updated by, timestamps)

### 1.3 Authorization & Tenant Scoping
- [ ] Implement tenant scoping middleware (workspace/account)
- [ ] Implement authorization policies for roles (Owner/Admin/Collaborator)
- [ ] Add “client” access model where applicable (read-only / limited)

---

## Phase 2: Project Management (Requirement 2.1)

### 2.1 Project CRUD
- [ ] Create project (required fields validation)
- [ ] Edit project metadata (client, date, location, tags)
- [ ] Archive/unarchive project
- [ ] Soft-delete strategy (if needed) with restore window

### 2.2 Project List & Views
- [ ] List projects with pagination and sorting
- [ ] Add status filters (active/archived)
- [ ] Add tag filters (multi-select)

### 2.3 Project API
- [ ] `POST /api/projects`
- [ ] `GET /api/projects` (paging, sort, filters)
- [ ] `GET /api/projects/{projectId}`
- [ ] `PUT /api/projects/{projectId}`
- [ ] `POST /api/projects/{projectId}/archive`
- [ ] `POST /api/projects/{projectId}/unarchive`

### 2.4 Project Tests
- [ ] Acceptance test: “Create a new project” scenario
- [ ] Unit tests for validation and authorization
- [ ] Integration tests for paging and filtering

---

## Phase 3: Client Management (CRM) (Requirement 2.2)

### 3.1 Client CRUD
- [ ] Create client profile
- [ ] Update contact details
- [ ] Merge duplicate clients (optional but high value)
- [ ] Archive client (optional)

### 3.2 Notes & History
- [ ] Add internal notes to client profile
- [ ] Enforce visibility rules (internal only)
- [ ] Expose project history for a client (projects list by client)

### 3.3 Client API
- [ ] `POST /api/clients`
- [ ] `GET /api/clients` (paging, search)
- [ ] `GET /api/clients/{clientId}`
- [ ] `PUT /api/clients/{clientId}`
- [ ] `POST /api/clients/{clientId}/notes`
- [ ] `GET /api/clients/{clientId}/notes`
- [ ] `GET /api/clients/{clientId}/projects`

### 3.4 CRM Tests
- [ ] Acceptance test: “Update client contact details” scenario
- [ ] Unit tests for notes permissions

---

## Phase 4: Search & Filters (Requirement 2.3)

### 4.1 Search Scope
- [ ] Define searchable fields (project name, client name, tags, location)
- [ ] Define unified search response model (projects + clients + galleries ref ids)

### 4.2 Indexing Strategy
- [ ] Start with database-based search (LIKE/FTS depending on DB)
- [ ] Introduce background indexing hooks (on project/client create/update)
- [ ] Optional: adopt dedicated search engine (Elastic/Azure AI Search) when needed

### 4.3 Filtering
- [ ] Tag filters
- [ ] Date range filters
- [ ] Multi-attribute filtering rules (AND/OR)

### 4.4 Search API
- [ ] `GET /api/search?q=...` (unified)
- [ ] `GET /api/projects?tag=...&dateFrom=...&dateTo=...`
- [ ] `GET /api/clients?q=...`

### 4.5 Search Tests
- [ ] Acceptance test: “Filter projects by tag” scenario
- [ ] Integration tests for combined filters

---

## Phase 5: Reliability, Observability, and Integrations

### 5.1 Observability
- [ ] Metrics for request latency and search query timings
- [ ] Traces across DB and any external services

### 5.2 Data Integrity
- [ ] Enforce uniqueness constraints where appropriate (tenant + slug/name)
- [ ] Add optimistic concurrency for edits (ETags/rowversion)

### 5.3 Integration Points
- [ ] Publish domain events (ProjectCreated/Updated, ClientUpdated) for other services
- [ ] Accept references to other domains (Galleries, Store orders) via IDs
