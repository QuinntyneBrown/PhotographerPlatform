# PhotographerPlatform.Galleries - Implementation Roadmap

## Overview
This roadmap outlines the implementation plan for the Galleries microservice, which handles gallery creation, image management, client proofing, downloads, access controls, and watermarking.

---

## Phase 1: Foundation & Infrastructure

### 1.1 Project Setup
- [ ] Initialize .NET project structure with clean architecture
- [ ] Configure dependency injection container
- [ ] Set up logging and telemetry infrastructure
- [ ] Configure database context and migrations
- [ ] Set up blob storage integration (Azure Blob/S3)

### 1.2 Data Models & Storage
- [ ] Create `Gallery` entity (project, name, cover, settings, status)
- [ ] Create `Image` entity (gallery, filename, metadata, order)
- [ ] Create `ImageSet` entity (gallery, name, images)
- [ ] Create `GalleryAccess` entity (type, password, expiration)
- [ ] Create `Favorite` entity (gallery, image, client)
- [ ] Create `Comment` entity (image, client, content)
- [ ] Set up database indexes and relationships

### 1.3 Storage Infrastructure
- [ ] Configure blob storage containers (originals, processed, thumbnails)
- [ ] Set up CDN integration for image delivery
- [ ] Implement secure signed URL generation
- [ ] Create storage cleanup policies

---

## Phase 2: Gallery Creation (Requirement 3.1)

### 2.1 Gallery CRUD Operations
- [ ] Implement gallery creation with project linking
- [ ] Add gallery metadata (name, description, date)
- [ ] Create gallery update functionality
- [ ] Implement gallery deletion with cleanup
- [ ] Add gallery archiving

### 2.2 Cover Image Management
- [ ] Implement cover image selection
- [ ] Create auto-select first image as cover
- [ ] Generate cover image thumbnails
- [ ] Store cover image reference

### 2.3 Gallery API
- [ ] Create `POST /api/galleries` - Create gallery
- [ ] Create `GET /api/galleries/{id}` - Get gallery details
- [ ] Create `PUT /api/galleries/{id}` - Update gallery
- [ ] Create `DELETE /api/galleries/{id}` - Delete gallery
- [ ] Create `PUT /api/galleries/{id}/cover` - Set cover image

### 2.4 Gallery Creation Tests
- [ ] Unit tests for gallery operations
- [ ] Integration tests for storage
- [ ] Acceptance test: "Create a gallery with cover" scenario

---

## Phase 3: Image Upload & Organization (Requirement 3.2)

### 3.1 Single Image Upload
- [ ] Implement file upload endpoint
- [ ] Add file type validation (JPEG, PNG, RAW)
- [ ] Create image metadata extraction (EXIF)
- [ ] Generate thumbnails (small, medium, large)
- [ ] Store original and processed versions

### 3.2 Bulk Upload
- [ ] Implement multi-file upload endpoint
- [ ] Create chunked upload for large files
- [ ] Add upload progress tracking
- [ ] Implement parallel processing
- [ ] Maintain upload order

### 3.3 Image Organization
- [ ] Implement drag-and-drop reordering
- [ ] Create batch reorder endpoint
- [ ] Add image metadata editing
- [ ] Implement image deletion

### 3.4 Image Sets (Grouping)
- [ ] Create image set management
- [ ] Implement add images to set
- [ ] Create set reordering
- [ ] Build set deletion with image handling

### 3.5 Image Upload API
- [ ] Create `POST /api/galleries/{id}/images` - Upload images
- [ ] Create `POST /api/galleries/{id}/images/bulk` - Bulk upload
- [ ] Create `PUT /api/galleries/{id}/images/order` - Reorder
- [ ] Create `DELETE /api/images/{id}` - Delete image
- [ ] Create `POST /api/galleries/{id}/sets` - Create set
- [ ] Create `PUT /api/sets/{id}/images` - Manage set images

### 3.6 Image Upload Tests
- [ ] Unit tests for upload processing
- [ ] Unit tests for image organization
- [ ] Integration tests for bulk upload
- [ ] Acceptance test: "Bulk upload images" scenario

---

## Phase 4: Proofing & Favorites (Requirement 3.3)

### 4.1 Client Favorites
- [ ] Implement favorite toggle endpoint
- [ ] Create favorites list per client per gallery
- [ ] Add favorite count tracking
- [ ] Implement favorites export

### 4.2 Image Comments
- [ ] Create comment submission endpoint
- [ ] Implement comment threading (optional)
- [ ] Add comment notifications
- [ ] Create comment moderation tools

### 4.3 Proofing Workflow
- [ ] Implement proofing mode toggle
- [ ] Create selection limits (if configured)
- [ ] Add proofing deadline support
- [ ] Build proofing completion notification

### 4.4 Proofing API
- [ ] Create `POST /api/images/{id}/favorite` - Toggle favorite
- [ ] Create `GET /api/galleries/{id}/favorites` - List favorites
- [ ] Create `POST /api/images/{id}/comments` - Add comment
- [ ] Create `GET /api/images/{id}/comments` - List comments
- [ ] Create `DELETE /api/comments/{id}` - Delete comment

### 4.5 Proofing Tests
- [ ] Unit tests for favorites
- [ ] Unit tests for comments
- [ ] Integration tests for proofing workflow
- [ ] Acceptance test: "Client favorites an image" scenario

---

## Phase 5: Downloads & Permissions (Requirement 3.4)

### 5.1 Download Permission System
- [ ] Implement permission levels (none, web-size, full-size)
- [ ] Create per-gallery download settings
- [ ] Add per-image download overrides
- [ ] Implement download expiration dates

### 5.2 Image Processing for Downloads
- [ ] Generate web-size versions (configurable dimensions)
- [ ] Maintain full-size originals
- [ ] Implement on-demand processing
- [ ] Create download quality settings

### 5.3 Download Delivery
- [ ] Create single image download endpoint
- [ ] Implement bulk download (ZIP generation)
- [ ] Add download tracking/logging
- [ ] Create secure download links

### 5.4 Download API
- [ ] Create `GET /api/images/{id}/download` - Download image
- [ ] Create `POST /api/galleries/{id}/download` - Bulk download
- [ ] Create `PUT /api/galleries/{id}/download-settings` - Settings
- [ ] Create `GET /api/galleries/{id}/download-status` - Check permissions

### 5.5 Download Tests
- [ ] Unit tests for permission checking
- [ ] Unit tests for image processing
- [ ] Integration tests for download delivery
- [ ] Acceptance test: "Client downloads web-size image" scenario

---

## Phase 6: Gallery Access Controls (Requirement 3.5)

### 6.1 Access Level Implementation
- [ ] Implement public gallery access
- [ ] Create unlisted gallery (link-only) access
- [ ] Build password-protected gallery access
- [ ] Add private gallery (invite-only) access

### 6.2 Password Protection
- [ ] Create password setting endpoint
- [ ] Implement secure password hashing
- [ ] Build password verification flow
- [ ] Add session/cookie for authenticated access
- [ ] Create password reset functionality

### 6.3 Access Expiration
- [ ] Implement gallery expiration dates
- [ ] Create expiration warnings
- [ ] Build expired gallery handling
- [ ] Add extension functionality

### 6.4 Access Control API
- [ ] Create `PUT /api/galleries/{id}/access` - Set access level
- [ ] Create `PUT /api/galleries/{id}/password` - Set password
- [ ] Create `POST /api/galleries/{id}/verify-password` - Verify
- [ ] Create `PUT /api/galleries/{id}/expiration` - Set expiration

### 6.5 Access Control Tests
- [ ] Unit tests for access levels
- [ ] Unit tests for password verification
- [ ] Integration tests for access flow
- [ ] Acceptance test: "Client enters correct password" scenario

---

## Phase 7: Watermarking (Requirement 3.6)

### 7.1 Watermark Configuration
- [ ] Create watermark upload/management
- [ ] Implement watermark positioning (9-point grid)
- [ ] Add watermark opacity settings
- [ ] Create watermark size/scaling options

### 7.2 Watermark Application
- [ ] Implement watermark processing pipeline
- [ ] Apply watermarks to preview images
- [ ] Exclude watermarks from purchased/downloaded images
- [ ] Add per-gallery watermark toggle

### 7.3 Watermark API
- [ ] Create `POST /api/watermarks` - Upload watermark
- [ ] Create `GET /api/watermarks` - List watermarks
- [ ] Create `PUT /api/galleries/{id}/watermark` - Gallery watermark settings
- [ ] Create `DELETE /api/watermarks/{id}` - Delete watermark

### 7.4 Watermark Tests
- [ ] Unit tests for watermark processing
- [ ] Integration tests for watermark application
- [ ] Acceptance test: "View watermarked image" scenario

---

## Phase 8: CDN & Image Optimization (Requirement 11.1)

### 8.1 CDN Integration
- [ ] Configure CDN for image delivery
- [ ] Implement cache headers
- [ ] Create cache invalidation on updates
- [ ] Set up geographic distribution

### 8.2 Responsive Image Delivery
- [ ] Implement srcset generation
- [ ] Create device-aware sizing
- [ ] Add WebP/AVIF format support
- [ ] Build lazy loading support

### 8.3 Optimization Tests
- [ ] Performance tests for image delivery
- [ ] Acceptance test: "Serve optimized images" scenario

---

## Phase 9: Backup & Recovery (Requirement 11.2)

### 9.1 Data Backup
- [ ] Implement database backup strategy
- [ ] Create blob storage backup/replication
- [ ] Set up automated backup schedules
- [ ] Build backup verification

### 9.2 Recovery Procedures
- [ ] Create point-in-time recovery
- [ ] Implement gallery restoration
- [ ] Build disaster recovery runbooks
- [ ] Test recovery procedures

### 9.3 Backup Tests
- [ ] Acceptance test: "Restore from backup" scenario

---

## Dependencies

| Dependency | Type | Description |
|------------|------|-------------|
| Workspace Service | Internal | Project information |
| Identity Service | Internal | Client authentication |
| Analytics Service | Internal | View/download tracking |
| Store Service | Internal | Purchase verification |
| Blob Storage | External | Image file storage |
| CDN | External | Image delivery |
| Image Processing | External | ImageMagick/SixLabors |

---

## API Endpoints Summary

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/galleries` | POST | Create gallery |
| `/api/galleries/{id}` | GET/PUT/DELETE | Gallery CRUD |
| `/api/galleries/{id}/cover` | PUT | Set cover image |
| `/api/galleries/{id}/images` | POST | Upload images |
| `/api/galleries/{id}/images/order` | PUT | Reorder images |
| `/api/galleries/{id}/sets` | GET/POST | Manage sets |
| `/api/galleries/{id}/favorites` | GET | List favorites |
| `/api/galleries/{id}/access` | PUT | Set access level |
| `/api/galleries/{id}/password` | PUT | Set password |
| `/api/galleries/{id}/watermark` | PUT | Watermark settings |
| `/api/galleries/{id}/download` | POST | Bulk download |
| `/api/images/{id}` | GET/DELETE | Image operations |
| `/api/images/{id}/favorite` | POST | Toggle favorite |
| `/api/images/{id}/comments` | GET/POST | Comments |
| `/api/images/{id}/download` | GET | Download image |

---

## Success Criteria

- [ ] Galleries can be created with cover images linked to projects
- [ ] Multiple images can be uploaded and appear in order
- [ ] Clients can favorite images and see their favorites list
- [ ] Download permissions correctly enforce web-size vs full-size
- [ ] Password-protected galleries require correct password
- [ ] Watermarks appear on preview images when enabled
- [ ] Images are delivered optimized for device size
- [ ] All acceptance criteria from requirements.md are passing
